using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMBorrar : MonoBehaviour
{
    public static PlayerMBorrar instance;

    private bool _cuttingBoardState = true;

    [SerializeField] private GameObject _player, _cuttingBoardCamera; 
    
    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }


    public void Swap()
    {
        _cuttingBoardState = !_cuttingBoardState;
        
        _player.SetActive(_cuttingBoardCamera);
        _cuttingBoardCamera.SetActive(!_cuttingBoardCamera);
    }
}
