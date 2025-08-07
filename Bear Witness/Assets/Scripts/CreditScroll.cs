using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour
{
    [SerializeField] private float rate = 2f;
    [SerializeField] private float stopAtHeight = 1500f;

    void Awake()
    {
        if (GameManager.instance) Destroy(GameManager.instance.gameObject);
        if (AudioManager.instance) Destroy(AudioManager.instance.gameObject);
        if (GameUI_Controller.instance) Destroy(GameUI_Controller.instance.gameObject);
    }


    void Update()
    {
        float mult = 1f;
        if (Input.GetButton("Jump")) mult *= 2;
        if (Input.GetButton("Run")) mult *= 2;
        transform.localPosition = new(transform.localPosition.x, transform.localPosition.y + rate * mult, 0);

        if (transform.localPosition.y > stopAtHeight)
        {
            SceneManager.LoadScene("Start");
        }
    }
}
