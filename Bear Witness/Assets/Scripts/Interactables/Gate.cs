using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Gates gateName;
    private int gateID;
    [SerializeField] private float openingSize = 1f;
    private float closedPoint = 0f;
    private GameManager gameManager;
    private bool isOpen;
    [SerializeField] private bool forceOpen;
    [SerializeField] private bool forceClose;
    [SerializeField] private float gateRate = 0.03f;

    public enum Gates
    {
        Lighthouse,
        LighthouseAirship,
        ArmouredMollusk,
        ShoresLedge,
        BoilerRoom,
        CrabArena,
        Library,
        ShoresLighthouseShortcut,
        CrabShortcut,
        CircleSigilDoor,
        CircleSigil,
        None,
        ShoresNexusShortcut
    }

    public static readonly Dictionary<Gates, int> GateMatch = new Dictionary<Gates, int>
    {
        { Gates.Lighthouse, 1 },
        { Gates.ShoresLedge, 2 },
        { Gates.BoilerRoom, 3 },
        { Gates.CrabArena, 4 },
        { Gates.ArmouredMollusk, 5 },
        { Gates.LighthouseAirship, 6 },
        { Gates.ShoresLighthouseShortcut, 7 },
        { Gates.CrabShortcut, 8 },
        { Gates.CircleSigilDoor, 9 },
        { Gates.CircleSigil, 10 },
        { Gates.ShoresNexusShortcut, 11}
    };


    private void Start()
    {
        if (gateName != Gates.None)
        {
            gateID = GateMatch[gateName];

            closedPoint = transform.localPosition.y;
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager.doorStates[gateID] || forceOpen)
            {
                isOpen = true;
                transform.localPosition = new Vector3(transform.localPosition.x, closedPoint + openingSize);
            }

            if (!gameManager.doorStates[gateID] || forceClose)
            {
                isOpen = false;
                transform.localPosition = new Vector3(transform.localPosition.x, closedPoint);
            }
        }
    }

    private void Update()
    {
        if (gateName != Gates.None)
        {
            if ((gameManager.doorStates[gateID] || forceOpen) && !forceClose)
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
    }

    public void SetForceOpen(bool value)
    {
        forceOpen = value;
    }

    public void SetForceClose(bool value)
    {
        forceClose = value;
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
