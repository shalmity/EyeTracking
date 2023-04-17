using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전체 합체 - 더이상 사용 안함 - 참고용
namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class EyeGazeRayCastQuad : MonoBehaviour
            {
                // About Eye Parameter
                EyeData eye;                        // 시각 데이터 파악
                Vector3 CombineGazeRayOrigin;       // 양안 혼합 시선 기준 좌표
                Vector3 CombineGazeRayDirection;    // 양안 혼합 시선 방향 벡터

                Ray CombineRay;                     // Origin + Direction 반영 Ray
                public static FocusInfo CombineFocus;
                float CombineFocusradius;
                float CombineFocusmaxDistance;
                // Eye Parameter End

                // About Shader Interaction
                public GameObject Origin;

                int num = 120;                      // 수집 가능 횟수
                Material mMaterial;
                MeshRenderer mMeshRenderer;

                float[] mPoints;
                int mHitCount;

                float ray_X, ray_Y;

                private GameObject quad;
                private Texture textures;
                private Renderer quadRenderer;
                private Texture MainTextures;
                // Shader Interaction End

                void Start()
                {
                    mMeshRenderer = GetComponent<MeshRenderer>();
                    mMaterial = mMeshRenderer.material;
                    MainTextures = mMaterial.mainTexture;
                    mPoints = new float[num * 3];
                }

                void Update()
                {
                    CombineGazeRayOrigin = Origin.transform.position;

                    // 기존 Direction은 너무 작아서 광선이 보이지 않음, 정배를 통해 광선 크기를 늘리고 목표하는 시선 처리 영역을 설정
                    Vector3 newCombineGazeRayDirection = new Vector3(CombineGazeRayDirection.x*8, 
                    CombineGazeRayDirection.y*8, CombineGazeRayDirection.z*8);
                    
                    // 양안 인식 시 조건문 수행
                    if (SRanipal_Eye_API.GetEyeData(ref eye) == ViveSR.Error.WORK)
                    {
                        Debug.DrawRay(CombineGazeRayOrigin, newCombineGazeRayDirection, Color.blue);
                        RaycastHit hit;

                        if(Physics.Raycast(CombineRay, out hit, LayerMask.GetMask("HeatMapLayer")))
                        {
                            quadRenderer = hit.transform.gameObject.GetComponent<Renderer>();

                            ray_X = hit.textureCoord.x;
                            ray_Y = 1f - hit.textureCoord.y;

                            Debug.Log("1 : Ray_X : " + ray_X + ", Ray_Y : " + ray_Y);

                            addHitPoint(ray_X*4-2, -(ray_Y*4-2));
                        
                        }
                    }

                    SRanipal_Eye_API.GetEyeData(ref eye);

                    // 양안 시선 인식 시 작동
                    if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out CombineGazeRayOrigin, out CombineGazeRayDirection, eye))
                    {
                    }

                    // 양안 시선 인식 후 최초로 부딪히는 오브젝트에 접근
                    if (SRanipal_Eye.Focus(GazeIndex.COMBINE, out CombineRay, out CombineFocus))
                    {                        
                    }
                }
                public void addHitPoint(float x, float y)
                {
                    if (MainTextures != quadRenderer.material.mainTexture)
                    {
                        mHitCount = 0;
                        MainTextures = quadRenderer.material.mainTexture;
                    }
                    //수정

                    mPoints[mHitCount * 3] = x;
                    mPoints[mHitCount * 3 + 1] = y;
                    mPoints[mHitCount * 3 + 2] = Random.Range(1f, 1.5f);

                    mHitCount++;
                    mHitCount %= num;

                    mMaterial.SetFloatArray("_Hits", mPoints);
                    mMaterial.SetInt("_HitCount", mHitCount);
                }
            }
        }
    }
}