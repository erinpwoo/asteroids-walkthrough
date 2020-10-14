using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for accessing Text object
using UnityEngine.SceneManagement; // loading/unloading of scenes

public class Spaceship : MonoBehaviour
{
    private float posX;
    private float posY;
    private float posZ; // rotation


    // screen bounnds
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;

    // critical damage
    public float deathForce;

    public GameObject bullet;
    public GameObject explosion;

    public float torque;
    private float torqueInput;
    public float thrust;
    private float thrustInput;
    public float bulletForce = 40;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;

    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().position.Set(0, 0);
        score = 0;

        gameOverPanel.SetActive(false);

        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject clone = Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
            clone.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(clone, 5);
        }

        thrustInput = Input.GetAxis("Vertical");
        torqueInput = Input.GetAxis("Horizontal");

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

    void ScorePoints(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > deathForce)
        {
            GameObject shipExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(shipExplosion, 3);
            lives--;
            livesText.text = "Lives: " + lives;

            // Respawn (turn off colliders and sprite renderers)
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Respawn", 3); // invoke Respawn() after 3 seconds

            if (lives <= 0)
            {
                // Game Over
                GameOver();
            }
        }
    }

    void GameOver()
    {
        CancelInvoke(); // cancels pending invoke methods (respawn)
        gameOverPanel.SetActive(true);
    }

    // reloading new game
    public void PlayAgain()
    {
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("Main Scene");
    }

    void Respawn()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // zero out velocity
        transform.position = Vector2.zero; // recentering position at origin
        
        // Respawn (turn off colliders and sprite renderers)
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

    }
 
    public float getX() => posX;

    public float getY() => posY;

    public float getAngle() => posZ;

    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * thrustInput);
        gameObject.GetComponent<Rigidbody2D>().AddTorque(-torqueInput);
    }
}
