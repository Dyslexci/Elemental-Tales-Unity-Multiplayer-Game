using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *
 *    Blurs the scene behind the UI when enabled for certain panels.
 */

public class BlurRenderer : MonoBehaviour
{
    [SerializeField] private Camera blurCamera;
    [SerializeField] private Material blurMaterial;

    /// <summary>
    /// Sets the texture of the panel behind the UI to be a blurry version of the camera output.
    /// </summary>
    void Start()
    {
        if (blurCamera.targetTexture != null)
            blurCamera.targetTexture.Release();

        blurCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
        blurMaterial.SetTexture("_RenTex", blurCamera.targetTexture);
    }
}