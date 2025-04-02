using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance { get; private set; }

    // Liste contenant les scores des joueurs
    public LevelsScores.PlayerList playerScore = new LevelsScores.PlayerList();

    public string playerName = "";

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
}
