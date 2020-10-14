using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHandler : MonoBehaviour
{
    public float maxThrust;
    public float maxTorque;
    public Rigidbody2D rigidBody;

    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;

    public GameObject medium;
    public GameObject small;
    // only small and mediums spawned at run time

    public int asteroidPts;
    public GameObject player;
    public GameManager gameManager;

    // 3 large, 2 medium, 1 small
    public int asteroidSize;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        // add random amount of torque and thrust
        Vector2 force = new Vector2(Random.Range(-maxThrust, maxThrust), Random.Range(-maxThrust, maxThrust));
        float torque = Random.Range(-maxTorque, maxTorque);
        rigidBody.AddForce(force);
        rigidBody.AddTorque(torque);

        // reference to spaceship/player
        // findWithTag can be pretty slow sometimes, but needs to reference a specific instance in the hierarchy (not like drag + dropping prefab)
        player = GameObject.FindWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
        gameManager.levelPassed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // screen wrapping
        Vector2 position = transform.position;
        if (transform.position.y > screenTop)
        {
            position.y = screenBottom;
        }
        if (transform.position.y < screenBottom)
        {
            position.y = screenTop;
        }
        if (transform.position.x > screenRight)
        {
            position.x = screenLeft;
        }
        if (transform.position.x < screenLeft) 
        {
            position.x = screenRight;
        }
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if collision is with bullet
        if (collision.CompareTag("Bullet"))
        {
            // destroy bullet
            Destroy(collision.gameObject);

            // split up asteroid to smaller sizes
            if (asteroidSize == 3) {
                // 1 large -> 2 medium
                Instantiate(medium, transform.position, transform.rotation);
                Instantiate(medium, transform.position, transform.rotation);

                gameManager.UpdateAsteroidCount(1);
            } else if (asteroidSize == 2) {
                // 2 medium -> 2 small
                Instantiate(small, transform.position, transform.rotation);
                Instantiate(small, transform.position, transform.rotation);

                gameManager.UpdateAsteroidCount(1);
            } else if (asteroidSize == 1) {
                gameManager.UpdateAsteroidCount(-1);
            }

            // tells player to call ScorePoints
            player.SendMessage("ScorePoints", asteroidPts);

            GameObject expl = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(expl, 2);

            Destroy(gameObject);
        }
        
    }

}
