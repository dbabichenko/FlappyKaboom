using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float speed = 0.5f;
    private Vector2 initPosition;

    void Start()
    {
        initPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Create a new vector where we modify the x position
            // of our game object
            Vector2 pos = new Vector2(
                gameObject.transform.position.x + speed,
                gameObject.transform.position.y);

            // Assign new position vector to game object
            gameObject.transform.position = pos;
                
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Create a new vector where we modify the x position
            // of our game object
            Vector2 pos = new Vector2(
                gameObject.transform.position.x - speed,
                gameObject.transform.position.y);

            // Assign new position vector to game object
            gameObject.transform.position = pos;

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // Create a new vector where we modify the x position
            // of our game object
            Vector2 pos = new Vector2(
                gameObject.transform.position.x,
                gameObject.transform.position.y + speed);

            // Assign new position vector to game object
            gameObject.transform.position = pos;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Create a new vector where we modify the x position
            // of our game object
            Vector2 pos = new Vector2(
                gameObject.transform.position.x,
                gameObject.transform.position.y - speed);

            // Assign new position vector to game object
            gameObject.transform.position = pos;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            // Destroy(gameObject);

            // Play the death scream
            gameObject.GetComponent<AudioSource>().Play();

            // Move bird to its initial position
            gameObject.transform.position = initPosition;
        }
    }
}
