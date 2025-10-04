using UnityEngine;
using TMPro;

public class ScoreboardPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] lines;

    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        var scores = HighScores.Scores;
        for (int i = 0; i < lines.Length; i++)
        {
            int val = i < scores.Count ? scores[i] : 0;
            lines[i].text = $"{i + 1}. {val}";
        }
    }
}
