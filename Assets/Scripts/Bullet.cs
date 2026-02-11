using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour, IPoolable<int,float,float,Vector3,IMemoryPool>
{
    private IMemoryPool _pool;

    private Rigidbody _rb;

    private int _damage;

    private float _movementSpeed;
    private float _lifeTime;

    public int Damage => _damage;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void OnDespawned() => _pool = null;
    public void OnSpawned(int damage,float lifetime, float speed, Vector3 spawnPos,IMemoryPool pool)
    {
        _damage = damage;
        _lifeTime = lifetime;
        _movementSpeed = speed;
        transform.position = spawnPos;
        _pool = pool;

        _rb.AddForce(Vector3.forward * _movementSpeed, ForceMode.Impulse);
    }
    private void Update()
    {
        //HandleMovement();
        CountdownLifeTime();
    }
    //private void HandleMovement() => transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
    private void CountdownLifeTime()
    {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            ReturnToPool();
    }
    public void ReturnToPool()
    {
        _rb.velocity = Vector3.zero;
        _pool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hitable))
        {
            hitable.TakeDamage(_damage);
            ReturnToPool();
        }
    }
    //Damage - LifeTime - Movement Speed - Spawn Position
    public class Pool : MonoPoolableMemoryPool<int,float,float,Vector3,IMemoryPool, Bullet> { }
}
