using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera billboardCamera;

    void Start()
    {
        if (!billboardCamera)
        {
            billboardCamera = Camera.main;
        }
    }
    
    void Update()
    {
        transform.rotation = billboardCamera?.transform.rotation ?? Quaternion.identity;
    }
}
