using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementCut : MonoBehaviour
{
    [Header("CameraControll")]
    public float m_YawRotationalSpeed = 360.0f;
    public float m_PitchRotationalSpeed = 180.0f;
    public float m_MinPitch = -90.0f;
    public float m_MaxPitch = 90.0f;
    public Transform m_PitchControllerTransform;
    public bool m_InvertedYaw = false;
    public bool m_InvertedPitch = true;
    float m_Yaw;
    float m_Pitch;
    

    // Start is called before the first frame update
    private void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float l_MouseAxisY = Input.GetAxis("Mouse Y");
        float l_MouseAxisX = Input.GetAxis("Mouse X");


        if (m_InvertedPitch == false) m_Pitch -= l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime;
        else m_Pitch += l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime;

        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        if (m_InvertedYaw == false) m_Yaw += l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime;
        else m_Yaw -= l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchControllerTransform.localRotation = Quaternion.Euler(m_Pitch, 0f, 0.0f);
    }
}
