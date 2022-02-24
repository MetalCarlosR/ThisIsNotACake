using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class CuttingManager : MonoBehaviour
{
    [SerializeField] private Collider _cuttingBounds;
    [SerializeField] private Material _interiorMat;
    [SerializeField] private Collider _cuttingBoard;
    private GameObject _currentCake = null;
    private List<CutInfo> _objectives;
    private List<CutInfo> _cutInfos;

    struct CutInfo
    {
        public Vector3 position;
        public float rotation;
        
        public CutInfo(Vector3 pos, float rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    void Start()
    {
        //GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //g.layer = GameManager.instance.CakeLayer();
        //NewCake(g);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            RotateCake(true);
        else if (Input.GetKey(KeyCode.E))
            RotateCake(false);
        else if (Input.GetKeyDown(KeyCode.Space))
            MakeCuts();
    }

    private void RotateCake(bool clockwise)
    {
        if(!_currentCake)
            return;
        
        float anglePerSencond = 45;
        float rotDir = Time.deltaTime * (clockwise ? 1 : -1) * anglePerSencond;
        
        _currentCake.transform.Rotate(transform.up,rotDir);
    }

    public void NewCake(GameObject cake)
    {
        if (_currentCake || cake.layer != GameManager.instance.CakeLayer())
            return;
        _currentCake = cake;
        _currentCake.transform.position = transform.position;
        
        
        Vector3 szA = _cuttingBounds.bounds.size;
        Vector3 szB = cake.GetComponent<Collider>().bounds.size;
        
        cake.transform.localScale = new Vector3(szA.x / szB.x, szA.y / szB.y, szA.z / szB.z);
        _cutInfos = new List<CutInfo>();
        _objectives = new List<CutInfo>();

        int numCuts = Random.Range(1, 4);

        for (int i = 0; i < numCuts; i++)
        {
            CutInfo info = new CutInfo();
            float extends = _cuttingBoard.bounds.extents.z;
            info.position = Vector3.forward * Random.Range(-extends, extends);
            info.rotation = Random.Range(0, 360f);
            _objectives.Add(info);
            print(info.position + " " + info.rotation);
        }
    }

    public void NewCut(Vector3 position)
    {
        if(_currentCake)
            _cutInfos.Add(new CutInfo(position, _currentCake.transform.rotation.eulerAngles.y));
    }
    
    private void MakeCuts()
    {
        if(_cutInfos.Count == 0 || !_currentCake)
            return;

        int obj = _objectives.Count;
        float cakeRot = _currentCake.transform.rotation.eulerAngles.y;
        foreach (CutInfo cut in _cutInfos)
        {
            Collider[] cuts = Physics.OverlapBox(cut.position, new Vector3(10, 0.1f, 10),
                transform.rotation, GameManager.instance.CakeMask());
            if(cuts.Length <= 0)
                return;

            foreach (CutInfo objective  in _objectives)
            {
                float distanceRot = Mathf.Abs(objective.rotation - cut.rotation);
                float distancePos = Mathf.Abs(objective.position.z - cut.position.z);
                if (distanceRot <= 5 && distancePos <= 0.05f)
                {
                    _objectives.Remove(objective);
                    print("Well Done");
                    break;
                }
            }

            foreach (Collider col in cuts)
            {
                float radians = Mathf.Deg2Rad * (cut.rotation - cakeRot);
                float r = Mathf.Cos(radians);
                float f = Mathf.Sin(radians);
                SlicedHull hull = col.gameObject.Slice(cut.position, transform.right * r + transform.forward * f, null);
                if (hull != null)
                {
                    GameObject bot = hull.CreateLowerHull(col.gameObject,_interiorMat);
                    GameObject top = hull.CreateUpperHull(col.gameObject,_interiorMat);
                    SetupHull(bot);
                    SetupHull(top);
                    GameManager.instance.cuttingManager.CheckAndEraseCake(col.gameObject);
                }
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
        rb.AddExplosionForce(20,hull.transform.position,1);
        
    }

    public void CheckAndEraseCake(GameObject cakeCut)
    {
        if (cakeCut == _currentCake)
            _currentCake = null;
        cakeCut.layer = 0;
        Destroy(cakeCut);
    }

    public Bounds CuttingBounds()
    {
        return _cuttingBoard.bounds;
    }
}


