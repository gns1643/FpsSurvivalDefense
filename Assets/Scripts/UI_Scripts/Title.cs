using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public static Title instance;

    public string sceneName;

    private SaveAndLoad theSaveLoad;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    public void ClickStart()
    {
        Debug.Log("�ε�");
        gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }
    public void ClickLoad()
    {
        Debug.Log("�ε�");
        StartCoroutine(LoadCoroutine());
        
    }
    IEnumerator LoadCoroutine()
    {
        //SceneManager.LoadScene(sceneName);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        theSaveLoad = FindFirstObjectByType<SaveAndLoad>();
        theSaveLoad.LoadData();
        gameObject.SetActive(false);
    }
    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}

