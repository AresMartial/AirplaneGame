using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveAndRotate : MonoBehaviour
{
    [SerializeField] private float limitMS = 6.0f;
    [SerializeField] private float rollTorque = 6.0f;
    [SerializeField] private float pitchTorque = 6.0f;
    [SerializeField] private GameObject prop;
    [SerializeField] private GameObject propBlured;
    private Rigidbody r;
    private float ms;
    private bool engenOn = false;
    private bool bonusPicked = false;
    private TextMesh[] messages;
    private TextMesh helloMessage;
    private TextMesh bonusMessage;
    private TextMesh congratsMessage;
    private TextMesh objMessage;
    void Start()
    {
        r = GetComponent<Rigidbody>();
        messages = GetComponentsInChildren<TextMesh>();
        helloMessage = messages[0];
        bonusMessage = messages[1];
        congratsMessage = messages[2];
        bonusMessage.gameObject.SetActive(false);
        congratsMessage.gameObject.SetActive(false);
        TextMesh objMessage = Camera.main.GetComponentInChildren<TextMesh>();
        objMessage.gameObject.SetActive(true);        
    }
    void OnTriggerEnter(Collider other)
    {
        string tagTrigger = other.gameObject.tag;
        switch (tagTrigger)
        {
            case "bonus":
                Destroy(other.gameObject);
                bonusPicked = true;
                bonusMessage.gameObject.SetActive(true);
                Destroy(bonusMessage, 5f);
                break;
            case "landing plane":
                engenOn = false;
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        string tagCollision = collision.gameObject.tag;
        
        switch (tagCollision)
        {
            case "landing plane":
                engenOn = false;
                Destroy(objMessage);
                congratsMessage.gameObject.SetActive(true);
                break;
            case "runway":

                break;
            default:
                r.AddExplosionForce(999f, transform.position, 5.0f, 5.0f);
                Destroy(gameObject, 0.2f);
                break;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            engenOn = true;
        }
    }
    void FixedUpdate()
    {
        
        float roll = Input.GetAxis("Mouse X") * Time.deltaTime;
        float pitch = Input.GetAxis("Mouse Y") * Time.deltaTime;
        if (engenOn)
        {
            Destroy(helloMessage);
            prop.SetActive(false);
            propBlured.SetActive(true);
            propBlured.transform.Rotate(1000 * Time.deltaTime, 0, 0);

            r.useGravity = false;
            ms -= transform.forward.y * Time.deltaTime * 5.0f;
            r.AddRelativeForce(0, 0, Mathf.Clamp(ms, limitMS-100, limitMS));
            r.AddRelativeTorque(Vector3.back * Mathf.Clamp(roll, -0.9f, 0.9f) * rollTorque * Time.deltaTime, ForceMode.Force);
            r.AddRelativeTorque(Vector3.right * Mathf.Clamp(pitch, -0.1f, 0.1f) * pitchTorque * Time.deltaTime, ForceMode.Force);
            if (bonusPicked)
            {
                var controlMS = Input.mouseScrollDelta;
                if ((limitMS-100) < 50 && controlMS.y < 0)
                {
                    limitMS -= controlMS.y;
                }
                else if (limitMS > 500 && controlMS.y > 0)
                {
                    limitMS -= controlMS.y;
                }
                else
                {
                    limitMS += controlMS.y;
                }
            }
        }
        else
        {
            prop.SetActive(true);
            propBlured.SetActive(false);
            r.useGravity = true;
            r.AddRelativeForce(0, 0, (limitMS-100)/50);
            r.AddRelativeTorque(Vector3.back * Mathf.Clamp(roll, -0.09f, 0.9f) * rollTorque * Time.deltaTime, ForceMode.Force);
            r.AddRelativeTorque(Vector3.right * Mathf.Clamp(pitch, -0.01f, 0.1f) * pitchTorque * Time.deltaTime, ForceMode.Force);
        }
    }
}
