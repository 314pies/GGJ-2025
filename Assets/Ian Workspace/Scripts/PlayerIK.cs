using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public Animator animator;
    public Vector3 lookAt;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK()
    {
        Debug.Log("callled IK: " + lookAt);

        animator.SetLookAtWeight(0.5f, 1, 0.5f);
        animator.SetLookAtPosition(lookAt);
    }
}
