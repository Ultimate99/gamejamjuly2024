using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateWeaponF();
        }
        else if (Input.GetKeyUp(KeyCode.E))
            RotateWeaponB();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetObject.transform.localScale = new Vector3(1f, 4f, 1f);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
            targetObject.transform.localScale = new Vector3(1f, 1f, 1f);

    }

    void RotateWeaponF()
    {
        if (targetObject != null)
        {
            float roatationAmount = -86;
            targetObject.transform.Rotate(new Vector3(0, 0, roatationAmount));
        }
        else
            Debug.Log("Attach Target Object bro.");
    }
    void RotateWeaponB()
    {
        if (targetObject != null)
        {
            float roatationAmount = 86;
            targetObject.transform.Rotate(new Vector3(0, 0, roatationAmount));
        }
        else
            Debug.Log("Attach Target Object bro.");
    }
}
