using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movimiento de camara
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

    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;

    bool m_AngleLocked;
    bool m_AimLocked;

    //Movimiento
    [Header("Movment")]
    public bool isMoving;
    CharacterController m_CharacterController;
    public float m_Speed = 10.0f;
    public float m_SpeedMultiplier = 1.0f;
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_SprintKeyCode = KeyCode.LeftShift;


    //Gravedad
    [Header("Jump")]
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = false;
    [SerializeField]
    float m_JumpForçe;
    [SerializeField]
    float m_GravityForçe;

    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;

        //Movement
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        /*#if UNITY_EDITOR
            if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
                m_AngleLocked = !m_AngleLocked;
            if (Input.GetKeyDown(m_DebugLockKeyCode))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;
                m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
            }
        #endif*/

        float l_MouseAxisY = Input.GetAxis("Mouse Y");
        float l_MouseAxisX = Input.GetAxis("Mouse X");

        /*#if UNITY_EDITOR
            if(m_AngleLocked)
            {
                l_MouseAxisX=0.0f;
                l_MouseAxisY=0.0f;
            }
        #endif*/

        if (m_InvertedPitch == false) m_Pitch -= l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime;
        else m_Pitch += l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime;

        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        if (m_InvertedYaw == false) m_Yaw += l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime;
        else m_Yaw -= l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchControllerTransform.localRotation = Quaternion.Euler(m_Pitch, 0f, 0.0f);

        //Movement
        Vector3 l_Movement = Vector3.zero;

        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;
        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0.0f, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0.0f, Mathf.Cos(l_Yaw90InRadians));

        if (Input.GetKey(m_UpKeyCode)) l_Movement = l_Forward;
        else if (Input.GetKey(m_DownKeyCode)) l_Movement = -l_Forward;

        if (Input.GetKey(m_RightKeyCode)) l_Movement += l_Right;
        else if (Input.GetKey(m_LeftKeyCode)) l_Movement -= l_Right;

        if (Input.GetKey(m_SprintKeyCode)) m_SpeedMultiplier = 1.5f;
        else m_SpeedMultiplier = 1f;

        l_Movement.Normalize();
        l_Movement = l_Movement * Time.deltaTime * m_Speed * m_SpeedMultiplier;

        if (l_Movement != new Vector3(0f, 0f, 0f))
            isMoving = true;
        else
            isMoving = false;

        //Gravedad
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime * m_GravityForçe;
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);
        
        /*if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            m_OnGround = true;
            m_VerticalSpeed = 0.0f;
        }
        else m_OnGround = false;

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f) m_VerticalSpeed = 0.0f;
        if (Input.GetKey(m_JumpKeyCode) && m_OnGround) m_VerticalSpeed = m_JumpForçe;*/
    }
}