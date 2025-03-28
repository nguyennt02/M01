using System.IO;
using UnityEngine;

public static class Utility
{
    public static string GetRawDataPathFrom(string fileName)
    {
        string dataPath;
#if UNITY_EDITOR
        dataPath = Path.Combine(Application.dataPath, $"{fileName}.txt");
#else
        dataPath = Path.Combine(Application.persistentDataPath, $"{fileName}.txt");
#endif
        return dataPath;
    }
    public static void SaveToFile<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data);
        WriteAlTo(json, fileName);
    }

    public static T LoadDataFromFile<T>(string fileName)
    {
        string json = LoadDataFrom(fileName);
        if (string.IsNullOrEmpty(json))
            return default(T);
        else
            return JsonUtility.FromJson<T>(json);
    }

    static void WriteAlTo(string json, string fileName)
    {
        string path = GetRawDataPathFrom(fileName);
        File.WriteAllText(path, json);
    }

    static string LoadDataFrom(string fileName)
    {
        string path = GetRawDataPathFrom(fileName);
        if (File.Exists(path))
        {
            Debug.Log("load data sucess :" + path);
            return File.ReadAllText(path);
        }
        else
        {
            Debug.LogWarning("File not found!");
            return null;
        }
    }
}