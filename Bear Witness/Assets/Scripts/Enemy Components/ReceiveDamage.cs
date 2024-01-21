using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] private ReceiveDamage passDamageTo;

    public virtual void Damage(int damage, float sourceX)
    {
        if (passDamageTo) passDamageTo.Damage(damage, sourceX);
    }
}
