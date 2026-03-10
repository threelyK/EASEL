using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{

    [SerializeField] public List<ItemSO> items;
    public int currencyAmount;

}
