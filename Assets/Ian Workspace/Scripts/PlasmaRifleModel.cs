using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaRifleModel : MonoBehaviour
{
    public ParticleSystem muzzle;
    public void PlayMuzzleFlash()
    {
        muzzle.Play(true);
    }
}
