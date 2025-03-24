using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelsScores : MonoBehaviour
{

    [SerializeField]
    private GameObject textScore;

    private TMP_Text text;

    public struct roundScore
    {
        public float time;
        public int errors;
    }

    /*public struct levelScore
    {
        List<roundScore> score;
    }*/

    private void Start()
    {
        text = textScore.GetComponent<TMP_Text>();
    }

    public List<roundScore> currentScore = new List<roundScore>();

    public void UpdateScore(int round, int errors, float timer)
    {
        // Vérifie que l'index est valide
        if (round >= 0)
        {
            roundScore updatedScore;
            updatedScore.errors = errors;  // Modification du nombre d'essais
            updatedScore.time = timer;     // Modification du temps
            currentScore.Add(updatedScore);
        }
        else
        {
            Debug.Log("Index invalide pour le score.");
        }
    }

    // Fonction pour obtenir le nombre de errors en fonction du round
    public int Geterrors(int round)
    {
        // Vérifie que l'index du round est valide
        if (round >= 0 && round < currentScore.Count)
        {
            return currentScore[round].errors; // Retourne le nombre de errors pour ce round
        }
        else
        {
            Debug.LogError("Round invalide !");
            return -1; // Retourne -1 si le round est invalide
        }
    }

    // Fonction pour obtenir le timer en fonction du round
    public float GetTime(int round)
    {
        // Vérifie que l'index du round est valide
        if (round >= 0 && round < currentScore.Count)
        {
            return currentScore[round].time; // Retourne le temps pour ce round
        }
        else
        {
            Debug.LogError("Round invalide !");
            return -1f; // Retourne -1f si le round est invalide
        }
    }

    public void AddTotalScore()
    {
        int totalerrors = 0;
        float totalTime = 0f;

        // Calcule les totaux pour tous les rounds
        foreach (var score in currentScore)
        {
            totalerrors += score.errors;  // Additionne les errors
            totalTime += score.time;      // Additionne les times
        }

        // Crée un nouveau roundScore avec les totaux
        roundScore totalScore = new roundScore
        {
            errors = totalerrors,  // Total des errors
            time = totalTime       // Total du time
        };

        // Ajoute le totalScore à la fin de la liste currentScore
        currentScore.Add(totalScore);

        // Affiche dans la console pour vérifier le résultat
        Debug.Log("Total errors: " + totalerrors + ", Total Time: " + totalTime);
    }

    public void DebugCurrentScore()
    {
        // Vérifie si la liste est vide
        if (currentScore.Count == 0)
        {
            Debug.Log("La liste currentScore est vide.");
            return;
        }

        // Parcours chaque élément de la liste et affiche ses valeurs dans la console
        for (int i = 0; i < currentScore.Count; i++)
        {
            roundScore score = currentScore[i];
            Debug.Log("<color=green>Round " + i + ": errors = " + score.errors + ", Time = " + score.time + "</color>");
        }
    }

    public void DisplayScore()
    {
        // Crée une chaîne pour afficher le score
        string displayText = "";
        float totalTime = 0f;
        int totalerrors = 0;

        // Parcourt la liste de scores et génère un texte pour chaque round
        for (int i = 0; i < currentScore.Count; i++)
        {
            // Accumule les totaux
            totalTime += currentScore[i].time;
            totalerrors += currentScore[i].errors;

            string formattedValue = currentScore[i].time.ToString("F2");

            // Génère le texte pour le round i (compte de 1 à n)
            displayText += $"Round {i + 1} : {currentScore[i].errors} errors, {formattedValue} s\n";
        }

        string formattedTotal = totalTime.ToString("F2");
        // Ajoute les totaux à la fin du texte
        displayText += $"Total : {totalerrors} errors, {formattedTotal} s";

        // Affecte le texte généré au composant TMP_Text pour l'affichage
        text.text = displayText;

        textScore.SetActive(true);
    }

    //public Dictionary<int, levelScore> userScore = new Dictionary<int, levelScore>();
}
