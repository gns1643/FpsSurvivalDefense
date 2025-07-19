using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public static Title instance;

    public string sceneName;

    private SaveAndLoad theSaveLoad;

    private void Awake()
    {
        theSaveLoad = FindFirstObjectByType<SaveAndLoad>();
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
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }
    public void ClickLoad()
    {
        Debug.Log("로드");
        SceneManager.LoadScene(sceneName);
        theSaveLoad.LoadData();
    }
    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}

