using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuInterfaceController : InterfaceBehavior
{
    public Text RankingText;
    private ScoreRanking _scoreRanking;

    void Start()
    {
        _scoreRanking = GetComponent<ScoreRanking>();
        RankingText.text = "TOP 5:\n\n";
        var topFiveScores = _scoreRanking.LoadScoresTop(5);
        if (topFiveScores != null)
        {
            for (int i = 0; i < topFiveScores.Count; i++)
            {
                RankingText.text += $"{i + 1}° [{topFiveScores[i].Id}] " + topFiveScores[i].Name + " - " + topFiveScores[i].Score + " pts\n";
            }
        }
        else
        {
            RankingText.text += "------";
        }
    }
}
