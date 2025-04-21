using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TutorialScript : MonoBehaviour
{
    // Déclaration des matériaux utilisés pour changer la couleur du cube
    [SerializeField] private Material deFault;
    [SerializeField] private Material red;
    [SerializeField] private Material blue;
    [SerializeField] private Material green;
    [SerializeField] private Material yellow;
    [SerializeField] private Material pink;
    [SerializeField] private Material orange;
    [SerializeField] private Material purple;

    // Références audio pour les sons
    public AudioSource wrongSound;
    public AudioSource rightSound;
    public AudioSource succeedSound;
    public AudioSource clickSound;

    public int roundDifficulty = 3; // Difficulté du round (nombre de couleurs)

    // Textes d'interface pour différentes étapes
    [SerializeField] private GameObject startingText;
    [SerializeField] private GameObject cubeText;
    [SerializeField] private GameObject buttonsText;
    [SerializeField] private GameObject quitText;

    // Propriété pour savoir si le mini-jeu est en cours
    public bool IsMiniGamePlaying { get; private set; }

    // Initialisation des matériaux et couleurs
    private readonly Dictionary<ChangeColor.ColorType, Material> _materials = new();
    private ChangeColor.ColorType _currentColor = ChangeColor.ColorType.Default;
    private bool _isRoundInProgress;

    // Initialisation des matériaux et de l'interface au lancement
    private void Awake()
    {
        // Lier chaque couleur à un matériau spécifique
        _materials.Add(ChangeColor.ColorType.Default, deFault);
        _materials.Add(ChangeColor.ColorType.Blue, blue);
        _materials.Add(ChangeColor.ColorType.Red, red);
        _materials.Add(ChangeColor.ColorType.Yellow, yellow);
        _materials.Add(ChangeColor.ColorType.Pink, pink);
        _materials.Add(ChangeColor.ColorType.Orange, orange);
        _materials.Add(ChangeColor.ColorType.Purple, purple);
        _materials.Add(ChangeColor.ColorType.Green, green);

        // Afficher les textes d'introduction
        startingText.SetActive(true);
        cubeText.SetActive(false);
        buttonsText.SetActive(false);
        quitText.SetActive(false);
    }

    // Méthode de démarrage pour définir la couleur par défaut
    private void Start()
    {
        ChangeMaterial(ChangeColor.ColorType.Default); // Initialiser avec la couleur par défaut
    }

    // Change le matériau du cube selon la couleur choisie
    public void ChangeMaterial(ChangeColor.ColorType color)
    {
        GetComponent<Renderer>().material = _materials[color];
        _currentColor = color;
    }

    // Démarre le mini-jeu
    public void StartMiniGame()
    {
        startingText.SetActive(false);
        StartCoroutine(MiniGame());
    }

    // Coroutine pour gérer les différentes étapes du mini-jeu
    private IEnumerator MiniGame()
    {
        var firstRoundAns = ChooseRandomColor(roundDifficulty);
        var secondRoundAns = ChooseRandomColor(roundDifficulty);
        var finalRoundAns = ChooseRandomColor(roundDifficulty);

        yield return new WaitForSeconds(1.0f); // Attente de 1 seconde avant de commencer

        // Démarre la première manche
        StartCoroutine(ChangeCubeColor(firstRoundAns, ChangeColor.ColorType.Default, roundDifficulty));
        yield return new WaitUntil(() => !_isRoundInProgress); // Attente de la fin de la manche

        quitText.SetActive(true); // Afficher le texte de fin

        yield return new WaitForSeconds(1.0f);

        succeedSound.Play(); // Jouer le son de succès
    }

    // Crée une liste de couleurs choisies au hasard
    private static List<ChangeColor.ColorType> ChooseRandomColor(int numberColors)
    {
        var ans = new List<ChangeColor.ColorType>();

        while (ans.Count != numberColors)
        {
            var value = (ChangeColor.ColorType)Random.Range(1, Enum.GetValues(typeof(ChangeColor.ColorType)).Length - 1);
            if (ans.Count != 0 && ans[^1] != value) // Éviter les doublons
            {
                ans.Add(value);
            }
            else
            {
                ans.Add(value);
            }
        }

        PrintListDebug(ans); // Affichage de la liste pour débogage
        return ans;
    }

    // Change la couleur du cube en fonction des couleurs à suivre
    private IEnumerator ChangeCubeColor(List<ChangeColor.ColorType> answers, ChangeColor.ColorType lastValue, int numbersAns)
    {
        _isRoundInProgress = true;
        cubeText.SetActive(true);

        // Afficher chaque couleur du mini-jeu
        foreach (var val in answers)
        {
            IsMiniGamePlaying = true;
            clickSound.Play(); // Jouer le son de clic
            ChangeMaterial(val); // Changer la couleur
            yield return new WaitForSeconds(1.0f); // Attendre avant de changer
        }

        ChangeMaterial(ChangeColor.ColorType.Default); // Revenir à la couleur par défaut
        IsMiniGamePlaying = false;
        cubeText.SetActive(false);
        buttonsText.SetActive(true); // Afficher les boutons pour la sélection de l'utilisateur

        var failed = false;
        var correctColors = 0;

        // Vérification des couleurs choisies par l'utilisateur
        while (correctColors < numbersAns)
        {
            var value = lastValue;
            yield return new WaitUntil(() => value != _currentColor); // Attente que la couleur change

            lastValue = _currentColor;

            // Vérifier si la couleur choisie correspond à la réponse attendue
            if (_currentColor == answers[correctColors])
            {
                correctColors++;
            }
            else
            {
                wrongSound.Play(); // Jouer le son d'erreur
                failed = true;
                lastValue = ChangeColor.ColorType.Default;
                break; // Arrêter en cas d'échec
            }
        }

        yield return new WaitForSeconds(1.0f);
        ChangeMaterial(ChangeColor.ColorType.Default); // Remettre la couleur par défaut

        // Si échec, recommencer la manche
        if (failed)
        {
            StartCoroutine(ChangeCubeColor(answers, lastValue, numbersAns));
            buttonsText.SetActive(false);
            yield break;
        }

        rightSound.Play(); // Jouer le son de succès
        buttonsText.SetActive(false);
        _isRoundInProgress = false;
    }

    // Affiche la liste des couleurs pour débogage
    private static void PrintListDebug(List<ChangeColor.ColorType> list)
    {
        if (list.Count == 0)
        {
            Debug.Log("<color=red>La liste est vide.</color>");
        }
        var res = list.Aggregate("<color=red>[", (current, c) => current + (c + ", "));
        res += "]</color>";
        Debug.Log(res);
    }

    // Quitte le tutoriel et retourne au menu principal
    public void QuitTutorial()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
