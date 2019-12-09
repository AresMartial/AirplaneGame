using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public TextMesh[] messages;
    private TextMesh helloMessage;
    private TextMesh bonusMessage;
    private TextMesh congratsMessage;
    [SerializeField] private Airplane airplane; 
    void Start()
    {
        messages = GetComponentsInChildren<TextMesh>();
        helloMessage = messages[0];
        bonusMessage = messages[1];
        congratsMessage = messages[2];
        bonusMessage.gameObject.SetActive(false);
        congratsMessage.gameObject.SetActive(false);
        if (airplane == null)
        {
            airplane = FindObjectOfType<Airplane>();
        }
        if (airplane == null)
        {
            return;
        }

    }
    void Update()
    {
        if (Airplane.bonusPicked)
        {
            if (bonusMessage != null)
            {
                bonusMessage.gameObject.SetActive(true);
            }
            Destroy(bonusMessage, 5f);
        }
        if (Airplane.landed)
        {

            congratsMessage.gameObject.SetActive(true);
        }
        else
        {
            congratsMessage.gameObject.SetActive(false);
        }
        if (Airplane.engenOn)
        {
            Destroy(helloMessage);

        }
    }
}
