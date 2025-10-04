using System.Collections.Generic;
using UnityEngine;

public static class HighScores
{
    private const int Max = 5;
    private const string KeyPrefix = "HighScore_";
    private static List<int> _scores;

    static HighScores() => Load();

    public static IReadOnlyList<int> Scores => _scores;

    public static bool TryAdd(int newScore)
    {
        _scores.Add(newScore);
        _scores.Sort((a, b) => b.CompareTo(a));  // descending
        bool isInTop5 = _scores.IndexOf(newScore) < Max;

        if (_scores.Count > Max)
            _scores.RemoveRange(Max, _scores.Count - Max);

        Save();
        return isInTop5;
    }

    public static void ClearAll()
    {
        for (int i = 0; i < Max; i++)
            PlayerPrefs.DeleteKey(KeyPrefix + i);
        _scores = new List<int>(Max);
        Save();
    }

    private static void Load()
    {
        _scores = new List<int>(Max);
        for (int i = 0; i < Max; i++)
            _scores.Add(PlayerPrefs.GetInt(KeyPrefix + i, 0));
        _scores.Sort((a, b) => b.CompareTo(a));
        if (_scores.Count > Max)
            _scores.RemoveRange(Max, _scores.Count - Max);
    }

    private static void Save()
    {
        for (int i = 0; i < Max; i++)
        {
            int val = i < _scores.Count ? _scores[i] : 0;
            PlayerPrefs.SetInt(KeyPrefix + i, val);
        }
        PlayerPrefs.Save();
    }
}
