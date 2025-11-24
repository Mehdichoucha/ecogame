using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime = 15f; // temps de survie pour gagner
    private float timer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timer = gameTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Victory();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over ! La tortue a mangé le plastique !");
        // Tu peux afficher un UI GameOver ici
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // restart
    }

    public void Victory()
    {
        Debug.Log("Victoire ! La tortue est sauvée !");
        // UI victoire
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // restart
    }
}
