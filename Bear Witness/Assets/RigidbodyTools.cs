using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTools : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody;

    public void AddNormalizedForce(float magnitude)
    {
        Vector2 force = new(magnitude, magnitude);
        rigidbody.AddForce(force);
    }
}
