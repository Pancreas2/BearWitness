using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFile : MonoBehaviour
{
    public void Delete(int slot)
    {
        SaveSystem.DeleteSave(slot);
    }

    public void DeleteAll()
    {
        SaveSystem.DeleteSave(0);
        SaveSystem.DeleteSave(1);
        SaveSystem.DeleteSave(2);
        SaveSystem.DeleteSave(3);
    }
}
