using Unity.FPS.AI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public int MaxEnemies = 10;
    public float SpawnInterval = 3f;
    private float _spawnTimer = 0f;
    private int _enemyCount = 0;
    private EnemyManager _enemyManager;
    private void Start()
    {
        EnemyController.OnEnemyDied += OnEnemyDied;
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
        if (_enemyCount < MaxEnemies)
        {
            var enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
            _enemyCount++;
        }
    }

    public void OnEnemyDied()
    {
        _enemyCount--;
    }
}
