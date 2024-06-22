using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot {
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amt) {
        item = _item;
        amount = _amt;
    }

    public void AddAmount(int amt) {
        amount += amt;
    }
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _amount) {
        bool hasItem = false;
        foreach(var slot in Container) {
            if (slot.item == _item) {
                slot.AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem) {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }
}
