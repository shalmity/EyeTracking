using System.IO;
using UnityEngine;

public class RWManager : MonoBehaviour
{
    TextureHeatmap Heatmap;
    public Material[] materials;
    void Start()
    {
        Heatmap = GameObject.Find("TestBed").GetComponent<TextureHeatmap>();
        Heatmap.mMaterial = materials[0];
    }

    void Update()
    {
        
    }
}
