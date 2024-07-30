using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDaddyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool inRange = false;
    Player player;

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
        inRange = true;
        animator.SetBool("inRange", inRange);
        Debug.Log("WOLF ATTACK: " + inRange);
        //Tikky's Mom
        yield return new WaitForSecondsRealtime(1);
    }
    private IEnumerator BearAttackEnd()
    {
        inRange = false;
        animator.SetBool("inRange", inRange);
        Debug.Log("WOLF ATTACK: " + inRange);
        yield return new WaitForSecondsRealtime(1);
    }
}
