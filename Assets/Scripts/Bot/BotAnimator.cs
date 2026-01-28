using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BotAnimator : MonoBehaviour
{
    private readonly int IsWalking = Animator.StringToHash("IsWalking");
    private readonly int HasBarrel = Animator.StringToHash("HasBarrel");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetupStaticIdle()
    {
        _animator.SetBool(IsWalking, false);
        _animator.SetBool(HasBarrel, false);
    }

    public void SetupWalk()
    {
        _animator.SetBool(IsWalking, true);
        _animator.SetBool(HasBarrel, false);
    }

    public void SetupWalkWithBarrel()
    {
        _animator.SetBool(IsWalking, true);
        _animator.SetBool(HasBarrel, true);
    }
}