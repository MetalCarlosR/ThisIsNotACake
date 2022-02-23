using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public PlayerManager PlayerManager;

    //GrabObjects
    public Transform m_KnifeSpotTransform;
    public GameObject m_Knife;
    public Camera m_PlayerCamera;

    private GameObject _lastHighlightedCake = null;
    private Color _lastColor = Color.white;
    [SerializeField] private Color highlightColor;

    float m_CurrentAttachObjectTime;
    public float m_AttachObjectTime = 1f;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerManager.GoToTable();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerManager.GoToPlayer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        if (Input.GetMouseButtonDown(0) && _lastHighlightedCake)
        {
            GameManager.instance.cuttingManager.NewCake(_lastHighlightedCake);
            ResetHighlight();
        }

        
        UpdateAttachedObject();
        HighLightCake();
    }


    private void PickUp()
    {
        Ray l_Ray = m_PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit l_RaycastHit;

        Debug.Log("entra en la funci�n");

        if (Physics.Raycast(l_Ray, out l_RaycastHit, 20f))
        {
            Debug.Log("Se lanza el rayo");

            if (l_RaycastHit.collider.tag == "Knife")
            {
                StartPickUpObject(l_RaycastHit.collider.gameObject, l_RaycastHit.collider.tag);
            }
        }
    }

    private void HighLightCake()
    {
        Ray l_Ray = m_PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(l_Ray, out RaycastHit hit, 20f))
        {
            GameObject g = hit.collider.gameObject;
            if (g.layer != GameManager.instance.CakeLayer() || g == _lastHighlightedCake)
                return;
            if (_lastHighlightedCake)
                ResetHighlight();
            _lastHighlightedCake = g;
            Renderer r = _lastHighlightedCake.GetComponent<Renderer>();
            _lastColor = r.material.color;
            r.material.color = highlightColor;
        }
        else if (_lastHighlightedCake)
            ResetHighlight();
    }

    private void ResetHighlight()
    {
        _lastHighlightedCake.GetComponent<Renderer>().material.color = _lastColor;
        _lastHighlightedCake = null;
    }

    void StartPickUpObject(GameObject AttachObject, string tag)
    {
        if (tag == "Knife")
        {
            if (m_Knife == null)
            {
                m_Knife = AttachObject;
            }
        }
    }

    void UpdateAttachedObject()
    {
        if (m_Knife != null && m_CurrentAttachObjectTime < m_AttachObjectTime)
        {
            m_CurrentAttachObjectTime += Time.deltaTime;
            float l_Pct = Mathf.Min(1.0f, m_CurrentAttachObjectTime / m_AttachObjectTime);

            m_Knife.transform.position = Vector3.Lerp(m_Knife.transform.position, m_KnifeSpotTransform.position, l_Pct);
            m_Knife.transform.rotation =
                Quaternion.Lerp(m_Knife.transform.rotation, m_KnifeSpotTransform.rotation, l_Pct);
            if (l_Pct == 1f)
                m_Knife.transform.SetParent(m_KnifeSpotTransform);
        }
    }
}