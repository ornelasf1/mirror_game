using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Default,
    Mirror
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject preFab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
