using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class Cuttingtest : MonoBehaviour
{
    [SerializeField] private Transform _cutPlane = null;
    [SerializeField] private LayerMask _cutMask;
    [SerializeField] private GameObject _currentCake = null;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float dir = Input.GetAxis("Mouse X");
        _cutPlane.Translate(Vector3.up * -dir * 0.5f);
        if(Input.GetKeyDown(KeyCode.Space))
            Cut();
        else if(Input.GetKeyDown(KeyCode.F))
            NewCake();
        else if (Input.GetKey(KeyCode.Q))
            RotateCake(true);
        else if (Input.GetKey(KeyCode.E))
            RotateCake(false);
    }

    private void RotateCake(bool clockwise)
    {
        if(!_currentCake)
            return;

        float anglePerSencond = 45;
        float rotDir = Time.deltaTime * (clockwise ? 1 : -1) * anglePerSencond;
        
        _currentCake.transform.Rotate(transform.up,rotDir);
    }

    private void Cut()
    {
        Collider[] cuts = Physics.OverlapBox(_cutPlane.transform.position, new Vector3(10, 0.1f, 10),
            _cutPlane.rotation, _cutMask);
        if(cuts.Length <= 0)
            return;

        foreach (Collider cut in cuts)
        {
            SlicedHull hull = cut.gameObject.Slice(_cutPlane.position, _cutPlane.up, null);
            if (hull != null)
            {
                GameObject bot = hull.CreateLowerHull(cut.gameObject,null);
                GameObject top = hull.CreateUpperHull(cut.gameObject,null);
                SetupHull(bot);
                SetupHull(top);
                if (cut == _currentCake)
                    _currentCake = null;
                Destroy(cut.gameObject);
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

    private void NewCake()
    {
        if (_currentCake)
            return;
        _currentCake = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _currentCake.transform.position = transform.position;
        _currentCake.layer = (int) Mathf.Log(_cutMask.value, 2);
    }
}


