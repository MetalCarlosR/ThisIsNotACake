using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class CuttingPlane : MonoBehaviour
{
    
    [SerializeField] private LayerMask _cutMask;
    
    void Update()
    {
        float dir = Input.GetAxis("Mouse X");
        transform.Translate(Vector3.up * -dir * 0.5f);
        if(Input.GetKeyDown(KeyCode.Space))
            Cut();
    }
    
    
    public void Cut()
    {
        Collider[] cuts = Physics.OverlapBox(transform.position, new Vector3(10, 0.1f, 10),
            transform.rotation, _cutMask);
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
                CuttingManager.instance.CheckAndEraseCake(cut.gameObject);
            }
        }
    }

    private void SetupHull(GameObject hull)
    {
        hull.layer = (int) Mathf.Log(_cutMask.value, 2) ;
        Rigidbody rb = hull.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider coll = hull.AddComponent<MeshCollider>();
        coll.convex = true;
        rb.AddExplosionForce(100,hull.transform.position,10);
        
    }
}
