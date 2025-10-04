using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenScript : MonoBehaviour
{
    [SerializeField] private Text finalScoreText;

    private void OnEnable()
    {
        int score = CameraFollowScript.CurrentScore;
        HighScores.TryAdd(score);
        finalScoreText.text = "score: " + score;
    }
}
