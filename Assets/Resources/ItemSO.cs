using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string itemDescription;

    public Sprite itemIcon;
    public GameObject itemObj;
}
