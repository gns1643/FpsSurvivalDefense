using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName;
    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }
    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }
    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}

