using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotParent;

    //���Ե�
    private Slot[] slots;

    public Slot[] GetSlots()
    {
        return slots;
    }

    [SerializeField] private Item[] items;
    public void LoadToInven(int _arrayNum, string _item, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _item)
            {
                slots[_arrayNum].AddItem(items[i], _itemNum);
            }
        }
    }
    private void Start()
    {
        slots = go_SlotParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        TryOpenInventory();
    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }
    void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }

    void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }
    public void AcquireItem(Item _item, int _count = 1)
    {//ActionController���� ȣ�� ����
        if(_item.itemType != Item.ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {//���� ���� �������� �̹� �� �ִ� ���
                        slots[i].SetSlotCount(_count);
                        return;
                    }

                }
            }
        }
        
        //���� ���� �������� ó�� ���� ��� �Ǵ� ��� �������� ���
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }

    }
}
