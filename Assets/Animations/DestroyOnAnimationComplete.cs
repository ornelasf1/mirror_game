using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyOnAnimationComplete : MonoBehaviour
{
    public float delay; // seconds
    // Start is called before the first frame update
    void Start()
    {
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animTime + delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
