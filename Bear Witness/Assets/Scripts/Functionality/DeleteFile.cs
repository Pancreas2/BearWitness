using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleteFile : MonoBehaviour
{
    [SerializeField] private List<SaveFile> files = new();
    private bool deletionState = false;
    [SerializeField] private Animator animator;

    public void SetAllowDeletion(bool value)
    {
        foreach (SaveFile file in files)
        {
            file.SetAllowDeletion(value);
        }
    }

    public void OnClick()
    {
        deletionState = !deletionState;
        SetAllowDeletion(deletionState);
        animator.SetBool("CanDelete", deletionState);
    }
}
