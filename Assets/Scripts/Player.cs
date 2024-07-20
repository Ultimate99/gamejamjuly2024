using UnityEngine;
using Utils;

public class Player : MonoBehaviour
{
    [SerializeField] MovementControler movementControler;

    float moveX;
    bool grounded = true;

    void Awake()
    {
        movementControler.facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementControler.jump = true;
        }
    }

    void FixedUpdate()
    {
        movementControler.Move(moveX, grounded);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
