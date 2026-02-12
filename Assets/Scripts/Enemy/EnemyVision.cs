using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public event Action<Transform> OnPlayerDetected;

    [Header("Vision Settings")]
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _checkLayer;

    private Transform _target;

    private bool _isDetected;

    private Collider[] _results = new Collider[1];
    void Update()
    {
        if (_isDetected)
            return;

        HandleDetect();
    }
    private void HandleDetect()
    {
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, _checkRadius, _results, _checkLayer);
        if (hitCount > 0)
        {
            _target = _results[0].transform;
            _isDetected = true;
            OnPlayerDetected?.Invoke(_target);
            this.enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
