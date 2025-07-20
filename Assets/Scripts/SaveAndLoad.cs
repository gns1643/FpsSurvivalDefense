using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveDate
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNum = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNum = new List<int>();
}
public class SaveAndLoad : MonoBehaviour
{
    private SaveDate saveDate = new SaveDate();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController thePlayer;
    private Inventory theInven;
    
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        thePlayer = FindFirstObjectByType<PlayerController>();
        theInven = FindFirstObjectByType<Inventory>();

        saveDate.playerPos = thePlayer.transform.position;
        saveDate.playerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theInven.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveDate.invenArrayNum.Add(i);
                saveDate.invenItemName.Add(slots[i].item.itemName);
                saveDate.invenItemNum.Add(slots[i].itemCount);
            }
        }

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
            theInven = FindFirstObjectByType<Inventory>();

            thePlayer.transform.position = saveDate.playerPos;
            thePlayer.transform.eulerAngles = saveDate.playerRot;

            for (int i = 0; i < saveDate.invenItemName.Count; i++)
            {
                theInven.LoadToInven(saveDate.invenArrayNum[i], saveDate.invenItemName[i], saveDate.invenItemNum[i]);
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }
}
