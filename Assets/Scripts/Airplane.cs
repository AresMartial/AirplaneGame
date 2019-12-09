using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class Airplane : MonoBehaviour
{
    [SerializeField] private float limitMS = 6.0f;
    [SerializeField] private float rollTorque = 6.0f;
    [SerializeField] private float pitchTorque = 6.0f;
    [SerializeField] private float virage = 6.0f;
    [SerializeField] private GameObject prop;
    [SerializeField] private GameObject propBlured;
    [SerializeField] private GameObject runway;
    private Rigidbody r;
    private bool landing = false;
    private bool destroyed = false;
    private float ms;
    public static bool engenOn = false;
    public static bool bonusPicked = false;
    public static bool landed = false;
    public static bool onRunway = true;
    public System.Action OnEngenOn;


    void Start()
    {
        r = GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider other)
    {
        string tagTrigger = other.gameObject.tag;
        switch (tagTrigger)
        {
            case "bonus":
                Destroy(other.gameObject);
                bonusPicked = true;
                break;
            case "landing plane":
                engenOn = false;
                landing = true;
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
                landed = true;
                break;
            case "runway":
                break;
            default:
                r.AddExplosionForce(999f, transform.position, 5.0f, 5.0f);
                Destroy(gameObject, 0.2f);
                destroyed = true;
                break;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            engenOn = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            FinishLine.LoadNextScene();
            engenOn = false;
            landed = false;
        }
    }
    void FixedUpdate()
    {
        if (!destroyed)
        {
            Cursor.visible = false;
            float roll = Input.GetAxis("Mouse X") * Time.deltaTime;
            float pitch = Input.GetAxis("Mouse Y") * Time.deltaTime;
            if (engenOn)
            {
                Destroy(runway);
                landed = false;
                prop.SetActive(false);
                propBlured.SetActive(true);
                propBlured.transform.Rotate(1000 * Time.deltaTime, 0, 0);

                r.useGravity = false;
                ms -= transform.forward.y * Time.deltaTime * 5.0f;
                r.AddRelativeForce(0, 0, Mathf.Clamp(ms, limitMS - 100, limitMS));
                r.AddRelativeTorque(Vector3.back * Mathf.Clamp(roll, -0.9f, 0.9f) * rollTorque * Time.deltaTime, ForceMode.Force);
                r.AddRelativeTorque(Vector3.right * Mathf.Clamp(pitch, -0.1f, 0.1f) * pitchTorque * Time.deltaTime, ForceMode.Force);


                var projection = -gameObject.transform.InverseTransformDirection(Vector3.up);
                //if (projection.x < 0)
                //{

                //    r.AddRelativeTorque(-gameObject.transform.up * projection.x * limitMS * virage * Time.deltaTime, ForceMode.Force);
                //}
                //else 
                //{
                //    r.AddRelativeTorque(-gameObject.transform.up * projection.x * limitMS * virage * Time.deltaTime, ForceMode.Force);
                //}

                r.AddTorque(0, projection.x * limitMS * virage * Time.deltaTime, 0, ForceMode.Force);

                if (bonusPicked)
                {
                    var controlMS = Input.mouseScrollDelta;
                    if ((limitMS - 100) < 50 && controlMS.y < 0)
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
                if (landing)
                {
                    r.AddRelativeForce(0, 0, (limitMS - 100) / 50);
                    r.AddRelativeTorque(Vector3.back * Mathf.Clamp(roll, -0.09f, 0.9f) * rollTorque * Time.deltaTime, ForceMode.Force);
                    r.AddRelativeTorque(Vector3.right * Mathf.Clamp(pitch, -0.01f, 0.1f) * pitchTorque * Time.deltaTime, ForceMode.Force);
                }                
            }
        }
        else
        {
            Cursor.visible = true;
            FinishLine.LoadNextScene();
            bonusPicked = false;
            engenOn = false;
            landed = false;
        }
        
    }
}
