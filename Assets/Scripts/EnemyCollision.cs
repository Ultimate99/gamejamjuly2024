using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliso : MonoBehaviour
{
    public string weaponTag = "Weapon";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);

        // Check if the specific collider in the collision is tagged as "Weapon"
        if (collision.collider.CompareTag(weaponTag))
        {
            Debug.Log("Weapon Collision Detected");
            Destroy(gameObject);
        }
    }
}

