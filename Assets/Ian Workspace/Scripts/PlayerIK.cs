using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public Animator animator;
    public Vector3 lookAt;
    public float weight = 1.0f, body = 1.0f, head = 1.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK()
    {
        animator.SetLookAtWeight(weight, body, head);
        animator.SetLookAtPosition(lookAt);
    }
}
