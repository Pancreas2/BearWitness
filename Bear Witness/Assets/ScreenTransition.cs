using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public string sceneDestination;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            FindObjectOfType<LevelLoader>().LoadNextLevel(sceneDestination);
        }
    }
}
