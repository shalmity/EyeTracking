using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 좌표 데이터 매니저로 관리 - 여기서 csv 데이터 파일 출력, 다시 인풋 관리 - 여길 수정해주세요~
public class CoordManager : MonoBehaviour
{
    SaveCSV saveCSV;
    LoadCSV loadCSV;

    // 좌표, 렌더러 전달, 매니저 역할
    public float Coord_X, Coord_Y;
    public List<float> coord_x;
    public List<float> coord_y;

    public Renderer quadRenderer;

    // 시스템 관리 구현
    Dictionary<KeyCode, Action> KeyDictionary;
    public bool IsTransfferAbled = false;
    public enum Distance{close, middle, far}
    public Distance distance = Distance.close;
    public enum XYScale{small, middle, big}
    public XYScale xyscale = XYScale.small;
    public int textureDistance;

    public bool IsLoadAbled = false;
    public bool isRecord = false;
    public bool isLoading = false;


    void Start()
    {
        KeyDictionary = new Dictionary<KeyCode, Action>
        {
            {KeyCode.Space, KeyDown_Space},
            {KeyCode.Q, Distance_Close},
            {KeyCode.E, Distance_Far},
            {KeyCode.A, XYScale_1},
            {KeyCode.S, XYScale_2},
            {KeyCode.W, KeyDown_W}
        };
        
        saveCSV = GameObject.Find("RWManager").GetComponent<SaveCSV>();
        loadCSV = GameObject.Find("RWManager").GetComponent<LoadCSV>();
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            foreach (var dic in KeyDictionary)
            {
                if(Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }
        //Debug.Log("CoordManager : Ray_X : " + Coord_X + ", Ray_Y : " + Coord_Y);
    }

    private void KeyDown_Space()    //Space
    {
        IsTransfferAbled = !IsTransfferAbled;
        //Debug.Log("Current Capture System is " + IsTransfferAbled);
        if(IsTransfferAbled == true)
        {
            saveCSV.sw = new StreamWriter(Application.persistentDataPath+"/"+DateTime.UtcNow.ToLocalTime().ToString("MMdd-HHmmss")+".csv", false, Encoding.GetEncoding("Shift_JIS"));
            string[] s1 = {"Num", "X", "Y"};
            string s2 = string.Join(",", s1);

            saveCSV.sw.WriteLine(s2);
        }

        if(IsTransfferAbled == false)
        {
            saveCSV.sw.Close();
            saveCSV.Num_Limit = 0;
        }
    }

    private void KeyDown_W()
    {
        IsLoadAbled = !IsLoadAbled;
        isLoading = !isLoading;
        if (IsLoadAbled == true)
        {
            loadCSV.LoadCoordsFromCSV();
        }
    }

    private void Distance_Close()   //Q
    {
        if(distance != Distance.close)  {distance = Distance.close;}
    }

    private void Distance_Far() //E
    {
        if(distance != Distance.far)    {distance = Distance.far;}
    }

    private void XYScale_1()    //A
    {
        if(xyscale != XYScale.small)    {xyscale = XYScale.small;}
    }
    private void XYScale_2()    //S
    {
        if(xyscale != XYScale.middle)   {xyscale = XYScale.middle;}
    }
}
