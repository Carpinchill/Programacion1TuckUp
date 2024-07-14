using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartelUI : MonoBehaviour
{
    public GameObject signUI;
    public TextMeshProUGUI signText;

    public string displayText;
    private bool isMessageComplete = false;

    private Movement movement;
    private Attack attack;

    void Start()
    {
        signUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            movement = other.GetComponent<Movement>();
            attack = other.GetComponent<Attack>();
            ActivateSign();
        }
    }

    void ActivateSign()
    {
        signUI.SetActive(true);
        if (movement != null) movement.enabled = false;
        if (attack != null) attack.enabled = false;
        StartCoroutine(TypeText());
    }

    void DeactivateSign()
    {
        signUI.SetActive(false);
        if (movement != null) movement.enabled = true;
        if (attack != null) attack.enabled = true;
        signText.text = "";
        isMessageComplete = false;
    }

    IEnumerator TypeText()
    {
        signText.text = "";
        foreach (char letter in displayText.ToCharArray())
        {
            signText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        isMessageComplete = true;
    }

    void Update()
    {
        if (signUI.activeSelf && Input.GetKeyDown(KeyCode.F) && isMessageComplete)
        {
            DeactivateSign();
        }
    }
}
