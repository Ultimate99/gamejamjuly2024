using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDestroyer : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string enemyTag = "Enemy";


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(playerTag) || collision.gameObject.CompareTag(enemyTag))
        {
            Destroy(collision.gameObject);
        }
    }
}
