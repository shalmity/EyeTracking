using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureHeatmap : MonoBehaviour
{
    CoordManager coord;

    // About Shader Interaction

    int num = 200;
    public Material mMaterial;
    MeshRenderer mMeshRenderer;

    float[] mPoints;
    int mHitCount;

    private GameObject quad;
    private Texture textures;
    private Renderer quadRenderer;
    private Texture MainTextures;

    // Shader Interaction Variable End

    public float Texture_X, Texture_Y;
    void Awake()
    {
        coord = GameObject.Find("CoordManager").GetComponent<CoordManager>();
    }

    void Start()
    {
        mMeshRenderer = GetComponent<MeshRenderer>();
        mMaterial = mMeshRenderer.material;
        MainTextures = mMaterial.mainTexture;
        mPoints = new float[num * 3];
    }

    void Update()
    {
        if (coord.IsLoadAbled == false)
        {
            Texture_X = (coord.Coord_X*4-2);
            Texture_Y = -(coord.Coord_Y*4-2);

            //CoordManager의 좌표값을 텍스쳐에 맞춰 전환한 값 출력
            //Debug.Log("Texture Coord : Ray_X : " + Texture_X + ", Ray_Y : " + Texture_Y);
            
            if(coord.IsTransfferAbled == true)
            {
                addHitPoint(Texture_X,Texture_Y);
            }
            SwitchDistance();
            SwitchScale();
        }
        else
        {
            for (int i = 0; i < coord.coord_x.Count; i++) 
            {
                Texture_X = (coord.coord_x[i]*4-2);
                Texture_Y = -(coord.coord_y[i]*4-2);

                addHitPoint(Texture_X,Texture_Y);
                SwitchDistance();
                SwitchScale();
            }
            coord.IsLoadAbled = false;
        }
    }

    public void addHitPoint(float x, float y)
    {
        if (MainTextures != coord.quadRenderer.material.mainTexture)
        {
            mHitCount = 0;
            MainTextures = coord.quadRenderer.material.mainTexture;
        }

            mPoints[mHitCount * 3] = x;
            mPoints[mHitCount * 3 + 1] = y;
            mPoints[mHitCount * 3 + 2] = Random.Range(1f, 1.5f);

            mHitCount++;
            mHitCount %= num;

            mMaterial.SetFloatArray("_Hits", mPoints);
            mMaterial.SetInt("_HitCount", mHitCount);
    
    }
    private void SwitchDistance()
    {
        //Debug.Log("Current distance is " + coord.distance);
        if(coord.distance == CoordManager.Distance.close)
        {
            //Debug.Log("Close Activated");
            transform.position = new Vector3(0,0,2);
        }
        if(coord.distance == CoordManager.Distance.far)
        {
            //Debug.Log("Far Activated");
            transform.position = new Vector3(0,0,4);
        }
    }

    private void SwitchScale()
    {
        //Debug.Log("Current Scale is " + coord.xyscale + "*" + coord.xyscale);
        if(coord.xyscale == CoordManager.XYScale.small)
        {
            //Debug.Log("1X1 Activated");
            transform.localScale = new Vector3(1,1,1);
        }
        if(coord.xyscale == CoordManager.XYScale.middle)
        {
            Debug.Log("2X2 Activated");
            transform.localScale = new Vector3(2,2,1);
        }
    }
}
