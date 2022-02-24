using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingPlane : MonoBehaviour
{
    private float centerPos, maxPos, minPos;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        centerPos = transform.position.z;
        float halfExtents = GameManager.instance.cuttingManager.CuttingBounds().extents.z;
        maxPos = centerPos + halfExtents;
        minPos = centerPos - halfExtents;
    }
    
    void Update()
    {
        float dir = Input.GetAxis("Mouse X");
        transform.Translate(Vector3.up * -dir * 0.5f);
        if(transform.position.z > maxPos || transform.position.z < minPos)
            transform.Translate(Vector3.up * dir * 0.5f);
        if(Input.GetMouseButtonDown(0))
            Cut();
    }
    
    
    private void Cut()
    {
        GameManager.instance.cuttingManager.NewCut(transform.position);
    }
}
