using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public LayerMask layersToCheckOverlapWithCell;
    private GameObject itemInCell;

    void Update() {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Rect rect = GetWorldRect(rectTransform);
        Vector3 worldTransform = rectTransform.TransformPoint(transform.position);
        Vector3 topLeft = worldTransform;
        Vector3 bottomRight = worldTransform;
        float width = rect.width;
        float height = rect.height;
        topLeft.x -= width / 2;
        topLeft.y -= height / 2;
        bottomRight.x += width / 2;
        bottomRight.y += height / 2;
        Debug.DrawRay(topLeft, Vector3.back * 10, Color.green);
        Debug.DrawRay(bottomRight, Vector3.back * 10, Color.green);

        // Collider2D overlappedCollider = Physics2D.OverlapPoint(transform.position, layersToCheckOverlapWithCell);
        Collider2D overlappedCollider = Physics2D.OverlapArea(topLeft, bottomRight, layersToCheckOverlapWithCell);
        Debug.DrawRay(transform.position, Vector3.back * 10);
        if (HasItem() && !overlappedCollider) {
            Debug.Log($"Overlapped collider {overlappedCollider} : Remove item in {gameObject.name}");
            itemInCell = null;
        }
        Image image = transform.GetComponent<Image>();
        Color color = image.color;
        if (HasItem()) {
            color.a = 0.9f;
        } else {
            color.a = .29f;
        }
        image.color = color;
    }

    public void AddItem(GameObject item) {
        if (itemInCell == null) {
            Vector3 newPos = transform.position;
            newPos.z = item.transform.position.z;
            item.transform.position = newPos;
            itemInCell = item;
        }
    }

    public bool HasItem() {
        return itemInCell != null;
    }

    public Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // Get the bottom left corner.
        Vector3 position = corners[0];
        
        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y);

        return new Rect(position, size);
    }
}
