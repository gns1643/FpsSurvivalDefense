using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject 
{
    public string itemName; //아이템의 이름
    [TextArea]
    public string itemDesc; //아이템의 설명
    public ItemType itemType; //아이템의 유형
    public Sprite itemImage; //아이템의 이미지
    public GameObject itemPrefab; //아이템의 프리팹

    public string weaponType; //무기 유형
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}
