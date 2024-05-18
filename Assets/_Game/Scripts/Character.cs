using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected const string RUN = "run";
    protected const string ATTACK = "attack";
    protected const string THROW = "throw";
    protected const string IDLE = "idle";
    protected const string JUMP = "jump";
    protected const string FALL = "fall";
    protected const string DIE = "die";

    [SerializeField] private Animator animator;
    private float hp;
    public bool IsDead => hp <= 0;
    private string currentAnimationName;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
    }
    public virtual void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {
        ChangeAnimation(DIE);
        Invoke(nameof(OnDespawn), 2f);
    }
    public void OnHit(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            if (IsDead)
            {
                OnDeath();
            }
        }
    }
    
    protected void ChangeAnimation(string animationName)
    {
        if (currentAnimationName != animationName)
        {
            animator.ResetTrigger(animationName);
            currentAnimationName = animationName;
            animator.SetTrigger(currentAnimationName);
        }
    }
}
