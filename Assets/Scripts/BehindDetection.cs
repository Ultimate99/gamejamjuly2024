using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindDetection : MonoBehaviour
{
    public float speed = 5f;
    public float stoppingDistance = 0.5f; // Distance at which the bear stops following
    private bool isFollowing = false; // Whether the bear is currently following the player
    private Transform target; // The player
    private EnemyPathing enemyPathing; // Reference to the EnemyPathing script

    private void Start()
    {
        enemyPathing = GetComponentInParent<EnemyPathing>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform; // Set the player as the target
            isFollowing = true; // Start following
            enemyPathing.SetFollowing(true, target); // Notify the EnemyPathing script to start following
            Debug.Log("Player entered detection range.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFollowing = false; // Stop following when the player exits the trigger
            enemyPathing.SetFollowing(false, null); // Notify the EnemyPathing script to stop following
            Debug.Log("Player exited detection range.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (target != null && isFollowing)
            {
                enemyPathing.SetFollowing(true, target); // Ensure the EnemyPathing script is still following
            }
        }
    }
}
