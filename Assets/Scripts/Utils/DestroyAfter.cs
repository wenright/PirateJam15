using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{

    public float waitTime = 1;

    void Start()
    {
        Destroy(gameObject, waitTime);
    }
}
