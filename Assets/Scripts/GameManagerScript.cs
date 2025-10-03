using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
