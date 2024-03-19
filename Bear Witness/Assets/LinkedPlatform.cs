using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D platformOne;
    [SerializeField] private Rigidbody2D platformTwo;

    private float balancePosition;

    private void Start()
    {
        balancePosition = platformOne.transform.position.y;
        Debug.Log(balancePosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
