<<<<<<< HEAD
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    float smoothTime = 0.3f;
    Vector3 offset = new Vector3(0,0,-10);
    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPozition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPozition, ref velocity, smoothTime);
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]private Transform player;
    [SerializeField]private Vector3 offset;
    [SerializeField]private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            Debug.Log("Ya need a player for Camera Follow! Remember that.");
        }

    }
}
>>>>>>> origin/combat-2
