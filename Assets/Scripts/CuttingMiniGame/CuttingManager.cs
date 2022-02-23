using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingManager : MonoBehaviour
{
    public static CuttingManager instance;
    [SerializeField] private LayerMask _cakeLayer;
    private GameObject _currentCake = null;
    private List<CuttingObjective> _objectives = new List<CuttingObjective>();

    struct CuttingObjective
    {
        private float rotation;
        private float position;
    }

    void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
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

    private void NewCake()
    {
        if (_currentCake)
            return;
        _currentCake = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _currentCake.transform.position = transform.position;
        _currentCake.layer = (int) Mathf.Log(_cakeLayer.value, 2);
        
        
    }

    public void CheckAndEraseCake(GameObject cakeCut)
    {
        if (cakeCut == _currentCake)
            _currentCake = null;
        Destroy(cakeCut);
    }
}


