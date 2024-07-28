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
