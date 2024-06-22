using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragLaser : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset;
    (float lower, float upper) verticalBounds = (-19f, 15f);
    (float xPos, float rotation) leftSideScreenXPosition = (0f, -90f);
    (float xPos, float rotation) rightSideScreenXPosition = (0f, 90f);

    // Start is called before the first frame update
    void Start()
    {
        // leftSideScreenXPosition.xPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        // rightSideScreenXPosition.xPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        // transform.position = new Vector3(rightSideScreenXPosition.xPos, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) return;
        if (dragging)
        {
            // Move object, taking into account original offset.
            Vector3 dragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            float newY = Math.Min(verticalBounds.upper, Math.Max(dragPosition.y, verticalBounds.lower));
            float leftDifference = leftSideScreenXPosition.xPos - dragPosition.x;
            float rightDifference = rightSideScreenXPosition.xPos - dragPosition.x;
            float newX;
            float newRotation;
            if (Math.Abs(leftDifference) < Math.Abs(rightDifference)) {
                newX = leftSideScreenXPosition.xPos;
                newRotation = leftSideScreenXPosition.rotation;
            } else {
                newX = rightSideScreenXPosition.xPos;
                newRotation = rightSideScreenXPosition.rotation;
            }
            transform.position = new Vector3(newX, newY, dragPosition.z);
            transform.eulerAngles = Vector3.forward * newRotation;
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
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity);
        if (hit)
        {
            transform.position = hit.transform.position;
        }
    }
}
