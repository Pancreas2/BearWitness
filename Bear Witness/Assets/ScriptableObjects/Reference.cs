using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reference", menuName = "Reference")]
[System.Serializable]
public class Reference : ScriptableObject
{
    public List<string> reference;
}
