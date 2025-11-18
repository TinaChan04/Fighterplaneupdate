using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //how to define a variable
    //1. access modifier: public or private
    //2. data type: int, float, bool, string
    //3. variable name: camelCase
    //4. value: optional
    public int lives;
    private int weaponType;
    private float playerSpeed;
    private float horizontalInput;
    private float verticalInput;
    
    public GameObject explosionPrefab;

    private float horizontalScreenLimit = 9.5f;
    private float verticalScreenLimit = 6.5f;

    public GameObject bulletPrefab;

    public GameObject BigBulletPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;
    private GameManager gameManager;

    
    


    void Start()
    { gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        playerSpeed = 6f;
        weaponType = 1;
        gameManager.ChangeLivesText(lives);
        if(thrusterPrefab != null)
        thrusterPrefab.SetActive(false);
        //This function is called at the start of the game

    }
    public void LoseALife()
    { 
        if(shieldPrefab.activeSelf)
        {
            shieldPrefab.SetActive(false);
            gameManager.ManagePowerupText(0);
            return;
        }

        lives--;  
        

        
        if(lives < 0)
            lives = 0;
         gameManager.ChangeLivesText(lives);
        if(lives <= 0)
        {
            
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

            gameManager.TriggerGameOver();
        }
    }

    IEnumerator SpeedPowerDown()
    {
        if (thrusterPrefab != null)
            thrusterPrefab.SetActive(true);
        yield return new WaitForSeconds(3f);
        playerSpeed = 6f;
        gameManager.ManagePowerupText(0);
        if(thrusterPrefab != null)
        thrusterPrefab.SetActive(false);
        gameManager.PlaySound(2);
    }

    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }

    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if(whatDidIHit.tag == "powerup")
        {
            Destroy(whatDidIHit.gameObject);
            int whichPowerup = Random.Range(1, 5);
            gameManager.PlaySound(1);
            switch (whichPowerup)
            {
            case 1:
                playerSpeed = 10f;
                thrusterPrefab.SetActive(true);
                StartCoroutine(SpeedPowerDown());
                gameManager.ManagePowerupText(1);
                break;
            case 2:
                weaponType = 2;
                StartCoroutine(WeaponPowerDown());
                gameManager.ManagePowerupText(2);
                gameManager.PlaySound(1);
                break;
            case 3: 
                weaponType = 3;
                StartCoroutine(WeaponPowerDown());
                gameManager.ManagePowerupText(3);
                break;
            case 4:
                shieldPrefab.SetActive(true);
                gameManager.ManagePowerupText(4);
                break;
            default:
                break;
            }
        }

        if(whatDidIHit.CompareTag("health"))
        {
            Destroy(whatDidIHit.gameObject);
            lives++;

            if(lives>3)

            lives=3;

            gameManager.ChangeLivesText(lives);
        }

    if(whatDidIHit.CompareTag("coin"))
        {
            Destroy(whatDidIHit.gameObject);
            

        

            gameManager.AddScore(1);
        }

    }
    

    
    
    


    

    void Update()
    {
        //This function is called every frame; 60 frames/second
        Movement();
        Shooting();

    }

    void Shooting()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        switch (weaponType)
        {
            case 1:
                Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;

            case 2:
                Instantiate(bulletPrefab, transform.position + new Vector3(-0.3f, 1f, 0), Quaternion.identity);
                Instantiate(bulletPrefab, transform.position + new Vector3( 0.3f, 1f, 0), Quaternion.identity);
                break;

            case 3:
                Instantiate(bulletPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                Instantiate(bulletPrefab, transform.position + new Vector3(-0.4f, 0.8f, 0), Quaternion.Euler(0, 0, 10));
                Instantiate(bulletPrefab, transform.position + new Vector3( 0.4f, 0.8f, 0), Quaternion.Euler(0, 0, -10));
                break;

            default:
                Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;
        }
    }
    if (Input.GetKeyDown(KeyCode.E))
    {
        Instantiate(BigBulletPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    }
}

    void Movement()
    {
        //Read the input from the player
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //Move the player
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * playerSpeed);
        //Player leaves the screen horizontally
        if(transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
        //Player leaves the screen vertically
        if(transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
        //Player limited bottom of screen
        if(transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        if(transform.position.y < -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, -verticalScreenLimit, 0);
        }
    
    }

}
