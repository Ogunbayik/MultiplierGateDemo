using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class Soldier : MonoBehaviour, IPoolable<IMemoryPool>
{
    private SoldierAnimatorController _animator;

    private IMemoryPool _pool;
   
    private Bullet.Pool _bulletPool;

    private SignalBus _signalbus;

    private PlayerController _playerController;
    public SoldierAnimatorController AnimatorController => _animator;

    [Inject]
    public void Construct(Bullet.Pool bulletPool,SignalBus signalBus, PlayerController playerController)
    {
        _bulletPool = bulletPool;
        _signalbus = signalBus;
        _playerController = playerController;
    }
    private void Awake() => _animator = GetComponent<SoldierAnimatorController>();
    private void OnEnable() => _playerController.OnPlayerAttacked += PlayerController_OnPlayerAttacked;
    private void OnDisable() => _playerController.OnPlayerAttacked -= PlayerController_OnPlayerAttacked;
    private void PlayerController_OnPlayerAttacked() => HandleFireSequence().Forget();
    private async UniTaskVoid HandleFireSequence()
    {
        var token = this.GetCancellationTokenOnDestroy();

        for (int i = 0; i < _playerController.Data.BulletCount; i++)
        {
            //StartAttacking
            _playerController.SetAttackStatus(false);

            _animator.PlayAttackAnimation();
            await UniTask.Delay(TimeSpan.FromSeconds(GameConstant.SoldierAnimation.AttackWaitDuration), cancellationToken: token);

            var bullet = _bulletPool.Spawn(
                _playerController.Data.BulletDamage,
                _playerController.Data.BulletLifeTime, 
                _playerController.Data.BulletSpeed,
                transform.position + _playerController.Data.AttackOffset,
                _bulletPool);

            bullet.transform.SetParent(null);
            _playerController.SetReloadStatus(true);
        }
        //Reloading
        _animator.PlayReloadAnimation();

        await UniTask.Delay(TimeSpan.FromSeconds(GameConstant.SoldierAnimation.ReloadWaitDuration), cancellationToken : token);

        _playerController.SetReloadStatus(false);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Die();
            HandleDeadSequence().Forget();
        }
    }
    private async UniTaskVoid HandleDeadSequence()
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();

        _animator.PlayDeadAnimation();
        transform.SetParent(null);
        //TODO Ölenler Gri olabilir.
        _signalbus.Fire(new GameSignal.SoldierDeadSignal(this));
        await UniTask.Delay(TimeSpan.FromSeconds(GameConstant.SoldierAnimation.DieWaitDuration), cancellationToken: token);
        //TODO Çürüme efekti eklenebilir.
        ReturnToPool();
    }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, Soldier> { }
}
