using Redcode.Pools;
using Unity.FPS.AI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    public EnemyController NormalEnemyPrefab;
    public EnemyController EliteEnemyPrefab;
    public EnemyController BossEnemyPrefab;

    [Header("Spawn Settings")]
    public int MaxEnemies = 10;
    public float SpawnInterval = 3f;
    public PatrolPath AssignedPath;

    private float _spawnTimer = 0f;
    private int _enemyCount = 0;

    private Pool<EnemyController> _normalPool;
    private Pool<EnemyController> _elitePool;
    private Pool<EnemyController> _bossPool;

    private DifficultyManager _difficultyManager;

    // 난이도 반영용 변수
    private float _eliteSpawnChance = 0.1f;
    private int _bossAsNormalCount = 0;

    private void Start()
    {
        EnemyController.OnEnemyDied += OnEnemyDied;

        _normalPool = Pool.Create(NormalEnemyPrefab, MaxEnemies, transform).NonLazy();
        _elitePool = Pool.Create(EliteEnemyPrefab, MaxEnemies, transform).NonLazy();
        _bossPool = Pool.Create(BossEnemyPrefab, MaxEnemies, transform).NonLazy();

        _difficultyManager = GameManager.Instance.GetComponent<DifficultyManager>();

        if (_difficultyManager == null)
        {
            Debug.LogError("[EnemySpawner] DifficultyManager를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // 난이도 변경 이벤트 구독
        GameManager.Instance.Events.Difficulty.OnTierChanged += OnDifficultyUpdated;
        //GameManager.Instance.Events.Stage.OnStageChanged += OnStageChanged;

        // 초기값 세팅
        RefreshDifficultySettings();
    }

    private void OnDestroy()
    {
        EnemyController.OnEnemyDied -= OnEnemyDied;
        GameManager.Instance.Events.Difficulty.OnTierChanged -= OnDifficultyUpdated;
        //GameManager.Instance.Events.Stage.OnStageChanged -= OnStageChanged;
    }
    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= SpawnInterval)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyCount >= MaxEnemies)
            return;

        EnemyController enemy;

        if (Random.value <= _eliteSpawnChance)
            enemy = _elitePool.Get();
        else
            enemy = _normalPool.Get();

        if (enemy != null)
        {
            SetupEnemy(enemy);
        }
    }

    private void SpawnInitialBosses()
    {
        for (int i = 0; i < _bossAsNormalCount; i++)
        {
            var boss = _bossPool.Get();
            if (boss != null)
            {
                SetupEnemy(boss);
            }
        }

        Debug.Log($"[EnemySpawner] 스테이지 시작 시 보스 {_bossAsNormalCount}마리 소환됨.");
    }

    private void SetupEnemy(EnemyController enemy)
    {
        enemy.transform.position = transform.position;
        // enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.PatrolPath = AssignedPath;
        enemy.SetPool(GetPoolByType(enemy));

        // 난이도 반영된 스탯 갱신
        //enemy.RefreshStats();

        _enemyCount++;
    }

    private Pool<EnemyController> GetPoolByType(EnemyController enemy)
    {
        if (enemy.EnemyType == EEnemyType.Normal)
        {
            return _normalPool;
        }
        if (enemy.EnemyType == EEnemyType.Glasses)
        {
            return _elitePool;
        }
        if (enemy.EnemyType==EEnemyType.Boss)
        {
            return _bossPool;
        }
        Debug.LogError($"[EnemySpawner] 알 수 없는 적 타입: {enemy.EnemyType}");
        return null;
    }

    public void OnEnemyDied()
    {
        _enemyCount--;
    }

    private void OnDifficultyUpdated(DifficultyDTO dto)
    {
        RefreshDifficultySettings();

        // 이미 생성된 적들 상태도 갱신
        foreach (var enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
        {
            // 적의 스탯을 난이도에 맞게 갱신
            //enemy.EnemyStatProvider.RefreshStats();
        }
    }

    private void RefreshDifficultySettings()
    {
        _eliteSpawnChance = _difficultyManager.GetEliteSpawnChance();
        _bossAsNormalCount = _difficultyManager.GetBossAsNormalEnemyCount();
    }

    private void OnStageChanged(StageDTO stage)
    {
        SpawnInitialBosses();
    }
}
