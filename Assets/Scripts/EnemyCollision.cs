using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyColliso : MonoBehaviour
{
    [SerializeField] private string weaponTag = "Weapon";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int HP = 3;
    [SerializeField] private float KB_Force = 30f;
    [SerializeField] private float KB_Duration = 0.2f;

    private Rigidbody2D rb;
    private Vector2 KB_Direction;
    private bool applyKB;
    private EnemyPathing enemyPath;
    private Combat combat;

    public Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        enemyPath = GetComponent<EnemyPathing>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);
        Debug.Log("TAG Detected with: " + collision.gameObject.tag);
        if (collision.collider.CompareTag(weaponTag))
        {
            if (HP > 0)
            {
                HP--;
                KB_Direction = (transform.position - collision.transform.position).normalized;
                applyKB = true;
                enemyPath.SetKnockedBack(true);
                StartCoroutine(EndKnockback());
                Debug.Log("Knockback direction: " + KB_Direction);
            }
            if(HP <= 0)
                Destroy(gameObject);
        }
        else if (collision.collider.CompareTag(playerTag) && combat.isAttacking)
        {
            Debug.Log("BANG");
            KB_Direction = (transform.position - collision.transform.position).normalized;
            applyKB = true;
            enemyPath.SetKnockedBack(true);
            StartCoroutine(EndKnockback());
            Debug.Log("Knockback direction: " + KB_Direction);
        }
    }

    private void FixedUpdate()
    {
        if(applyKB)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(KB_Direction * KB_Force, ForceMode2D.Impulse);
            Debug.Log("Knockback force applied: " + KB_Direction * KB_Force);
            applyKB = false;
        }
    }

    private IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(KB_Duration);
        enemyPath.SetKnockedBack(false);
    }
}

