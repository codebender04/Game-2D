using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class Player : MonoBehaviour
{
    private const string RUN = "run";
    private const string ATTACK = "attack";
    private const string THROW = "throw";
    private const string IDLE = "idle";
    private const string JUMP = "jump";
    private const string FALL = "fall";
    private const string DIE = "die";

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDead = false;

    private float horizontal;
    private int coin = 0;   
    private string currentAnimationName;
    private Vector3 savePoint;
    private void Start()
    {
        SavePoint();
        OnInit();
    }
    private void OnInit()
    {
        isDead = false;
        isAttacking = false;
        transform.position = savePoint;
        ChangeAnimation(IDLE);
    }
    private bool IsGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    void FixedUpdate()
    {
        if (isDead) return;
        isGrounded = IsGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (isAttacking)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnimation(RUN);
            }
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnimation(FALL);
            isJumping = false;
        }
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded)
        {
            ChangeAnimation(IDLE); 
            rb.velocity = Vector2.zero;
        }
    }
    private void Attack()
    {
        ChangeAnimation(ATTACK);
        isAttacking = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }
    private void Throw()
    {
        ChangeAnimation(THROW);
        isAttacking = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }
    private void ResetAttack()
    {
        ChangeAnimation(IDLE);
        isAttacking = false;
    }
    private void Jump()
    {
        ChangeAnimation(JUMP);
        isJumping = true;
        rb.AddForce(jumpForce * Vector2.up);
    }
    private void ChangeAnimation(string animationName)
    {
        if (currentAnimationName != animationName)
        {
            animator.ResetTrigger(animationName);
            currentAnimationName = animationName;
            animator.SetTrigger(currentAnimationName);
        }
    }
    internal void SavePoint()
    {
        savePoint = transform.position; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coin++;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("DeathZone"))
        {
            isDead = true;
            ChangeAnimation(DIE);
            Invoke(nameof(OnInit), 1f);
        }
    }

}