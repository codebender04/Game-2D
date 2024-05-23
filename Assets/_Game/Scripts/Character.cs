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
    protected const string GLIDE = "glide";
    protected const string FALL = "fall";
    protected const string DIE = "die";

    [SerializeField] private Animator animator;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;
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
        healthBar.OnInit(100, transform);
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
                hp = 0; 
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
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
