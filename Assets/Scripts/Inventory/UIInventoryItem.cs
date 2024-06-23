using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public ItemObject mirror;
    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 resultDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        resultDrag.z = 0;
        transform.position = resultDrag;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        GetComponent<Image>().raycastTarget = true;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        RaycastResult raycastResult = results.Find(res => res.gameObject.CompareTag("Inventory"));
        if (raycastResult.gameObject == null) {
            Instantiate(mirror.preFab, eventData.pointerDrag.transform.position, Quaternion.identity, transform.root.parent);
            Destroy(transform.gameObject);
        }
        
    }
}
