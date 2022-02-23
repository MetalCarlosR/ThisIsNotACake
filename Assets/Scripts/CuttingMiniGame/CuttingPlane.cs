using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class CuttingPlane : MonoBehaviour
{
    [SerializeField] private Collider cuttingBoard;
    private float centerPos, maxPos, minPos;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        centerPos = transform.position.z;
        float halfExtents = cuttingBoard.bounds.extents.z;
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
    
    
    public void Cut()
    {
        Collider[] cuts = Physics.OverlapBox(transform.position, new Vector3(10, 0.1f, 10),
            transform.rotation, GameManager.instance.CakeMask());
        if(cuts.Length <= 0)
            return;

        foreach (Collider cut in cuts)
        {
            SlicedHull hull = cut.gameObject.Slice(transform.position, transform.up, null);
            if (hull != null)
            {
                GameObject bot = hull.CreateLowerHull(cut.gameObject,null);
                GameObject top = hull.CreateUpperHull(cut.gameObject,null);
                SetupHull(bot);
                SetupHull(top);
                GameManager.instance.cuttingManager.CheckAndEraseCake(cut.gameObject);
            }
        }
    }

    private void SetupHull(GameObject hull)
    {
        hull.layer = GameManager.instance.CakeLayer();
        Rigidbody rb = hull.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider coll = hull.AddComponent<MeshCollider>();
        coll.convex = true;
        rb.AddExplosionForce(100,hull.transform.position,10);
        
    }
}
