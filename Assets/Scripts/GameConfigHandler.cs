using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GameConfigHandler
{
    private static string savePath = Application.dataPath + @"/_Data/gameconfig.json";

    public void Save(GameConfig s)
    {
        string json = JsonUtility.ToJson(s);
        using(StreamWriter sw = new StreamWriter(savePath, false, Encoding.GetEncoding("utf-8"))){
            sw.Write(json);
        }
    }

    public GameConfig Load()
    {
        using (StreamReader reader = new StreamReader(savePath, Encoding.GetEncoding("utf-8")))
        {
            string json = reader.ReadToEnd();
            return JsonUtility.FromJson<GameConfig>(json);
        }
    }
}