using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{

    [SerializeField] private Gate.Gates gateName;
    public void SetDoorState(bool value)
    {
        GameManager.instance.doorStates[Gate.GateMatch[gateName]] = value;
    }

    public Gate.Gates GetGateName()
    {
        return gateName;
    }
}
