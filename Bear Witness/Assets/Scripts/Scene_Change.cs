using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Change : MonoBehaviour
{

    public string destination; 
    void Start()
    {
        SceneManager.LoadScene(destination);
    }

}
