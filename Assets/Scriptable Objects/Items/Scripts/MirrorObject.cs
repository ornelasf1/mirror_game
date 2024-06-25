using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MirrorType {
    Square,
    HalfCircle
}

[CreateAssetMenu(fileName = "Mirror", menuName = "Inventory System/Items/Mirror")]
public class MirrorObject : ItemObject
{
    public MirrorType mirrorType;
    // Start is called before the first frame update
    void Awake() {
        type = ItemType.Mirror;
    }
}