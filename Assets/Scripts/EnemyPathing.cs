using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    public float stoppingDistance = 0.5f;

    private Rigidbody2D rb;
    private Transform currentPoint;
    private bool isGrounded = true;
    private bool isKB = false;
    private bool facingRight = true;

    Animator animator;

    private bool isFollowing = false;
    private Transform target; // The player


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    void Update()
    {
        if (!isKB)
        {
            if (isFollowing)
            {
                FollowPlayer();
            }
            else
            {
                MovePoint();
            }
        }
    }

    void MovePoint()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(-speed, rb.velocity.y);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
            if (facingRight) // Ensure the bear is facing the right direction when patrolling
                Flip();
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            if (!facingRight) // Ensure the bear is facing the right direction when patrolling
                Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Not Grounded");
            isGrounded = false;
        }
    }

    public void SetKnockedBack(bool knockedBack)
    {
        isKB = knockedBack;
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void FollowPlayer()
    {
        if (target == null)
        {
            isFollowing = false;
            return;
        }

        float distance = Vector2.Distance(rb.position, target.position);
        if (distance > stoppingDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            // Flip the bear to face the player and set the velocity
            if (direction.x > 0 && !facingRight)
            {
                Flip();
                rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
            }
            else if (direction.x < 0 && facingRight)
            {
                Flip();
                rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
            }

            Debug.Log("Following player. Distance: " + distance);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving when within stopping distance
            Debug.Log("Within stopping distance. Stopping.");
        }
    }

    public void SetFollowing(bool following, Transform followTarget)
    {
        isFollowing = following;
        target = followTarget;

        if (!following)
        {
            // Ensure the bear flips back to the correct patrol direction when stopping following
            Vector2 directionToPoint = (currentPoint.position - transform.position).normalized;
            if (directionToPoint.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (directionToPoint.x < 0 && facingRight)
            {
                Flip();
            }
        }
    }
}
