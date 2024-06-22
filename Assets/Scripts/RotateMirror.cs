using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMirror : MonoBehaviour
{
    public GameObject mirrorObject;
    private bool isRotating;
    private float gapEulerAngle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // oldEulerAngle = mirrorObject.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating) {
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -10;
    
            Vector3 objectPos = Camera.main.WorldToScreenPoint(mirrorObject.transform.position);
            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;
    
            float newAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            float newEulerAngle = newAngle >= 0f ? newAngle : newAngle + 360f;
            float currentAngle = mirrorObject.transform.rotation.eulerAngles.z;
            if (gapEulerAngle == 0f) {
                gapEulerAngle = Math.Abs(currentAngle - newEulerAngle);
            }
            mirrorObject.transform.eulerAngles = new Vector3(0, 0, (gapEulerAngle + newEulerAngle) % 360);
        } else {
            gapEulerAngle = 0f;
        }
    }

    private void OnMouseDown() {
        isRotating = true;
    }

    private void OnMouseUp() {
        isRotating = false;
    }
}
