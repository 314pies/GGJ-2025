using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator animator;
    public PlasmaRifleModel plasmaRifleModel;
    public PlayerIK playerIK;

    public void Initialize(Animator previousAnimator, bool isEquipingRifle)
    {
        if (previousAnimator != null)
        {
            CopyOverPreviousAnimatorState(previousAnimator);
        }

        UpdateStatus(isEquipingRifle);
    }

    public void UpdateStatus(bool isEquipingRifle)
    {
        plasmaRifleModel.gameObject.SetActive(isEquipingRifle);
    }

    public void PlayMuzzleEffect()
    {
        plasmaRifleModel.PlayMuzzleFlash();
    }

    private void CopyOverPreviousAnimatorState(Animator previousAnimator)
    {
        animator.SetBool(Player.isGroundAnimParm,
            previousAnimator.GetBool(Player.isGroundAnimParm));
        animator.SetBool(PlasmaLauncher.IsHoldingRifleAnimParm,
            previousAnimator.GetBool(PlasmaLauncher.IsHoldingRifleAnimParm));
    }

    [Button]
    public void FindAndAutoAssign()
    {
        if (Application.isPlaying)
        {
            Debug.LogError("This is editor only method");
            return;
        }

        animator = GetComponent<Animator>();
        playerIK = GetComponent<PlayerIK>();
        plasmaRifleModel = GetComponentInChildren<PlasmaRifleModel>();

        EditorUtility.SetDirty(this);
    }
}
