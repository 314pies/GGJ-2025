/* 
 * Headless Builder
 * (c) Salty Devs, 2022
 * 
 * Please do not publish or pirate this code.
 * We worked really hard to make it.
 * 
 */

using UnityEngine;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.Rendering;
#endif

// This class affects the game's behaviour during runtime.
public class HeadlessBehaviour : MonoBehaviour
{

    // Callback to be called before any culling
    public void NullifyCamera(Camera camera)
    {
        camera.enabled = false;
    }

#if UNITY_2019_1_OR_NEWER
    public void NullifyCamera(ScriptableRenderContext context, Camera camera)
    {
        camera.enabled = false;
    }
#endif

}
