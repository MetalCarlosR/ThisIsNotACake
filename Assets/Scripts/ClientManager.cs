using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    /*
     * Este script debe ocuparse de spawnear nuevos clientes
     * También de hacer que los clientes pasen a la siguiente posición de la cola
     * Y que desaparezcan cuando se hayan ido
     * */
    public List<GameObject> ClientsInGame = new List<GameObject>();

    public GameObject Client;

    PoolElements m_ClientsPool;

    public Transform m_ClientsSpawnSpot;
    public GameObject m_FirstPosition;


    private void Awake()
    {
        m_ClientsPool = new PoolElements(30, transform, Client);
    }

    public void MoveClients()
    {
        for (int i = 0; i < ClientsInGame.Count; i++)
        {
            if (ClientsInGame[i].GetComponent<Client>().m_IsWaiting)
            {
                ClientsInGame[i].SetActive(false);
            }
        }
    }

    public void RemoveClientFromList()
    {
        for (int i = 0; i < ClientsInGame.Count; i++)
        {
            if (ClientsInGame[i].GetComponent<Client>().m_IsOver)
            {
                ClientsInGame[i].GetComponent<Client>().NextPosition();
            }
        }
    }

    public void SpawnClient()
    {
        GameObject l_NewClient = m_ClientsPool.GetNextElement();
        ClientsInGame.Add(l_NewClient);
        l_NewClient.transform.position = m_ClientsSpawnSpot.position;
        l_NewClient.GetComponent<Client>().m_CurrentPosition = m_FirstPosition;
    }
}
