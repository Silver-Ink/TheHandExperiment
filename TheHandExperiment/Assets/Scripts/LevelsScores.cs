using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public struct roundScore
{
    public float time;
    public int errors;
}

public class LevelsScores : MonoBehaviour
{
    [SerializeField]
    private GameObject textScore;

    private TMP_Text text;

    string filename = "";

    [System.Serializable]
    public class LevelScore : List<roundScore> { }

    // currentScore est réinitialisé à chaque début de partie
    public LevelScore currentScore = new LevelScore();

    [System.Serializable]
    public class PlayerList
    {
        public List<LevelScore> levelScore;
    }

    private void Start()
    {
        text = textScore.GetComponent<TMP_Text>();
    }

    public void InitializeTestData()
    {
        // Initialisation des données de test pour la première fois
        if (PlayerScoreManager.Instance.playerScore.levelScore.Count == 0)
        {
            PlayerScoreManager.Instance.playerScore.levelScore = new List<LevelScore>();

            LevelScore player1Level1 = new LevelScore
            {
                new roundScore { time = 12.5f, errors = 2 },
                new roundScore { time = 14.3f, errors = 1 }
            };

            LevelScore player1Level2 = new LevelScore
            {
                new roundScore { time = 13.2f, errors = 2 },
                new roundScore { time = 11.8f, errors = 1 }
            };

            PlayerScoreManager.Instance.playerScore.levelScore.Add(player1Level1);
            PlayerScoreManager.Instance.playerScore.levelScore.Add(player1Level2);
        }
    }

    public void UpdateScore(int round, int errors, float timer)
    {
        // Vérifie que l'index est valide
        if (round >= 0)
        {
            roundScore updatedScore;
            updatedScore.errors = errors;
            updatedScore.time = timer;
            currentScore.Add(updatedScore);
        }
        else
        {
            Debug.Log("Index invalide pour le score.");
        }
    }

    public void AddTotalScore()
    {
        int totalerrors = 0;
        float totalTime = 0f;

        foreach (var score in currentScore)
        {
            totalerrors += score.errors;
            totalTime += score.time;
        }

        roundScore totalScore = new roundScore
        {
            errors = totalerrors,
            time = totalTime
        };

        currentScore.Add(totalScore);

        // Ajoute ce score à la liste persistante dans PlayerScoreManager
        PlayerScoreManager.Instance.AddPlayerScore(currentScore);

        Debug.Log("Total errors: " + totalerrors + ", Total Time: " + totalTime);
    }

    public void DebugCurrentScore()
    {
        if (currentScore.Count == 0)
        {
            Debug.Log("La liste currentScore est vide.");
            return;
        }

        for (int i = 0; i < currentScore.Count; i++)
        {
            roundScore score = currentScore[i];
            Debug.Log("<color=green>Round " + i + ": errors = " + score.errors + ", Time = " + score.time + "</color>");
        }
    }

    public void DisplayScore()
    {
        string displayText = "Score\n";

        for (int i = 0; i < currentScore.Count - 1; i++)
        {
            string formattedValue = currentScore[i].time.ToString("F2");
            displayText += $"Round {i + 1} : {currentScore[i].errors} errors, {formattedValue} s\n";
        }

        string formattedTotal = currentScore[currentScore.Count].time.ToString("F2");
        displayText += $"Total : {currentScore[currentScore.Count].errors} errors, {formattedTotal} s";

        text.text = displayText;
        textScore.SetActive(true);
    }

    public void WriteCSV()
    {
        
        //InitializeTestData();
        filename = Application.dataPath + "/usersResults.csv";
        bool fileExists = File.Exists(filename);

        if (PlayerScoreManager.Instance.playerScore.levelScore.Count > 0)
        {
            Debug.Log("<color=green>Here 1 </color>");
            TextWriter tw = new StreamWriter(filename, true);
            if (!fileExists)
            {
                tw.WriteLine("Player; Level; Round; Score");
            }

            string firstcolumn = "";
            string secondcolumn = "";
            string thirdcolumn = "";

            firstcolumn = (PlayerScoreManager.Instance.playerNumber + 1) + ";";
            for (int j = 0; j < PlayerScoreManager.Instance.playerScore.levelScore.Count; j++)
            {
                Debug.Log("<color=green>Here 2 </color>");
                secondcolumn = firstcolumn + (j + 1) + ";";
                for (int t = 0; t < PlayerScoreManager.Instance.playerScore.levelScore[j].Count; t++)
                {
                    Debug.Log("<color=green>Here 3 </color>");
                    thirdcolumn = secondcolumn + (t + 1) + "; Time : " + PlayerScoreManager.Instance.playerScore.levelScore[j][t].time.ToString("F2");
                    tw.WriteLine(thirdcolumn);
                    tw.WriteLine(" ; ; ; Errors : " + PlayerScoreManager.Instance.playerScore.levelScore[j][t].errors);
                    secondcolumn = ";;";
                }
                firstcolumn = ";";
            }
            tw.Flush();
            tw.Close();
        }
    }
}
