using UnityEngine;

public class BombController : MonoBehaviour
{
    public float speed = 0.005f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Create a new vector where we modify the x position
        // of our game object
        Vector2 pos = new Vector2(
            gameObject.transform.position.x,
            gameObject.transform.position.y + speed);

        // Assign new position vector to game object
        gameObject.transform.position = pos;

        if(gameObject.transform.position.y >= 10f)
        {
            Destroy(gameObject);
        }
    }
}
