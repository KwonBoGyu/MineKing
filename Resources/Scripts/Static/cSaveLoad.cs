using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class cSaveLoad
{
    //======================================릴리즈 모드=========================================//

    public static void SaveData(string pFileName, object pObject)
    {
        string savingFile = JsonUtility.ToJson(pObject);

        File.WriteAllText(
            Application.persistentDataPath + "/Saves/" + pFileName + ".txt",
            savingFile);
    }

    public static T LoadData<T>(string pFileName, ref bool pIsExist)
    {
        if (!File.Exists(Application.persistentDataPath + "/Saves/" + pFileName + ".txt"))
        {
            pIsExist = false;
            Debug.Log("No data - CreatingDefaultSavings");

            File.WriteAllText(
            Application.persistentDataPath + "/Saves/" + pFileName + ".txt",
            "");
        }
        else
            pIsExist = true;

        string loadingData = File.ReadAllText(
            Application.persistentDataPath + "/Saves/" + pFileName + ".txt");

        return JsonUtility.FromJson<T>(loadingData);
    }

    public static T LoadData<T>(string pFileName)
    {
        if (!File.Exists(Application.persistentDataPath + "/Saves/" + pFileName + ".txt"))
        {
            Debug.Log("No data - CreatingDefaultSavings");

            File.WriteAllText(
            Application.persistentDataPath + "/Saves/" + pFileName + ".txt",
            "");
        }

        string loadingData = File.ReadAllText(
            Application.persistentDataPath + "/Saves/" + pFileName + ".txt");

        return JsonUtility.FromJson<T>(loadingData);
    }
}
