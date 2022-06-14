
using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _anim;


    private readonly int _velocityXHash = Animator.StringToHash("VelocityX");
    private readonly int _velocityZHash = Animator.StringToHash("VelocityZ");
    private readonly int _isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int _isCrouchingHash = Animator.StringToHash("IsCrouching");
    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int _jumpTriggerHash = Animator.StringToHash("Jump");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _getHitTriggerHash = Animator.StringToHash("GetHit");



    private readonly int _isFallingHash = Animator.StringToHash("IsFalling");
    private readonly int _isSwimmingHash = Animator.StringToHash("IsSwimming");


    private Action _onAttackEnd;

    public void SetLayer(int layer,bool isActive)
    {
        _anim.SetLayerWeight(layer, isActive ? 1 : 0);
    }
    public void SetVelocities(float x, float z)
    {
        _anim.SetFloat(_velocityXHash, x, 0.1f, Time.deltaTime * 0.7f);
        _anim.SetFloat(_velocityZHash, z, 0.1f, Time.deltaTime * 0.7f);
    }
    public void SetIsRunning(bool isRunning)
    {
        _anim.SetBool(_isRunningHash, isRunning);
    }
    public void SetIsGrounded(bool isGrounded)
    {
        _anim.SetBool(_isGroundedHash, isGrounded);
    }
    public void SetIsCrouching(bool isCrouching)
    {
        _anim.SetBool(_isCrouchingHash, isCrouching);
    }
    public void TriggerJump()
    {
        _anim.SetTrigger(_jumpTriggerHash);
    }
    public void SetIsFalling(bool isFalling)
    {
        _anim.SetBool(_isFallingHash, isFalling);
    }
    public void SetIsSwimming(bool isSwimming)
    {
        _anim.SetBool(_isSwimmingHash, isSwimming);
    }

    public void TriggerAttack(Action onAttackEnd = null)
    {
        _anim.SetTrigger(_attackTriggerHash);
        _onAttackEnd = onAttackEnd;
    }
    public void AttackEnd()
    {
        _onAttackEnd?.Invoke();
    }
    public void TriggerGetHit()
    {
        _anim.SetTrigger(_getHitTriggerHash);
    }

}
