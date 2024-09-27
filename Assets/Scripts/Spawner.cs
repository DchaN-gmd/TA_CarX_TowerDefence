using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool _isStartOnAwake;
    [SerializeField, Min(0)] private float _spawnInterval = 3;

    [Header("References")]
    [SerializeField] private SimpleEnemyPool _enemyPool;
    [SerializeField] private Transform moveTarget;

    private Transform _transform;
    private Coroutine _coroutine;

    private void Awake()
    {
        _transform = transform;

        if (_isStartOnAwake) StartSpawn();
    }

    public void StartSpawn()
    {
        if (_coroutine != null) return;

        _coroutine = StartCoroutine(Spawning());
    }

    public void StopSpawn()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator Spawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            Spawn();
        }
    }

    private void Spawn()
    {
        var enemy = _enemyPool.GetObject();

        enemy.Release += OnEnemyReleased;
        enemy.transform.position = _transform.position;
        enemy.UpdateHealth();
        enemy.SetTarget(moveTarget);
        enemy.SetMove(true);
    }

    private void OnEnemyReleased(Enemy enemy)
    {
        enemy.Dieing -= OnEnemyReleased;
        enemy.SetMove(false);
        _enemyPool.ReturnObjectToPool(enemy);
    }
}
