using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public int gateID;
    [SerializeField] private float openingSize = 1f;
    private float closedPoint = 0f;
    private GameManager gameManager;
    private bool isOpen;
    [SerializeField] private bool forceOpen;
    [SerializeField] private float gateRate = 0.03f;

    private void Start()
    {
        closedPoint = transform.localPosition.y;
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.doorStates[gateID] || forceOpen)
        {
            isOpen = true;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + openingSize);
        }
    }

    private void Update()
    {
        if (gameManager.doorStates[gateID] || forceOpen)
        {
            if (!isOpen)
            {
                StopAllCoroutines();
                StartCoroutine(OpenGate());
            }
            isOpen = true;
        }
        else
        {
            if (isOpen)
            {
                StopAllCoroutines();
                StartCoroutine(CloseGate());
            }
            isOpen = false;
        }
    }

    public void SetForceOpen(bool value)
    {
        forceOpen = value;
    }

    IEnumerator OpenGate()
    {
        float targetY = closedPoint + openingSize - gateRate;
        while (transform.localPosition.y < targetY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + gateRate);
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = new Vector3(transform.localPosition.x, closedPoint + openingSize);
    }

    IEnumerator CloseGate()
    {
        float targetY = closedPoint + gateRate;
        while (transform.localPosition.y > targetY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - gateRate);
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = new Vector3(transform.localPosition.x, closedPoint);
    }
}
