using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    
    public static void SavePlayer(GameManager gameManager, int slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player_data_" + slot + ".pancreas";
        using FileStream stream = new FileStream(path, FileMode.Create);

        string dataAsJson = JsonUtility.ToJson(gameManager);

        formatter.Serialize(stream, dataAsJson);
        stream.Close();
    }


    public static string LoadPlayer(int slot)
    {
        string path = Application.persistentDataPath + "/player_data_" + slot + ".pancreas";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream stream = new FileStream(path, FileMode.Open);

            string dataAsJson = formatter.Deserialize(stream) as string;
            stream.Close();

            return dataAsJson;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
