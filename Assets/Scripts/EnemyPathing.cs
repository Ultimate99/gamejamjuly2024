using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float minJumpInterval = 2f;
    [SerializeField] private float maxJumpInterval = 5f;
    private bool isGrounded = true;
    private bool isKB = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
        StartCoroutine(RandomJump());
    }

    // Update is called once per frame


    void Update()
    {
        if(!isKB)
            MovePoint();
    }

   void MovePoint()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
            rb.velocity = new Vector2(speed, 0);
        else
            rb.velocity = new Vector2(-speed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            currentPoint = pointA.transform;

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            currentPoint = pointB.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
    }

    private IEnumerator RandomJump()
    {
        
        while (isGrounded)
        {
            yield return new WaitForSeconds(Random.Range(minJumpInterval, maxJumpInterval));
            Jump();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            Debug.Log("Jumping");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
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

}
