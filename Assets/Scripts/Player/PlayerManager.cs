using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header ("Go to table")]
    public static PlayerManager instance;
    Vector3 _tablePos;
    Quaternion _tableRot;

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
        _tablePos = CutSpot.transform.position;
        _tableRot = CutSpot.transform.rotation;
    }


    public void GoToTable()
    {
        CutSpot.transform.position = PlayerCamera.transform.position;
        CutSpot.transform.rotation = PlayerCamera.transform.rotation;

        PlayerCamera.SetActive(false);
        Player.GetComponent<PlayerMovement>().enabled = false;
        CutSpotCamera.SetActive(true);

        StartCoroutine(MoveCameraToTable());
    }

    public void GoToPlayer()
    {
        CameraMovementCut.enabled = false;

        StartCoroutine(MoveCameraToPlayer());

    }

    IEnumerator MoveCameraToTable()
    {
        //Vector3 _startPos = CutSpot.transform.position;
        //Quaternion _startRot = CutSpot.transform.rotation;

        float elapsedTime = 0;

        while (elapsedTime <= CameraAnimationTime)
        {
            float ratio = elapsedTime / CameraAnimationTime;

            Vector3 posLerp = Vector3.Lerp(CutSpot.transform.position, _tablePos, ratio);
            Quaternion rotLerp = Quaternion.Lerp(CutSpot.transform.rotation, _tableRot, ratio);

            CutSpot.transform.SetPositionAndRotation(posLerp, rotLerp);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        CutSpot.transform.SetPositionAndRotation(_tablePos, _tableRot);
        CameraMovementCut.enabled = true;
    }

    IEnumerator MoveCameraToPlayer()
    {
        float elapsedTime = 0;

        while (elapsedTime <= CameraAnimationTime)
        {
            float ratio = elapsedTime / CameraAnimationTime;

            Vector3 posLerp = Vector3.Lerp(CutSpot.transform.position, Player.transform.position, ratio);
            Quaternion rotLerp = Quaternion.Lerp(CutSpot.transform.rotation, Player.transform.rotation, ratio);

            CutSpot.transform.SetPositionAndRotation(posLerp, rotLerp);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        CutSpot.transform.SetPositionAndRotation(Player.transform.position, Player.transform.rotation);
        PlayerCamera.SetActive(true);
        Player.GetComponent<PlayerMovement>().enabled = true;
        CutSpotCamera.SetActive(false);
    }
}
