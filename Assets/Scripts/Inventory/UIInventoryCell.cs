using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryCell : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            GameObject dropped = eventData.pointerDrag;
            UIInventoryItem inventoryItem = dropped.GetComponent<UIInventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
    
    public void AddItemToSlot(GameObject _itemPrefab) {
        if (transform.childCount == 0) {
            Instantiate(_itemPrefab, transform);
        }
    }
}
