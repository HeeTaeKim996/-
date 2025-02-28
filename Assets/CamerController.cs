using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 initialiPosition;
    private Vector3 initialRotation;

    private void Awake()
    {
        initialiPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;
    }

    public void Rotate180()
    {
        transform.rotation = Quaternion.Euler(initialRotation + new Vector3(0, 0, 180));
    }

    public void RotateToInitial()
    {
        transform.rotation = Quaternion.Euler(initialRotation);
    }

}
