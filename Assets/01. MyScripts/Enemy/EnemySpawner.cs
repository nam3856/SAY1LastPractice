using Redcode.Pools;
using Unity.FPS.AI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController EnemyPrefab;
    public int MaxEnemies = 10;
    public float SpawnInterval = 3f;
    private float _spawnTimer = 0f;
    private int _enemyCount = 0;
    private Pool<EnemyController> _enemyPool;
    public PatrolPath AssignedPath;
    private void Start()
    {
        EnemyController.OnEnemyDied += OnEnemyDied;
        _enemyPool = Pool.Create(EnemyPrefab, MaxEnemies, transform).NonLazy();
    }

    private void OnDestroy()
    {
        EnemyController.OnEnemyDied -= OnEnemyDied;
    }
    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if(_spawnTimer >= SpawnInterval)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (_enemyCount >= MaxEnemies)
            return;

        var enemy = _enemyPool.Get();
        if (enemy != null)
        {
            enemy.transform.position = transform.position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.PatrolPath = AssignedPath;
            enemy.SetPool(_enemyPool);
            _enemyCount++;
        }
    }

    public void OnEnemyDied()
    {
        _enemyCount--;
    }
}
