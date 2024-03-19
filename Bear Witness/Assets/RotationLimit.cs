using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLimit : MonoBehaviour
{
    [SerializeField] private float limit;

    // Update is called once per frame
    void FixedUpdate()
    {
        float newRotation = Mathf.Clamp(transform.rotation.eulerAngles.z, -limit, limit);
        
        transform.rotation.eulerAngles.Set(0f, 0f, newRotation);

        Debug.Log(newRotation == transform.rotation.eulerAngles.z);
    }
}
