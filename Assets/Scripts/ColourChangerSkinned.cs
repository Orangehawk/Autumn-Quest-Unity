using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChangerSkinned : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer meshRenderer;

    public void ChangeColor(float r, float g, float b)
	{
        meshRenderer.material.color = new Color(r / 255f, g / 255f, b / 255f);
    }

    public void ChangeColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void ChangeColor(System.Drawing.Color color)
    {
        meshRenderer.material.color = new Color(color.R / 255f, color.G / 255f, color.B / 255f);
    }
}
