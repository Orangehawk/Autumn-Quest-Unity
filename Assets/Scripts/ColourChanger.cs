using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
