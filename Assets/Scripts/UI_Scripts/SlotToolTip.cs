using TMPro;
using UnityEngine;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private TMP_Text text_ItemName;
    [SerializeField]
    private TMP_Text text_Desc;
    [SerializeField]
    private TMP_Text text_HowToUse;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.7f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.7f, 0f);
        go_Base.transform.position = _pos;

        text_ItemName.text = _item.itemName;
        text_Desc.text = _item.itemDesc;
        if (_item.itemType == Item.ItemType.Equipment)
            text_HowToUse.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            text_HowToUse.text = "우클릭 - 사용";
        else
            text_HowToUse.text = "";
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
