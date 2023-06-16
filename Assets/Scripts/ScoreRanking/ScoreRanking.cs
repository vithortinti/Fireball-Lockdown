using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using FireballLockdownGame.ScoreRanking.Exceptions;

public class ScoreRanking : MonoBehaviour
{
    private string _file;
    private ScoreList _scoreList;

    // Start is called before the first frame update
    void Awake()
    {
        _file = GetScoreDirectory() + @"\rnkng.json";
        _scoreList = new ScoreList()
        {
            Scores = LoadScores() ?? new List<ScoreModel>()
        };
    }

    public void SaveScore(ScoreModel score)
    {
        score.Name = score.Name.Trim();
        if (!string.IsNullOrEmpty(score.Name))
        {
            RefreshScore(); // Verificar se houve alguma alteração no aquivo dos scores

            var containsTheId = 
                _scoreList.Scores.Find(x => x.Id == score.Id) != null && 
                (_scoreList.Scores.Count() > 0 || _scoreList.Scores.Count() != null);

            if (!containsTheId)
            {
                try
                {
                    _scoreList.Scores.Add(score);
                    Save();
                }
                catch { }
            }
            else
            {
                throw new SaveIdException($"O Id: {score.Id} já existe");
            }
        }
    }

    public void SaveScoreOverwriting(ScoreModel score, Predicate<ScoreModel> match)
    {
        RefreshScore();
        int index = _scoreList.Scores.FindIndex(match);
        _scoreList.Scores[index] = score;
        Save();
    }

    public List<ScoreModel> LoadScores()
    {
        byte[] jsonByte = File.ReadAllBytes(_file);
        string json = Encoding.UTF8.GetString(jsonByte);
        if (!string.IsNullOrEmpty(json))
        {
            string jsonDecrypted = Cryptografy.Decrypt(json);
            var scoreList = JsonUtility.FromJson<ScoreList>(jsonDecrypted);
            return scoreList.Scores;
        }
        return null;
    }

    public void RefreshScore()
    {
        _scoreList.Scores = LoadScores() ?? new List<ScoreModel>();
    }

    public List<ScoreModel> LoadScoresTop(int top)
    {
        var scores = LoadScores();
        if (scores != null)
        {
            scores.Sort((x1, x2) => x2.Score.CompareTo(x1.Score));
            return scores.Take(top).ToList();
        }
        return null;
    }

    private void Save()
    {
        string scoreJson = JsonUtility.ToJson(_scoreList);
        string encryptedScore = Cryptografy.Encrypt(scoreJson);
        byte[] scoreBytes = Encoding.UTF8.GetBytes(encryptedScore);
        File.WriteAllBytes(_file, scoreBytes);
    }

    public bool CheckPassword(string id, string password)
    {
        ScoreModel score = _scoreList.Scores.FirstOrDefault(x => x.Id == id);
        if (score != null)
        {
            return score.Password == password;
        }
        return false;
    }

    public bool UserExists(string id, string nickname)
    {
        bool userExists = _scoreList.Scores.Exists(x => x.Id == id && x.Name == nickname);
        if (userExists) return true;
        return false;
    }

    public int UserScore(string id)
    {
        return _scoreList.Scores.FirstOrDefault(x => x.Id == id).Score;
    }

    private string GetScoreDirectory()
    {
        string scoreDirectory = Directory.GetCurrentDirectory() + @"\LocalState\GameRnkng\scrs.txt";
        byte[] txtBytes = File.ReadAllBytes(scoreDirectory);
        string txtFileContent = Encoding.UTF8.GetString(txtBytes);
        if (txtFileContent == "local")
        {
            return Directory.GetCurrentDirectory() + @"\LocalState\GameRnkng";
        }
        return txtFileContent;
    }
}
