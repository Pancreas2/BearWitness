using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Change : MonoBehaviour
{

    public string destination;

    public void ChangeScene()
    {
        if (SceneManager.GetActiveScene().name == "The_Twixt") GameManager.instance.ExitTwixt();
        FindObjectOfType<LevelLoader>().LoadNextLevel(destination);
    }

}
