using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliso : MonoBehaviour
{
    [SerializeField] private string weaponTag = "Weapon";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);
        if (collision.collider.CompareTag(weaponTag))
        {
            Debug.Log("Weapon Collision Detected");
            Destroy(gameObject);
        }
    }
}

