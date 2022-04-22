using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    /*LAB 7 STEP 2 Create a member variable of the GameManager class, for indicating whether the game is in Menu or
    Playing state
    I don't need it since I chance scenes using scene manager with build index
    */
    public static GameManagerScript Instance;
    private int _currentGameLevel = 1;
    private static int Score = 0;
    private static int BestScore = 0;
    private static int Lives  = 3;

    public GameObject prefabAsteroid;

    public GameObject spaceshipPrefab;
    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
        Camera.main.transform.position = new Vector3(0, 30, 0);
        Camera.main.transform.LookAt(Vector3.zero, Vector3.up);

        StartNewGame();
    }

    private void StartNewGame()
    {
        if (Score > BestScore)
        {
            BestScore = Score;
        }

        Score = 0;
        _currentGameLevel++;
        for (int i = 0; i < _currentGameLevel * 10; i++)
        {
            GameObject asteroid = Instantiate(prefabAsteroid);
        }
        CreatePlayerSpaceship();
    }

    public void CreatePlayerSpaceship()
    {
        var spaceship = Instantiate(spaceshipPrefab);
        spaceship.transform.position = Vector3.zero;
        ReceiveDamage();
    }

    public static void AddScore(int asteroidScore)
    {
        Score += asteroidScore;
    }

    private static void ReceiveDamage()
    {
        Lives--;
    }

    private void Update()
    {
        if (Lives <= 0)
        { 
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) { // destroy all asteroids, bullets etc.
                Destroy(o);
            }
            SceneManager.LoadScene("MainMenuScene"); // Back to menu
        }
    }

    public static int GetScore()
    {
        return Score;
    }

    public static int GetBest()
    {
        return BestScore;
    }

    public static int GetLives()
    {
        return Lives;
    }
}
