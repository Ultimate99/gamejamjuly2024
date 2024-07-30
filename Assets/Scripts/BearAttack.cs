using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttack : MonoBehaviour
{

    [SerializeField] private Animator animator;
    private bool isAttacking = false;
    Player player;
    EnemyPathing enemyPathing;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Weapon"))
        {

            StartCoroutine(BearAttackStart());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Weapon"))
        {
            StartCoroutine(BearAttackEnd());
        }
    }     


    private IEnumerator BearAttackStart()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", isAttacking);
        Debug.Log("BEAR ATTACK: " + isAttacking);
        //Tikky's Mom
        yield return new WaitForSecondsRealtime(1);
    }
    private IEnumerator BearAttackEnd()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        Debug.Log("BEAR ATTACK: " + isAttacking);
        yield return new WaitForSecondsRealtime(1);
    }
}
