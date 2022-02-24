using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    /*
     * Esta clase es individual para cada cliente
     * Ha de controlar:
           * La llegada a la zona de pedir
           * Que salga el bocadillo de su pedido
           * Comprovar cuando se ha cortado ya el pedido
           * Irse de la zona de pedir
    */

    public enum State {INITIAL, WAITING, GOINGTOTABLE, INTABLE, LEAVING };

    public State m_CurrentState = State.INITIAL;

    public Transform LeavePoint, TablePoint;
    public Transform LookPlayerPoint;

    public bool m_IsWaiting = true;
    public bool m_Leave = false;
    public bool m_GoToTable = false;
    public bool m_IsInTable = false;
    public bool m_IsOver = false;

    public float TimeToLeave;

    public Image m_Bubble;

    public GameObject m_CurrentPosition;

    public ClientManager m_ClientManager;

    // Start is called before the first frame update
    void Start()
    {
        m_Bubble.enabled = false;
        StartCoroutine(MoveToNextPosition(2f));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) NextPosition();

        switch (m_CurrentState)
        {
            case State.INITIAL:
                ChangeState(State.WAITING);
                break;

            case State.WAITING:
                //Hacer condicion que le diga al cliente cuando es su turno
                if(m_GoToTable)
                {
                    ChangeState(State.GOINGTOTABLE);
                }

                break;

            case State.GOINGTOTABLE:
                //Condicion que compruebe cuando llega a la mesa
                if (m_IsInTable)
                {
                    ChangeState(State.INTABLE);
                }
                GoToTable();
                break;

            case State.INTABLE:
                //Condicion que compruebe si el pastel ha sido servido correctamente
                //TODO: Hacer que detecte cuando el pastel está cortado y el cliente se puede marchar
                if (m_Leave)
                {
                    ChangeState(State.LEAVING);
                }

                
                break;

            case State.LEAVING:
                Leave();
                break;
        }
    }

    void ChangeState(State newState)
    {
        //Exit state logic
        switch (m_CurrentState)
        {
            case State.WAITING:
                m_IsWaiting = false;
                break;

            case State.GOINGTOTABLE:
                m_GoToTable = false;
                break;

            case State.INTABLE:
                m_IsInTable = false;
                m_Bubble.enabled = false;

                break;

            case State.LEAVING:
                
                break;
        }

        //Enter state logic
        switch (newState)
        {
            case State.WAITING:
                m_IsWaiting = true;
                break;

            case State.GOINGTOTABLE:
                
                break;

            case State.INTABLE:
                m_Bubble.enabled = true;
                break;

            case State.LEAVING:
                m_ClientManager.MoveClients();
                break;
        }

        m_CurrentState = newState;
    }

    void Leave()
    {
        m_IsInTable = false;

        //Rotar el cliente hacia la salida
        Vector3 LookAtLeavePoint = new Vector3(LeavePoint.position.x, transform.position.y, LeavePoint.position.z);
        transform.LookAt(LookAtLeavePoint);

        //Mover al cliente hacia la salida
        transform.position = Vector3.MoveTowards(transform.position, LeavePoint.position, 2.0f * Time.deltaTime);

        if (transform.position == LeavePoint.position)
        {
            m_IsOver = true;
        }
    }

    void GoToTable()
    {
        Vector3 LookAtLeavePoint = new Vector3(TablePoint.position.x, transform.position.y, TablePoint.position.z);
        transform.LookAt(LookAtLeavePoint);

        //Movel el cliente hacia la mesa
        transform.position = Vector3.MoveTowards(transform.position, TablePoint.position, 2.0f * Time.deltaTime);


        if (transform.position == TablePoint.position)
        {
            Vector3 LookAtPlayer = new Vector3(LookPlayerPoint.position.x, transform.position.y, LookPlayerPoint.position.z);
            transform.LookAt(LookAtPlayer);
            m_IsInTable = true;
        }
    }

    public void NextPosition()
    {
        if (m_CurrentPosition.tag != "LastPosition")
        {
            StartCoroutine(MoveToNextPosition(0.1f));            
        }
        else
        {
            m_GoToTable = true;
        }
    }

    IEnumerator MoveToNextPosition(float speed)
    {
        GameObject NextPosition = m_CurrentPosition.GetComponent<PositionScript>().NextPositionGO;
        while (transform.position != NextPosition.transform.position)
        {
            Vector3 LookAtLeavePoint = new Vector3(NextPosition.transform.position.x, transform.position.y, NextPosition.transform.position.z);
            transform.LookAt(LookAtLeavePoint);

            //Mover al cliente hacia el siguiente punto de espera
            transform.position = Vector3.MoveTowards(transform.position, NextPosition.transform.position, speed * Time.deltaTime);

            
            yield return null;
        }
        
            m_CurrentPosition = NextPosition;
        
    }
}
