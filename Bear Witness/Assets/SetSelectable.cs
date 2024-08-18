using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectable : MonoBehaviour
{
    [SerializeField] private GameObject targetSelectable;

    public void SelectSelectable()
    {
        EventSystem.current.SetSelectedGameObject(targetSelectable);
    }
}
