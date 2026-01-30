using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "ScriptableObject/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Data Settings")]
    [SerializeField] private float _movementSpeed;
    [Header("Movement Border")]
    [SerializeField] private float _minXConstraint;
    [SerializeField] private float _maxXConstraint;
    [SerializeField] private float _minZConstraint;
    [SerializeField] private float _maxZConstraint;
    [Header("Attack Settings")]
    [SerializeField] private Vector3 _attackOffset;
    [SerializeField] private int _bulletCount;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _reloadTime;


    public float MovementSpeed => _movementSpeed;
    public float MinXConstraint => _minXConstraint;
    public float MaxXConstraint => _maxXConstraint;
    public float MinZConstraint => _minZConstraint;
    public float MaxZConstraint => _maxZConstraint;
    public Vector3 AttackOffset => _attackOffset;
    public int BulletCount => _bulletCount;
    public int BulletDamage => _bulletDamage;
    public float BulletSpeed => _bulletSpeed;
    public float BulletLifeTime => _bulletLifeTime;
    public float FireRate => _fireRate;
    public float ReloadTime => _reloadTime;

}