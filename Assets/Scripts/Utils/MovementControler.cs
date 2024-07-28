using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Utils
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class MovementControler : MonoBehaviour, IPlayerController
    {
        [SerializeField] public ScriptableStats stats;
        [SerializeField] Animator animator;
        [SerializeField] float dashForce = 24f;
        [SerializeField] float dashTime = 0.2f;
        [SerializeField] float dashCooldown = 1f;
        public bool isDashingActive = false;
        public Transform WallCheck;
        bool canDash = true;
        bool isDashing = false;
        Rigidbody2D rb;
        CapsuleCollider2D col;
        FrameInput frameInput;
        Vector2 frameVelocity;
        bool cachedQueryStartInColliders;
        bool facingRight = true;
        bool isWallSliding = false;


        #region Interface

        public Vector2 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        public float time { get; set; }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<CapsuleCollider2D>();

            cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        public void PlayerInput()
        {
            frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump"),
                JumpHeld = Input.GetButton("Jump"),
                Dash = Input.GetKey(KeyCode.S),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };


            if (stats.SnapInput)
            {
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.JumpDown)
            {
                jumpToConsume = true;
                timeJumpWasPressed = time;
            }

            if(frameInput.Move.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (frameInput.Move.x < 0 && facingRight)
            {
                Flip();
            }
        }

        public void NPCInput(bool jumpDown, bool jumpHeld, Vector2 move)
        {
            frameInput = new FrameInput
            {
                JumpDown = jumpDown,
                JumpHeld = jumpHeld,
                Move = move
            };

            if (stats.SnapInput)
            {
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.JumpDown)
            {
                jumpToConsume = true;
                timeJumpWasPressed = time;
            }

            if (frameInput.Move.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (frameInput.Move.x < 0 && facingRight)
            {
                Flip();
            }
        }

        #region Collisions

        float frameLeftGrounded = float.MinValue;
        bool grounded;

        public void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, stats.GrounderDistance, ~stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.up, stats.GrounderDistance, ~stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) frameVelocity.y = Mathf.Min(0, frameVelocity.y);

            // Landed on the Ground
            if (!grounded && groundHit)
            {
                grounded = true;
                coyoteUsable = true;
                bufferedJumpUsable = true;
                endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(frameVelocity.y));
            }
            // Left the Ground
            else if (grounded && !groundHit)
            {
                grounded = false;
                frameLeftGrounded = time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        bool jumpToConsume;
        bool bufferedJumpUsable;
        bool endedJumpEarly;
        bool coyoteUsable;
        float timeJumpWasPressed;

        bool HasBufferedJump => bufferedJumpUsable && time < timeJumpWasPressed + stats.JumpBuffer;
        bool CanUseCoyote => coyoteUsable && !grounded && time < frameLeftGrounded + stats.CoyoteTime;

        public void HandleJump()
        {
            if (!endedJumpEarly && !grounded && !frameInput.JumpHeld && !isWallSliding && rb.velocity.y > 0) endedJumpEarly = true;

            if (!jumpToConsume && !HasBufferedJump) return;

            if (grounded || CanUseCoyote || isWallSliding) ExecuteJump();

            jumpToConsume = false;
        }

        void ExecuteJump()
        {
            endedJumpEarly = false;
            timeJumpWasPressed = 0;
            bufferedJumpUsable = false;
            coyoteUsable = false;
            frameVelocity.y = stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        public void HandleDirection()
        {
            if (frameInput.Move.x == 0)
            {
                var deceleration = grounded ? stats.GroundDeceleration : stats.AirDeceleration;
                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, frameInput.Move.x * stats.MaxSpeed, stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        public void HandleGravity()
        {
            if (grounded && frameVelocity.y <= 0f)
            {
                frameVelocity.y = stats.GroundingForce;
            }
            else
            {
                var inAirGravity = stats.FallAcceleration;
                if (endedJumpEarly && frameVelocity.y > 0) inAirGravity *= stats.JumpEndEarlyGravityModifier;
                frameVelocity.y = Mathf.MoveTowards(frameVelocity.y, -stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        public void ApplyMovement()
        {
            rb.velocity = frameVelocity;
        } 

        #if UNITYEDITOR
        void OnValidate()
        {
            if (stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
        #endif

        bool IsWall()
        {
            return Physics2D.OverlapCircle(WallCheck.position, 0.2f, stats.WallLayer);
        }

        public void WallSlide()
        {
            if (IsWall() && !grounded && rb.velocity.y < 0)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -stats.WallSlideSpeed, 1));
            }
            else
            {
                isWallSliding = false;
            }
        }

        void Flip()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            facingRight = !facingRight;
        }

        public void Dashing()
        {
            if (isDashing) { return; }
            if (canDash && isDashingActive && frameInput.Dash)
            {
                Debug.Log("Test");
                StartCoroutine(DoDash());
                frameInput.Dash = false;
            }
        }

        IEnumerator DoDash()
        {
            Debug.Log("Test Do Dash");
            canDash = false;
            isDashing = true;
            float originalGravity = rb.gravityScale;
            Debug.Log("Test gravity scale " + originalGravity);
            frameVelocity = new Vector2(transform.localScale.x * dashForce, 0);
            //trilRenderer.emitting = true;
            yield return new WaitForSeconds(dashTime);
            //trilRenderer.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public bool Dash;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}