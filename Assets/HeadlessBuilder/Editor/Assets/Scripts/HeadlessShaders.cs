/* 
 * Headless Builder
 * (c) Salty Devs, 2022
 * 
 * Please do not publish or pirate this code.
 * We worked really hard to make it.
 * 
 */


#if UNITY_2018_2_OR_NEWER

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

class HeadlessShader : IPreprocessShaders
{

    public int callbackOrder { get { return 1024; } }

    public void OnProcessShader(
        Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
    {
        if (!Headless.IsBuildingHeadless() || !HeadlessBuilder.ShouldStripShaders())
            return;

        int shaderCount = shaderCompilerData.Count;

        for (int i = 0; i < shaderCompilerData.Count; ++i)
        {
            shaderCompilerData.RemoveAt(i);
            --i;
        }

        HeadlessBuilder.shaderCount += shaderCount;
    }
}

#endif