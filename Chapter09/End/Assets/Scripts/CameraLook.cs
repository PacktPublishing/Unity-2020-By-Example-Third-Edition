using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float LookSpeed = 3f;
    public Transform Body;
    public Camera LookCamera;

    private Vector2 Rotation = Vector2.zero;

    void Update()
    {
        Rotation.y += Input.GetAxis("Mouse X");
        Rotation.x -= Input.GetAxis("Mouse Y");
        Vector2 RotThisStep = Rotation * LookSpeed;
        Body.eulerAngles = new Vector2(0, RotThisStep.y);
        LookCamera.transform.localRotation = Quaternion.Euler(RotThisStep.x, 0, 0);
    }
}
