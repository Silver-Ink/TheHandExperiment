using TMPro;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    [SerializeField]
    private GameObject textDifficulty; // Objet UI pour afficher la difficulté

    private TMP_Text text; // Composant TMP_Text pour modifier l'affichage de la difficulté

    private void Start()
    {
        // Récupère le composant TMP_Text et initialise le texte avec la difficulté actuelle
        text = textDifficulty.GetComponent<TMP_Text>();
        text.text = PlayerScoreManager.Instance.GetStringDifficulty();
    }

    // Incrémente la difficulté et met à jour l'affichage
    public void IncreaseDifficulty()
    {
        PlayerScoreManager.Instance.AddDifficulty();
        text.text = PlayerScoreManager.Instance.GetStringDifficulty();
    }

    // Décrémente la difficulté et met à jour l'affichage
    public void DecreaseDifficulty()
    {
        PlayerScoreManager.Instance.SubstractDifficulty();
        text.text = PlayerScoreManager.Instance.GetStringDifficulty();
    }
}
