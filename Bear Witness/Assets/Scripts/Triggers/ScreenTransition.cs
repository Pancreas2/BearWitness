using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public LoadingZone loadingZone;
    private string sceneDestination;

    void Start()
    {
        sceneDestination = loadingZone.room;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<PlayerMovement>().frozen = true;
            FindObjectOfType<LevelLoader>().LoadNextLevel(sceneDestination);
            Rigidbody2D rigidbody = collision.collider.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0f;
        }
    }
}
