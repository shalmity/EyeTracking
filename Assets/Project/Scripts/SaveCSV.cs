using System.IO;
using System.Text;
using UnityEngine;

public class SaveCSV : MonoBehaviour
{
    CoordManager coordManager;
    public StreamWriter sw;

    private float time_Repeat;
    private float timeDelay_Repeat;
    public float Num_Count;

    public int Num_Limit;
    

    void Start()
    {
        coordManager = GameObject.Find("CoordManager").GetComponent<CoordManager>();
        time_Repeat = 0f;
        timeDelay_Repeat = 0.01f;
    }


    void Update()
    {
        time_Repeat = time_Repeat + 1f * Time.deltaTime;
        if(time_Repeat >= timeDelay_Repeat)
        {
            time_Repeat = 0f;
            if(coordManager.IsTransfferAbled == true)
            {
                Num_Count = Num_Count + 1;
                SaveXYtoCSV(Num_Count.ToString(), coordManager.Coord_X.ToString(), coordManager.Coord_Y.ToString());
            }
        }
    }

    public void SaveXYtoCSV(string txt1, string txt2, string txt3)
    {
        string[] s1 = {txt1,txt2,txt3};
        string s2 = string.Join(",", s1);

        sw.WriteLine(s2);
    }
}
