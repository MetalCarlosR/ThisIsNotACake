using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private LayerMask cakeLayer;

    void Awake()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }


    public LayerMask CakeMask()
    {
        return cakeLayer;
    }

    public int CakeLayer()
    {
        return (int) Mathf.Log(cakeLayer.value, 2);
    }
}