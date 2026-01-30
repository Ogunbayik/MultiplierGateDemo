using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IPoolable<int,float,float,Vector3,IMemoryPool>
{
    private IMemoryPool _pool;

    private float _movementSpeed;
    private int _damage;
    private float _lifeTime;

    public int Damage => _damage;
    public void OnDespawned() => _pool = null;
    public void OnSpawned(int damage,float lifetime, float speed, Vector3 spawnPos,IMemoryPool pool)
    {
        _damage = damage;
        _lifeTime = lifetime;
        _movementSpeed = speed;
        transform.position = spawnPos;
        _pool = pool;
    }
    private void Update()
    {
        HandleMovement();
        CountdownLifeTime();
    }
    private void HandleMovement() => transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
    private void CountdownLifeTime()
    {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            ReturnToPool();
    }
    public void ReturnToPool() => _pool.Despawn(this);
   
    //Damage - LifeTime - Movement Speed - Spawn Position
    public class Pool : MonoPoolableMemoryPool<int,float,float,Vector3,IMemoryPool, Bullet> { }
}
