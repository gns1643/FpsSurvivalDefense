using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //ȹ�� ������ �ִ� �Ÿ�

    private bool pickupActivated = false; //���氡���Ѱ� ����

    private RaycastHit hitInfo; //�浹ü ���� ����

    // ������ ���̾�� �����ϵ��� ���̾� ����ũ ����
    [SerializeField]
    private LayerMask layerMask;

    //����� ������Ʈ
    [SerializeField]
    private TMP_Text actionText;
    [SerializeField]
    private Inventory theInventory;


    private void Update()
    {
        CheckItem();
        TryAction();
    }

    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickup();
        }
    }
    void CanPickup()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "ȹ��");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }
    void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range))
        {
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }
    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickup>().item.itemName
            + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>";
    }
    void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
