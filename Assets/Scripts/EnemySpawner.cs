using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private BoxCollider _spawnArea;

    [SerializeField, Range(0.1f, 10.0f)]
    private float _spawnInterval = 3.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float _releaseAfterDeathTime = 5.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float _releaseInterval = 1.0f;

    private ObjectPool<Enemy> _pool;
    private HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
        StartCoroutine(ReleaseDeadEnemiesCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(OnCreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy);

        Debug.Assert(_enemyPrefab.GetComponentInChildren<Enemy>() != null, "Enemy prefab must have an Enemy component attached");
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while(enabled)
        {
            Enemy enemy = _pool.Get();

            float x = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
            float z = Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z);
            enemy.transform.position = new Vector3(x, 0.0f, z);
            enemy.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private IEnumerator ReleaseDeadEnemiesCoroutine()
    {
        List<Enemy> enemiesToRelease = new List<Enemy>();

        while(enabled)
        {
            foreach(Enemy enemy in _activeEnemies)
            {
                if(enemy.DeathTime != 0.0f && Time.time - enemy.DeathTime > _releaseAfterDeathTime)
                    enemiesToRelease.Add(enemy);
            }

            // Using a second loop to avoid modifying _activeEnemies while iterating over it
            foreach(Enemy enemy in enemiesToRelease)
                _pool.Release(enemy);

            enemiesToRelease.Clear();

            yield return new WaitForSeconds(_releaseInterval);
        }
    }

    private Enemy OnCreateEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab, transform);
        return enemy.GetComponentInChildren<Enemy>();
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void OnGetEnemy(Enemy enemy)
    {
        enemy.ResetState();
        enemy.transform.SetParent(null);
        enemy.gameObject.SetActive(true);
        _activeEnemies.Add(enemy);
    }

    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.transform.SetParent(transform);
        enemy.gameObject.SetActive(false);
        _activeEnemies.Remove(enemy);
    }
}
