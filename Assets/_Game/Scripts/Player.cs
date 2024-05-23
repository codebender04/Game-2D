using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float distanceBetweenImages;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float glidingSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isDead = false;
    private bool isGliding = false;

    private float dashTimeLeft;
    private float lastImageXPos;
    private float lastDash = -100;
    private float horizontal;
    private int coin = 0;   
    private Vector3 savePoint;


    private void Awake()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
    }
    public override void OnInit()
    {
        base.OnInit();
        isAttacking = false;
        transform.position = savePoint;
        ChangeAnimation(IDLE);
        DeactivateAttack();
        SavePoint();
        UIManager.instance.SetCoin(coin);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    private bool IsGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    void Update()
    {
        if (IsDead) return;
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
            if (Input.GetKey(KeyCode.Space))
            {
                Glide();
            }
            else
            {
                ChangeAnimation(FALL);
            }
            isJumping = false;
        }
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.deltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 0 : 180, 0);
        }
        else if (isGrounded)
        {
            ChangeAnimation(IDLE); 
            rb.velocity = Vector2.zero;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            AttemptToDash();
        }
        CheckDash();
    }
    private void AttemptToDash()
    {
        if ((Time.time - lastDash) > dashCooldown)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            PlayerAfterImagePool.Instance.GetFromPool().GetComponent<PlayerAfterImage>().OnInit(transform.position, transform.rotation, spriteRenderer.sprite);
            lastImageXPos = transform.position.x;

        }
    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * horizontal, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;
                if (MathF.Abs(transform.position.x - lastImageXPos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool().GetComponent<PlayerAfterImage>().OnInit(transform.position, transform.rotation, spriteRenderer.sprite); 
                    lastImageXPos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }
    private void Glide()
    {
        ChangeAnimation(GLIDE);
        isGliding = true;
        rb.velocity = new Vector2(rb.velocity.x, glidingSpeed);
    }
    public void Attack()
    {
        ChangeAnimation(ATTACK);
        isAttacking = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActivateAttack();
        Invoke(nameof(DeactivateAttack), 0.5f);
    }
    public void Throw()
    {
        ChangeAnimation(THROW);
        isAttacking = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Instantiate(kunaiPrefab.gameObject, throwPoint.position, throwPoint.rotation);
    }
    private void ResetAttack()
    {
        ChangeAnimation(IDLE);
        isAttacking = false;
    }
    public void Jump()
    {
        if (isGrounded)
        {
            ChangeAnimation(JUMP);
            isJumping = true;
            rb.AddForce(jumpForce * Vector2.up);
        }
    }
    internal void SavePoint()
    {
        savePoint = transform.position; 
    }
    private void ActivateAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeactivateAttack()
    {
        attackArea.SetActive(false);
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            coin++;
            UIManager.instance.SetCoin(coin);
            PlayerPrefs.SetInt("Coin", coin);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("DeathZone"))
        {
            ChangeAnimation(DIE);
            Invoke(nameof(OnInit), 1f);
        }
    }

}
