using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public float verticalScreenSize = 5f;
    public float horizontalScreenSize = 6f;
    public int score;
    public int cloudMove;
    public GameObject enemyOnePrefab;
    public GameObject Enemy2Prefab;
    public GameObject Enemy3Prefab;
    public GameObject cloudPrefab;
    public GameObject GameOverText;
    public GameObject restartText;
    public GameObject powerupPrefab;
    public GameObject audioPlayer;
    public GameObject healthPrefab;
    public AudioClip powerUpSound;
    public AudioClip powerDownSound;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI PowerupText;
    

    public bool IsGameOver;

    // Start is called before the first frame update
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        CreateSky();
        IsGameOver = false;
        AddScore(0);
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        StartCoroutine(SpawnPowerup());
        StartCoroutine(SpawnHealth());
        InvokeRepeating("CreateEnemyOne", 1, 2);
        InvokeRepeating("CreateEnemy2", 4, 6);
        InvokeRepeating("CreateEnemy3", 2, 4);
        PowerupText.text = "No Powers yet!";
      
        
    }

IEnumerator SpawnPowerup()
{
    float spawnTime = Random.Range(3,5);
    yield return new WaitForSeconds(spawnTime);
    CreatePowerup();
    StartCoroutine(SpawnPowerup());
}
void CreatePowerup()
{
    Instantiate(powerupPrefab, new Vector3(Random.Range(-6f, 8f), Random.Range (-6f, 0f), 0), Quaternion.identity); 
}
public void ManagePowerupText(int powerupType)
{
    switch(powerupType)
    {
        case 1:
            PowerupText.text = "Speed Boost!";
            break;
        case 2: 
            PowerupText.text = "Double shot!";
            break;
        case 3:
            PowerupText.text = "Triple Shot!";
            break;
        case 4:
            PowerupText.text = "Shield Activated!";
            break;
        default:
            PowerupText.text = "No powerups yet!";
            break;
        
    }
}

IEnumerator SpawnHealth()
{
    float spawnTime = Random.Range(6,8);
    yield return new WaitForSeconds(spawnTime);
    CreateHealth();
    StartCoroutine(SpawnHealth());
}

void CreateHealth()
{
    Instantiate(healthPrefab, new Vector3(Random.Range(-6,8f), Random.Range (-6,0f), 0), Quaternion.identity);
}

public void PlaySound(int soundType)
{
    
    switch (soundType)
    {
        case 1: 
            audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerUpSound);
            break;
        case 2:
            audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerDownSound);
            break;
    }
}
    // Update is called once per frame

     void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
    }
 void Update()
    {
       if (IsGameOver && Input.GetKeyDown(KeyCode.R))
       {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       }
    }
    void CreateEnemyOne()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-9f, 9f), 6.5f, 0), Quaternion.identity);
    }
   void CreateEnemy2()
    {
        Instantiate(Enemy2Prefab, new Vector3(Random.Range(-9f, 9f), 6.5f, 0), Quaternion.identity);
    }
    void CreateEnemy3()
    {
        Instantiate(Enemy3Prefab, new Vector3(Random.Range(-9f, 9f), 6.5f, 0), Quaternion.identity);
    }

    public void AddScore(int earnedScore)
    {
        score += earnedScore;
        scoreText.text = "Score: " + score;
        //score = score + pointsToAdd;
        //
    }
    public void ChangeLivesText(int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }
    public void TriggerGameOver()
    {
        GameOverText.SetActive(true);
        restartText.SetActive(true);
        IsGameOver = true;
        CancelInvoke();
        cloudMove = 0;
    }
}
