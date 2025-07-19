using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveDate
{
    public Vector3 playerPos;
}
public class SaveAndLoad : MonoBehaviour
{
    private SaveDate saveDate = new SaveDate();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController thePlayer;
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        thePlayer = FindFirstObjectByType<PlayerController>();

        saveDate.playerPos = thePlayer.transform.position;

        string json = JsonUtility.ToJson(saveDate);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveDate = JsonUtility.FromJson<SaveDate>(loadJson);

            thePlayer = FindFirstObjectByType<PlayerController>();

            thePlayer.transform.position = saveDate.playerPos;

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }
}
