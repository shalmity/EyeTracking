using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCSV : MonoBehaviour
{
    public string filename;
    public CoordManager coordManager;
    public string[] csvFiles;
    public string[] csvFileNames;
    string[] text;
    int length;
    public void Start()
    {
        coordManager = GameObject.Find("CoordManager").GetComponent<CoordManager>();

        csvFiles = Directory.GetFiles(Application.persistentDataPath, "*.csv");
        csvFileNames = new string[csvFiles.Length];

        for (int i = 0; i < csvFiles.Length; i++)
        {
            text = csvFiles[i].Split('\\');
            csvFileNames[i] = text[1];
        }
    }

    public void LoadCoordsFromCSV()
    {
        length = csvFileNames.Length - 1;
        filename = csvFileNames[length];
        List<string[]> lines = new List<string[]>();
        StreamReader reader = new StreamReader(Application.persistentDataPath + "/" + filename);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            lines.Add(values);
        }
        reader.Close();
        for (int i = 1; i < lines.Count; i++)
        {
            float x = float.Parse(lines[i][1]);
            float y = float.Parse(lines[i][2]);
            coordManager.coord_x.Add(x);
            coordManager.coord_y.Add(y);
        }
    }
}

