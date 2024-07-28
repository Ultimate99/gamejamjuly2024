<<<<<<< HEAD
using System;
using UnityEngine;
using Utils;
using Utils.PowerupSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private MovementControler movementControler;
    [SerializeField] int CurrentHealth = 6;
    [SerializeField] int MaxiumumHealth = 6;
    [SerializeField] Dash dashing;

    public event EventHandler<HealedEventArgs> Healed;
    public event EventHandler<DamagedEventArgs> Damaged;

    private void Update()
    {
        movementControler.time += Time.deltaTime;
        movementControler.PlayerInput();
    }

    private void FixedUpdate()
    {
        movementControler.CheckCollisions();
        
        movementControler.HandleJump();
        movementControler.HandleDirection();
        movementControler.HandleGravity();
        movementControler.WallSlide();
        movementControler.Dashing();

        movementControler.ApplyMovement();
    }

    public void Heal(int amount)
    {
        var newHealth = Math.Min(CurrentHealth + amount, MaxiumumHealth);
        if (Healed != null)
            Healed(this, new HealedEventArgs(newHealth - CurrentHealth));
        CurrentHealth = newHealth;
    }

    public void Damage(int amount)
    {
        var newHealth = Math.Max(CurrentHealth - amount, 0);
        if (Damaged != null)
            Damaged(this, new DamagedEventArgs(CurrentHealth - newHealth));
        CurrentHealth = newHealth;
    }

    public class HealedEventArgs : EventArgs
    {
        public HealedEventArgs(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; private set; }
    }

    public class DamagedEventArgs : EventArgs
    {
        public DamagedEventArgs(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; private set; }
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;

    private float moveX;
    private Vector3 v = Vector3.zero;

    public Animator animator;


    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityScale = 10f;
    [SerializeField] private float fallGravityScale = 50f;
    private bool grounded = true;
    [SerializeField] private Transform groundCheck;
    private bool jump = false;
    public bool facingRight = true;

    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;

    private bool isWallSliding = false;

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || isWallSliding))
        {
            jump = true;
        }

        if(moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }

        // Ground check
        grounded = groundCheck.GetComponent<Collider2D>().IsTouchingLayers(groundLayer);
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(moveX * speed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref v, movementSmoothing);
        float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));

        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else
        {
            rb.gravityScale = fallGravityScale;
        }

        WallSlide();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            grounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            grounded = false;
        }
    }

    private bool IsWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if(IsWall() && !grounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

  /*  void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheck.GetComponent<BoxCollider2D>());
        }
    }*/

}
>>>>>>> origin/combat-2
