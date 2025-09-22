using UnityEngine;
using TMPro;

public class BirdController : MonoBehaviour
{
    public float speed = 0.5f;
    public TMP_Text scoreText;

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
            // gameObject.GetComponent<AudioSource>().Play();

            // Move bird to its initial position
            // gameObject.transform.position = initPosition;

            // On collision, subtract 25 points
            int score = int.Parse(scoreText.text);
            score = score - 25;
            scoreText.text = score.ToString();

            if(score <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Play the death scream
        gameObject.GetComponent<AudioSource>().Play();

        // Flip the bird upside down
        gameObject.GetComponent<SpriteRenderer>().flipY = true;

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}
