using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mirror", menuName = "Inventory System/Items/Mirror")]
public class MirrorObject : ItemObject
{
    // Start is called before the first frame update
    void Awake() {
        type = ItemType.Mirror;
    }
}
