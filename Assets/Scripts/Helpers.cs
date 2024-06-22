using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Helpers
{
    public static GameObject FindGameObjectInChildWithTag(GameObject gameObject, string tag) {
        if (gameObject == null) {
            return null;
        }
        Transform t = gameObject.transform;

        if (t.childCount == 0) {
            return null;
        }

		for (int idx = 0; idx < t.childCount; idx++) 
		{
			if(t.GetChild(idx).gameObject.CompareTag(tag))
			{
				return t.GetChild(idx).gameObject;
			} else {
                GameObject go = FindGameObjectInChildWithTag(t.GetChild(idx).gameObject, tag);
                if (go != null) {
                    return go;
                }
            }
				
		}
			
		return null;
    }

    public static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
       => new(EventSystem.current) { position = screenPos };
}
