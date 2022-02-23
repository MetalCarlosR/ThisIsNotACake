using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingManager : MonoBehaviour
{
    private GameObject _currentCake = null;
    private List<CuttingObjective> _objectives = new List<CuttingObjective>();


    struct CuttingObjective
    {
        private float rotation;
        private float position;
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

    public void NewCake(GameObject cake)
    {
        if (_currentCake || cake.layer != GameManager.instance.CakeLayer())
            return;
        _currentCake = cake;
        _currentCake.transform.position = transform.position;
    }

    public void CheckAndEraseCake(GameObject cakeCut)
    {
        if (cakeCut == _currentCake)
            _currentCake = null;
        Destroy(cakeCut);
    }
}


