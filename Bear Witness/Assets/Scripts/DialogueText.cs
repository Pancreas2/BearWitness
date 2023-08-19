using UnityEngine;
using TMPro;

public class DialogueText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void Display()
    {
        text.enabled = true;
    }
}
