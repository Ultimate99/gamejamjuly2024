using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float AttackDistanceX = 1.2f;
    [SerializeField] private float AttackDistanceY = 0f;
    [SerializeField] private float AttackRotationZ = 0f;

    private Vector3 originalPosition;

    private void Start()
    {
        if (targetObject != null)
            originalPosition = targetObject.transform.localPosition;
        else
            Debug.LogError("Attach Target Object bro.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            AttackF();
        else if (Input.GetKeyUp(KeyCode.E))
            AttackB();
    }

    void AttackF()
    {
        if (targetObject != null)
        {
            targetObject.transform.localPosition = originalPosition + new Vector3(AttackDistanceX, AttackDistanceY, 0f);
            targetObject.transform.Rotate(new Vector3(0, 0, AttackRotationZ));
        }
        else
            Debug.Log("Attach Target Object bro.");
    }    
    void AttackB()
    {
        if (targetObject != null)
        {
            targetObject.transform.localPosition  = originalPosition;
            targetObject.transform.Rotate(new Vector3(0, 0, -AttackRotationZ));
        }
        else
            Debug.Log("Attach Target Object bro.");
    }
}
