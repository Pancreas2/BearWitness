using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArktisExitDoor : Door
{
    public override void ChangeRooms()
    {
        base.ChangeRooms();
        GameManager.instance.StartRun();
    }
}
