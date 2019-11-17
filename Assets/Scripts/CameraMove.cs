using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform objToFollow;
    [SerializeField] private float forward = 10f;
    [SerializeField] private float up = 5f;
    [SerializeField] private float lookForward = 30f;
    [SerializeField] private float bias = 0.80f;
    void FixedUpdate()
    {
        Vector3 moveCamTo = objToFollow.position - objToFollow.forward * forward + objToFollow.up * up;
        
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1 - bias);
        Camera.main.transform.LookAt(objToFollow.position + transform.forward * lookForward);
    }
}
