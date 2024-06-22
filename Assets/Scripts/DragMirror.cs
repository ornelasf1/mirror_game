using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMirror : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    public GameObject mirrorObject;
    public LayerMask mirrorMask;

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
            mirrorObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

        }
    }

    private void OnMouseDown()
    {
        // Record the difference between the objects centre, and the clicked point on the camera plane.
        offset = mirrorObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging.
        dragging = false;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, mirrorMask);
        if (hit)
        {
            mirrorObject.transform.position = hit.transform.position;
        }
    }
}
