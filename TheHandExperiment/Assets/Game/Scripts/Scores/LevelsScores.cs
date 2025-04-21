using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public struct roundScore
{
    public float time;  // Temps de la partie (en secondes)
    public int errors;  // Nombre d'erreurs
}

public class LevelsScores : MonoBehaviour
{
    [SerializeField]
    private GameObject textScore;  // Référence au texte qui affiche le score

    private TMP_Text text;  // Composant TMP_Text pour afficher le score

    [System.Serializable]
    public class LevelScore : List<roundScore> { }  // Liste des scores d'un niveau

    public LevelScore currentScore = new LevelScore();  // Liste des scores pour la session actuelle

    [System.Serializable]
    public class PlayerList
    {
        public List<LevelScore> levelScore;  // Liste des scores pour tous les niveaux
    }

    private void Start()
    {
        text = textScore.GetComponent<TMP_Text>();  // Initialisation du composant TMP_Text
    }

    // Initialise les données de test si elles sont vides
    public void InitializeTestData()
    {
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

    // Met à jour le score actuel pour une partie (round)
    public void UpdateScore(int round, int errors, float timer)
    {
        if (round >= 0)  // Vérifie que l'index est valide
        {
            roundScore updatedScore;
            updatedScore.errors = errors;
            updatedScore.time = timer;
            currentScore.Add(updatedScore);  // Ajoute le score à la liste
        }
        else
        {
            Debug.Log("Index invalide pour le score.");
        }
    }

    // Calcule et ajoute le score total pour tous les rounds de la session actuelle
    public void AddTotalScore()
    {
        int totalerrors = 0;
        float totalTime = 0f;

        // Calcule les erreurs et le temps total
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

        currentScore.Add(totalScore);  // Ajoute le score total à la liste

        // Ajoute ce score à la liste persistante du PlayerScoreManager
        PlayerScoreManager.Instance.AddPlayerScore(currentScore);

        Debug.Log("Total errors: " + totalerrors + ", Total Time: " + totalTime);
    }

    // Affiche le score actuel dans la console pour le débogage
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

    // Affiche le score actuel dans l'interface utilisateur
    public void DisplayScore()
    {
        Debug.Log("<color=blue>Display Score</color>");
        string displayText = "Score\n";

        // Affiche les scores de chaque round
        for (int i = 0; i < currentScore.Count - 1; i++)
        {
            string formattedValue = currentScore[i].time.ToString("F2");
            displayText += $"Round {i + 1} : {currentScore[i].errors} errors, {formattedValue} s\n";
        }

        // Affiche le score total
        string formattedTotal = currentScore[currentScore.Count - 1].time.ToString("F2");
        displayText += $"Total : {currentScore[currentScore.Count - 1].errors} errors, {formattedTotal} s";

        Debug.Log("<color=blue>Text : " + displayText + "</color>");
        text.text = displayText;  // Met à jour le texte affiché
        textScore.SetActive(true);  // Active l'affichage du texte
    }

    // Écrit les résultats dans un fichier CSV
    public static void WriteCSV()
    {
        string filename = Application.dataPath + "/Game/Results/usersResults.csv";
        bool fileExists = File.Exists(filename);  // Vérifie si le fichier existe déjà

        if (PlayerScoreManager.Instance.playerScore.levelScore.Count > 0)
        {
            TextWriter tw = new StreamWriter(filename, true);  // Ouvre le fichier en mode ajout
            if (!fileExists)
            {
                tw.WriteLine("Player; Difficulty; Level; Round; Score");  // En-tête du fichier CSV
            }

            string firstcolumn = "";
            string secondcolumn = "";
            string thirdcolumn = "";

            // Parcourt chaque niveau et écrit les résultats dans le CSV
            firstcolumn = PlayerScoreManager.Instance.playerName + ";" + PlayerScoreManager.Instance.GetStringDifficulty() + ";";
            for (int j = 0; j < PlayerScoreManager.Instance.playerScore.levelScore.Count; j++)
            {
                secondcolumn = firstcolumn + (j + 1) + ";";
                for (int t = 0; t < PlayerScoreManager.Instance.playerScore.levelScore[j].Count; t++)
                {
                    float timeInSeconds = PlayerScoreManager.Instance.playerScore.levelScore[j][t].time;
                    int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
                    float seconds = timeInSeconds % 60f;

                    // Formate le temps en minutes et secondes
                    string formattedTime;
                    if (minutes > 0)
                    {
                        formattedTime = string.Format("{0} min {1:00.00} s", minutes, seconds);
                    }
                    else
                    {
                        formattedTime = string.Format("{0:0.00} s", seconds);
                    }

                    if (t + 1 == 4)
                    {
                        thirdcolumn = secondcolumn + " Total; Time : " + formattedTime;
                    }
                    else
                    {
                        thirdcolumn = secondcolumn + (t + 1) + "; Time : " + formattedTime;
                    }

                    tw.WriteLine(thirdcolumn);  // Écrit la ligne du round dans le CSV
                    tw.WriteLine("; ; ; ; Errors : " + PlayerScoreManager.Instance.playerScore.levelScore[j][t].errors);  // Écrit les erreurs
                    secondcolumn = ";;;";
                }
                firstcolumn = "; ;";
            }
            tw.Flush();
            tw.Close();  // Ferme le fichier
        }
    }
}
