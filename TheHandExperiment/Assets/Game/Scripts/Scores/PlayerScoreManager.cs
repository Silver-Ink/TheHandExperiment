using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance { get; private set; }

    // Liste contenant les scores des joueurs
    public LevelsScores.PlayerList playerScore = new LevelsScores.PlayerList();

    public string playerName = "";

    public DifficultyType difficulty = DifficultyType.Normal;

    public enum DifficultyType
    {
        Easy, 
        Normal,
        Intermediate,
        Hard,
        Expert
    }


    private void Awake()
    {
        // Singleton pour s'assurer qu'il n'y ait qu'une seule instance de PlayerScoreManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Assure que l'objet persiste entre les scènes
        }
    }

    // Méthode pour ajouter un score pour un joueur
    public void AddPlayerScore(LevelsScores.LevelScore score)
    {
        playerScore.levelScore.Add(score);
    }

    public string GetPlayerNumber()
    {
        return playerName;
    }

    // Méthode pour réinitialiser les scores des joueurs si nécessaire
    public void ResetPlayerScores()
    {
        playerScore.levelScore.Clear();
    }

    void OnApplicationQuit()
    {
        LevelsScores.WriteCSV();
    }

    public void AddDifficulty()
    {
        switch(difficulty)
        {
            case DifficultyType.Easy:
                difficulty = DifficultyType.Normal;
                break;
            case DifficultyType.Normal:
                difficulty = DifficultyType.Intermediate;
                break;
            case DifficultyType.Intermediate:
                difficulty = DifficultyType.Hard;
                break;
            case DifficultyType.Hard:
                difficulty = DifficultyType.Expert;
                break;
        }
    }

    public void SubstractDifficulty()
    {
        switch (difficulty)
        {
            case DifficultyType.Expert:
                difficulty = DifficultyType.Hard;
                break;
            case DifficultyType.Normal:
                difficulty = DifficultyType.Easy;
                break;
            case DifficultyType.Intermediate:
                difficulty = DifficultyType.Normal;
                break;
            case DifficultyType.Hard:
                difficulty = DifficultyType.Intermediate;
                break;
        }
    }

    public string GetStringDifficulty()
    {
        string res = "";
        switch (difficulty)
        {
            case DifficultyType.Expert:
                res = "Expert";
                break;
            case DifficultyType.Normal:
                res = "Normal";
                break;
            case DifficultyType.Intermediate:
                res = "Intermediate";
                break;
            case DifficultyType.Hard:
                res = "Hard";
                break;
            case DifficultyType.Easy:
                res = "Easy";
                break;
        }

        return res;
    }
}
