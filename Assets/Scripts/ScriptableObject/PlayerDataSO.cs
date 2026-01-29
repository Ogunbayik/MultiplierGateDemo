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


    public float MovementSpeed => _movementSpeed;
    public float MinXConstraint => _minXConstraint;
    public float MaxXConstraint => _maxXConstraint;
    public float MinZConstraint => _minZConstraint;
    public float MaxZConstraint => _maxZConstraint;

}