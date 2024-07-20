using UnityEngine;

namespace Utils
{
    public class MovementControler : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [Range(1f, 100f)] [SerializeField] float speed = 20f;
        [Range(0, .3f)][SerializeField] float movementSmoothing = .05f;
        [SerializeField] float jumpHeight = 4f;
        [SerializeField] float gravityScale = 10f;
        [SerializeField] float fallGravityScale = 50f;
        [SerializeField] float wallSlideSpeed = 2f;
        [SerializeField] Transform wallCheck;
        [SerializeField] LayerMask wallLayer;

        public  bool facingRight { get; set; }
        public bool jump { get; set; }

        private Vector3 v = Vector3.zero;
        bool grounded = true;
        bool isWallSliding = false;

        float wallJumpingDirection;

        public void Jump()
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));

            if (isWallSliding)
            {
                wallJumpingDirection = -transform.localScale.x;
            }

            if (jump && !isWallSliding)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = false;
            }
            if (jump && isWallSliding)
            {
                jump = false;
                rb.AddForce(new Vector2(wallJumpingDirection * (jumpForce / 2), (jumpForce / 2)), ForceMode2D.Impulse);

                if (transform.localScale.x != wallJumpingDirection)
                {
                    Flip();
                }
            }

            if (rb.velocity.y >= 0)
            {
                rb.gravityScale = gravityScale;
            }
            else
            {
                rb.gravityScale = fallGravityScale;
            }
        }

        public void Move(float moveX, bool grounded)
        {
            this.grounded = grounded;

            Vector3 targetVelocity = new Vector2(moveX * speed, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref v, movementSmoothing);

            if (moveX > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveX < 0 && facingRight)
            {
                Flip();
            }
            
            Jump();

            WallSlide();
        }

        public void Flip()
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            facingRight = !facingRight;
        }

        private bool IsWall()
        {
            return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        }

        private bool WallSlide()
        {
            if (IsWall() && !grounded && rb.velocity.y < 0)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = false;
            }

            return isWallSliding;
        }
    }
}

