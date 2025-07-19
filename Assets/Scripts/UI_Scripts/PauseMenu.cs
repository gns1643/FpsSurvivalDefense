using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
            {
                CallMenu();
            }
            else
                CloseMenu();
        }
    }
    void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClickSave()
    {
        Debug.Log("���̺�");
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
