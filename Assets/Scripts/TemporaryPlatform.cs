using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TemporaryPlatform : MonoBehaviour
{
    [SerializeField] float disapearAfter = 0.5f;
    [SerializeField] float apearAfter = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            StartCoroutine(platformMechanics());
        }
    }

    IEnumerator platformMechanics()
    {
        yield return new WaitForSeconds(disapearAfter);
        this.GetComponent<BoxCollider2D>().enabled = false;
        Color color = this.GetComponentInChildren<Tilemap>().color;
        this.GetComponentInChildren<Tilemap>().color = new Color(color.r, color.g, color.b, 0.5f);
        yield return new WaitForSeconds(apearAfter);
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponentInChildren<Tilemap>().color = new Color(color.r, color.g, color.b, 1f);
    }
}
