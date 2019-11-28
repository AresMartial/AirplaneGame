using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    [SerializeField] private Transform objToFollow;
    private void Update()
    {
        gameObject.transform.position = objToFollow.position;
        gameObject.transform.LookAt(objToFollow.position + transform.forward * 2);
    }
}
