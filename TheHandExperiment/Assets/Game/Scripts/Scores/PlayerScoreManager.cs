using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    // Instance Singleton pour assurer qu'il n'y a qu'une seule instance de PlayerScoreManager
    public static PlayerScoreManager Instance { get; private set; }

    // Liste contenant les scores des joueurs
    public LevelsScores.PlayerList playerScore = new LevelsScores.PlayerList();

    public string playerName = "";  // Nom du joueur
    public DifficultyType difficulty = DifficultyType.Normal;  // Difficulté actuelle

    // Enumération des niveaux de difficulté disponibles
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
        // Vérifie s'il existe déjà une instance, sinon crée cette instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Détruit cette instance si une autre existe déjà
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Préserve cet objet entre les scènes
        }
    }

    // Méthode pour ajouter un score pour un joueur à la liste des scores
    public void AddPlayerScore(LevelsScores.LevelScore score)
    {
        playerScore.levelScore.Add(score);  // Ajoute le score à la liste
    }

    // Récupère le nom du joueur
    public string GetPlayerNumber()
    {
        return playerName;  // Retourne le nom du joueur
    }

    // Méthode pour réinitialiser les scores des joueurs si nécessaire
    public void ResetPlayerScores()
    {
        playerScore.levelScore.Clear();  // Vide la liste des scores
    }

    // Sauvegarde les scores dans un fichier CSV lors de la fermeture de l'application
    void OnApplicationQuit()
    {
        LevelsScores.WriteCSV();  // Appelle la méthode pour écrire les scores dans un fichier CSV
    }

    // Méthode pour augmenter la difficulté du jeu
    public void AddDifficulty()
    {
        switch (difficulty)
        {
            case DifficultyType.Easy:
                difficulty = DifficultyType.Normal;  // Passe de Easy à Normal
                break;
            case DifficultyType.Normal:
                difficulty = DifficultyType.Intermediate;  // Passe de Normal à Intermediate
                break;
            case DifficultyType.Intermediate:
                difficulty = DifficultyType.Hard;  // Passe de Intermediate à Hard
                break;
            case DifficultyType.Hard:
                difficulty = DifficultyType.Expert;  // Passe de Hard à Expert
                break;
        }
    }

    // Méthode pour diminuer la difficulté du jeu
    public void SubstractDifficulty()
    {
        switch (difficulty)
        {
            case DifficultyType.Expert:
                difficulty = DifficultyType.Hard;  // Passe de Expert à Hard
                break;
            case DifficultyType.Normal:
                difficulty = DifficultyType.Easy;  // Passe de Normal à Easy
                break;
            case DifficultyType.Intermediate:
                difficulty = DifficultyType.Normal;  // Passe de Intermediate à Normal
                break;
            case DifficultyType.Hard:
                difficulty = DifficultyType.Intermediate;  // Passe de Hard à Intermediate
                break;
        }
    }

    // Retourne la difficulté actuelle sous forme de chaîne de caractères
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

        return res;  // Retourne la difficulté sous forme de texte
    }
}
