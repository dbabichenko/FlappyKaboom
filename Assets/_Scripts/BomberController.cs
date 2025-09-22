using UnityEngine;

public class BomberController : MonoBehaviour
{
    public float speed = 0.5f;
    public GameObject bombPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            // Create a new vector where we modify the x position
            // of our game object
            Vector2 pos = new Vector2(
                gameObject.transform.position.x + speed,
                gameObject.transform.position.y);

            // Assign new position vector to game object
            gameObject.transform.position = pos;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Create a new vector where we modify the x position
            // of our game object
            Vector2 pos = new Vector2(
                gameObject.transform.position.x - speed,
                gameObject.transform.position.y);

            // Assign new position vector to game object
            gameObject.transform.position = pos;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bombPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }

        
       

    }
}
