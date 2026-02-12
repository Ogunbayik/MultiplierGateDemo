using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class EnemyAnimationController : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyVision _vision;

    private Animator _animator;

    [Inject]
    public void Construct(Enemy enemy, EnemyVision vision, Animator animator)
    {
        _enemy = enemy;
        _vision = vision;
        _animator = animator;
    }
    private void OnEnable()
    {
        _enemy.OnEnemyDead += Enemy_OnEnemyDead;
        _vision.OnPlayerDetected += Vision_OnPlayerDetected;
    }
    private void OnDisable()
    {
        _enemy.OnEnemyDead -= Enemy_OnEnemyDead;
        _vision.OnPlayerDetected -= Vision_OnPlayerDetected;
    }
    private void Vision_OnPlayerDetected(Transform target) => PlayRunAnimation();
    private void Enemy_OnEnemyDead() => DieSequence();
    private void PlayRunAnimation() => _animator.SetTrigger(GameConstant.EnemyAnimation.IsRunHash);
    private void PlayDeadAnimation() => _animator.SetTrigger(GameConstant.EnemyAnimation.IsDeadHash);
    private void DieSequence()
    {
        Sequence dieSequence = DOTween.Sequence();
        dieSequence.AppendCallback(() => PlayDeadAnimation());
        dieSequence.AppendInterval(GameConstant.EnemyAnimation.SinkWaitDuration);

        dieSequence.Append(transform.DOMoveY(GameConstant.EnemyAnimation.SinkDepth, GameConstant.EnemyAnimation.SinkMoveDuration))
            .OnComplete(() => _enemy.ReturnToPool());
    }
}
