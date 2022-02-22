using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("Go to table")]
    public static PlayerManager instance;
    Vector3 _originalPos;
    Quaternion _originalRot;

    public GameObject Player;
    public GameObject CutSpot;

    public GameObject PlayerCamera;
    public GameObject CutSpotCamera;

    public float CameraAnimationTime;

    public CameraMovementCut CameraMovementCut;


    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        _originalPos = CutSpot.transform.position;
        _originalRot = CutSpot.transform.rotation;
    }


    public void GoToTable()
    {
        CutSpot.transform.position = PlayerCamera.transform.position;
        CutSpot.transform.rotation = PlayerCamera.transform.rotation;
       
        Player.SetActive(false);
        CutSpotCamera.SetActive(true);

        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        Vector3 _startPos = CutSpot.transform.position;
        Quaternion _startRot = CutSpot.transform.rotation;

        float elapsedTime = 0;

        while (elapsedTime <= CameraAnimationTime)
        {
            float ratio = elapsedTime / CameraAnimationTime;

            Vector3 posLerp = Vector3.Lerp(CutSpot.transform.position, _originalPos, ratio);
            Quaternion rotLerp = Quaternion.Lerp(CutSpot.transform.rotation, _originalRot, ratio);

            CutSpot.transform.SetPositionAndRotation(posLerp, rotLerp);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //CutSpot.transform.SetPositionAndRotation(_originalPos, _originalRot);
        CameraMovementCut.enabled = true;
    }
}
