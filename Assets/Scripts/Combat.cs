using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float AttackDistanceX = 1.2f;
    [SerializeField] private float AttackDistanceY = 0f;
    [SerializeField] private float AttackRotationZ = 0f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject weaponHitBox;

    private Vector3 originalPosition;
    public bool isAttacking = false;
    public bool empAtttack = false;
    

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
        {
            isAttacking = true;
            weaponHitBox.SetActive(true);
            animator.SetBool("Attack", isAttacking);
            AttackF();
        } 
        else if (Input.GetKeyUp(KeyCode.E))
        {
            isAttacking = false;
            weaponHitBox.SetActive(false);
            animator.SetBool("Attack", isAttacking);
            AttackB();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            empAtttack = true;
            weaponHitBox.SetActive(true);
            animator.SetBool("Attack_Emp", empAtttack);
            AttackF();
        }        
        else if (Input.GetKeyUp(KeyCode.F))
        {
            empAtttack = false;
            weaponHitBox.SetActive(false);
            animator.SetBool("Attack_Emp", empAtttack);
            AttackB();
        }
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
