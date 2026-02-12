using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

public class SpawnManager : MonoBehaviour
{
    private Enemy.Pool _enemyPool;

    [Header("Spawn Quantity & Timing")]
    [SerializeField] private int _spawnCount;
    [SerializeField] private float _spawnDuration;
    [Header("Spawn Area Boundaries")]
    [SerializeField] private float _minXPosition;
    [SerializeField] private float _maxXPosition;
    [SerializeField] private float _minZPosition;
    [SerializeField] private float _maxZPosition;

    [Inject]
    public void Construct(Enemy.Pool pool) => _enemyPool = pool;
    private void Start()
    {
        StartSpawning().Forget();
    }
    private async UniTaskVoid StartSpawning()
    {
        var token = this.GetCancellationTokenOnDestroy();

        for (int i = 0; i < _spawnCount; i++)
        {
            var newEnemy = _enemyPool.Spawn(_enemyPool);
            newEnemy.transform.position = GetRandomPosition();

            await UniTask.Delay(TimeSpan.FromSeconds(_spawnDuration), cancellationToken: token);
        }
    }
    private Vector3 GetRandomPosition()
    {
        var randomX = UnityEngine.Random.Range(_minXPosition, _maxXPosition);
        var randomZ = UnityEngine.Random.Range(_minZPosition, _maxZPosition);
        var randomPosition = new Vector3(randomX, 0f, randomZ);

        return randomPosition;
    }
}
