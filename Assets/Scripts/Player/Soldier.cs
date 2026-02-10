using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class Soldier : MonoBehaviour, IPoolable<IMemoryPool>
{
    private IMemoryPool _pool;
   
    private PlayerController _playerController;

    private Bullet.Pool _bulletPool;

    [Inject]
    public void Construct(Bullet.Pool bulletPool, PlayerController playerController)
    {
        _bulletPool = bulletPool;
        _playerController = playerController;
    }
    private void OnEnable() => _playerController.OnPlayerAttacked += PlayerController_OnPlayerAttacked;
    private void OnDisable() => _playerController.OnPlayerAttacked -= PlayerController_OnPlayerAttacked;
    private void PlayerController_OnPlayerAttacked() => HandleFireSequence().Forget();
    private async UniTaskVoid HandleFireSequence()
    {
        var token = this.GetCancellationTokenOnDestroy();

        for (int i = 0; i < _playerController.Data.BulletCount; i++)
        {
            //StartAttacking
            var bullet = _bulletPool.Spawn(
                _playerController.Data.BulletDamage,
                _playerController.Data.BulletLifeTime, 
                _playerController.Data.BulletSpeed,
                transform.position + _playerController.Data.AttackOffset,
                _bulletPool);

            bullet.transform.SetParent(null);
            await UniTask.Delay(TimeSpan.FromSeconds(_playerController.Data.FireRate), cancellationToken: token);
        }
        //Reloading
        await UniTask.Delay(TimeSpan.FromSeconds(_playerController.Data.ReloadTime), cancellationToken : token);

        _playerController.SetAttackStatus(true);
    }
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }
    public void OnDespawned()
    {
        _pool = null;
    }
    public void ReturnToPool()
    {
        _pool.Despawn(this);
    }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, Soldier> { }
}
