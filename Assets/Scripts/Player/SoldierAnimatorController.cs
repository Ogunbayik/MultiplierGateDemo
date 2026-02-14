using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private void Awake() => _animator = GetComponent<Animator>();
    public void PlayMoveAnimation(float speed) => _animator.SetFloat(GameConstant.SoldierAnimation.MOVEMENT_KEY, speed);
    public void PlayBlendMoveAnimation(float horizontal, float vertical)
    {
        _animator.SetFloat(GameConstant.SoldierAnimation.BLEND_MOVE_X, horizontal);
        _animator.SetFloat(GameConstant.SoldierAnimation.BLEND_MOVE_Y, vertical);
    }
    public void PlayDeadAnimation() => _animator.SetTrigger(GameConstant.SoldierAnimation.DIE_KEY);
    public void PlayAttackAnimation() => _animator.SetTrigger(GameConstant.SoldierAnimation.FIRE_KEY);
    public void PlayReloadAnimation() => _animator.SetTrigger(GameConstant.SoldierAnimation.RELOAD_KEY);
}
