using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckBadgeEquipped : MonoBehaviour
{
    [SerializeField] string badgeName;

    public UnityEvent OnBadgeEquipped;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.currentBadges.Contains(badgeName))
        {
            OnBadgeEquipped.Invoke();
        }
    }

    public void Windbreaker(AreaEffector2D effector)
    {
        effector.forceMagnitude *= 0.5f;
    }
}
