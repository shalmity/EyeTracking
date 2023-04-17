using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시선처리 - 고칠것없음 - coord 좌표 값 매니저로 관리(CoordManager 로 넘김)
namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class EyeGazeRayData : MonoBehaviour
            {
                // About Eye Parameter
                EyeData eye;                        // 시각 데이터 파악
                Vector3 CombineGazeRayOrigin;       // 양안 혼합 시선 기준 좌표
                Vector3 CombineGazeRayDirection;    // 양안 혼합 시선 방향 벡터

                Ray CombineRay;                     // Origin + Direction 반영 Ray
                public static FocusInfo CombineFocus;
                float CombineFocusradius;
                float CombineFocusmaxDistance;

                public float ray_X, ray_Y;
                // Eye Parameter End

                public Renderer quad;

                CoordManager coord;

                void Awake()
                {   
                    coord = GameObject.Find("CoordManager").GetComponent<CoordManager>();   //coordManager
                }

                void Update()
                {
                    if(coord.isLoading == false)
                    {
                        if (coord.IsTransfferAbled == true)
                        {
                            CombineGazeRayOrigin = transform.position;

                            Vector3 newCombineGazeRayDirection = new Vector3(CombineGazeRayDirection.x * 8,
                            CombineGazeRayDirection.y * 8, CombineGazeRayDirection.z * 8);

                            if (SRanipal_Eye_API.GetEyeData(ref eye) == ViveSR.Error.WORK)
                            {
                                Debug.DrawRay(CombineGazeRayOrigin, newCombineGazeRayDirection, Color.red);
                                RaycastHit hit;

                                if (Physics.Raycast(CombineGazeRayOrigin, newCombineGazeRayDirection, out hit, LayerMask.GetMask("HeatMapLayer")))
                                {
                                    coord.quadRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                                    ray_X = hit.textureCoord.x;
                                    ray_Y = 1f - hit.textureCoord.y;

                                    coord.Coord_X = hit.textureCoord.x;
                                    coord.Coord_Y = 1f - hit.textureCoord.y;

                                    //Debug.Log("Gaze Origin : Ray_X : " + ray_X + ", Ray_Y : " + ray_Y);
                                }

                            }
                        }
                        Debug.Log("Current Capture System is " + coord.IsTransfferAbled);
                        Debug.Log("Current Capture System is " + coord.IsTransfferAbled);

                        SRanipal_Eye_API.GetEyeData(ref eye);

                        // 양안 시선 인식 시 작동
                        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out CombineGazeRayOrigin, out CombineGazeRayDirection, eye))
                        {

                        }

                        if (SRanipal_Eye.Focus(GazeIndex.COMBINE, out CombineRay, out CombineFocus))
                        {

                        }
                    }
                    else
                    {
                        coord.quadRenderer = quad;
                        for (int i = 0; i < coord.coord_x.Count; i++)
                        {
                            coord.Coord_X = coord.coord_x[i];
                            coord.Coord_Y = coord.coord_y[i];
                        }
                        coord.isLoading = false;
                        //Debug.Log("Gaze Origin : Ray_X : " + ray_X + ", Ray_Y : " + ray_Y);
                    }
                }
            }
        }
    }
}
