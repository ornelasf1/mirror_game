using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseMirrorDrag : MonoBehaviour
{
    public ItemObject mirror;
    private bool dragging = false;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            // Move object, taking into account original offset.
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

        }
    }

    private void OnMouseDown()
    {
        // Record the difference between the objects centre, and the clicked point on the camera plane.
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging.
        dragging = false;
        // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, layerToSnapTo);
        // Debug.Log(hit.transform.name);
        // if (hit)
        // {
        //     transform.position = hit.transform.position;
        // }

        UIImages images = UIRaycast(Helpers.ScreenPosToPointerData(Input.mousePosition));
        if (images.uiInventoryCell != null)
        {
            UIInventoryCell inventoryCell = images.uiInventoryCell.GetComponent<UIInventoryCell>();
            if (inventoryCell != null)
            {
                Debug.Log($"Cell: Place {transform.name} at {images.uiInventoryCell.transform.name}");
                inventoryCell.AddItemToSlot(mirror.inventoryImage);
                Destroy(transform.gameObject);
            }
        } else if (images.uiInventory != null)
        {
            for (int i = 0; i < images.uiInventory.transform.childCount; i++)
            {
                Transform childTransform = images.uiInventory.transform.GetChild(i);
                UIInventoryCell inventoryCell = childTransform.GetComponent<UIInventoryCell>();
                if (inventoryCell != null && inventoryCell.transform.childCount == 0)
                {
                    Debug.Log($"Inv: Place {transform.name} at {childTransform.name}");
                    inventoryCell.AddItemToSlot(mirror.inventoryImage);
                    Destroy(transform.gameObject);
                    break;
                }
            }
        }
    }

    private UIImages UIRaycast(PointerEventData pointerData)
    {
        UIImages images = new();
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count == 0)
        {
            return images;
        }
        RaycastResult foundCell = results.Find(result => result.gameObject.CompareTag("InventoryCell"));
        RaycastResult foundInventory = results.Find(result => result.gameObject.CompareTag("Inventory"));
        images.uiInventory = foundInventory.gameObject;
        images.uiInventoryCell = foundCell.gameObject;

        return images;
    }
}

struct UIImages
{
    public GameObject uiInventory;
    public GameObject uiInventoryCell;
}