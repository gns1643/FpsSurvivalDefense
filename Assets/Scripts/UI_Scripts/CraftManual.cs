using UnityEditor.PackageManager.UI;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; //�̸�
    public GameObject go_Prefab; //������
    public GameObject go_PreviewPrefab; //�̸����� ������
}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField]
    private GameObject go_BaseUI; //�⺻ ���̽� UI 

    [SerializeField]
    private Craft[] craft_Fire; //��ںҿ� ��

    private GameObject go_Preview; // �̸����� �������� ���� ����
    private GameObject go_Prefab; // ������ �������� ���� ����

    [SerializeField]
    private Transform tf_Player; //�÷��̾��� ��ġ

    //Raycast �ʿ� ���� ����
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_Fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_Fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
        GameManager.isOpenCraftMenu = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            WindowOn();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }
    void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask)){
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    void Close()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }

    void WindowOn()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }
    void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
        GameManager.isOpenCraftMenu = true;
    } 
    void CloseWindow()
    {
        
        isActivated = false;
        go_BaseUI.SetActive(false);
        GameManager.isOpenCraftMenu = false;
    }
}
