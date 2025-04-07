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
    // Déclaration des matériaux
    [SerializeField] private Material deFault;

    [SerializeField] private Material red;

    [SerializeField] private Material blue;

    [SerializeField] private Material green;

    [SerializeField] private Material yellow;

    [SerializeField] private Material pink;

    [SerializeField] private Material orange;

    [SerializeField] private Material purple;

    public AudioSource wrongSound;
    public AudioSource rightSound;
    public AudioSource succeedSound;
    public AudioSource clickSound;

    public int firstRoundDifficulty = 3;
    public int secondRoundDifficulty = 3;
    public int finalRoundDifficulty = 3;


    [SerializeField] private GameObject startingText;
    [SerializeField] private GameObject cubeText;
    [SerializeField] private GameObject buttonsText;
    [SerializeField] private GameObject quitText;
    
    // Getter
    public bool IsMiniGamePlaying { get; private set; }

    // Setter
    public void SetIsMiniGamePlaying(bool value)
    {
        IsMiniGamePlaying = value;
    }

    // Dictionnaire des matériaux avec ColorType comme clé
    private readonly Dictionary<ChangeColor.ColorType, Material> _materials = new();

    // Variable pour suivre la couleur actuelle
    private ChangeColor.ColorType _currentColor = ChangeColor.ColorType.Default;

    private bool _isRoundInProgress;

    // Start est appelé une seule fois avant le premier Update
    private void Awake()
    {
        // Initialisation du dictionnaire avec les couleurs et les matériaux associés
        _materials.Add(ChangeColor.ColorType.Default, deFault);
        _materials.Add(ChangeColor.ColorType.Blue, blue);
        _materials.Add(ChangeColor.ColorType.Red, red);
        _materials.Add(ChangeColor.ColorType.Yellow, yellow);
        _materials.Add(ChangeColor.ColorType.Pink, pink);
        _materials.Add(ChangeColor.ColorType.Orange, orange);
        _materials.Add(ChangeColor.ColorType.Purple, purple);
        _materials.Add(ChangeColor.ColorType.Green, green);
        startingText.SetActive(true);
        cubeText.SetActive(false);
        buttonsText.SetActive(false);
        quitText.SetActive(false);
    }

    private void Start()
    {
        ChangeMaterial(ChangeColor.ColorType.Default); // Initialiser avec la couleur par défaut
        //StartMiniGame(); // Démarrer le mini-jeu
    }

    // Méthode pour déclencher un changement de couleur

    // Méthode pour changer le matériau en fonction de la couleur choisie
    public void ChangeMaterial(ChangeColor.ColorType color)
    {
        GetComponent<Renderer>().material = _materials[color];
        _currentColor = color;
        //Debug.Log("<color=blue>Color : " + color + "</color>");
    }

    // Démarre le mini-jeu
    public void StartMiniGame()
    {
        startingText.SetActive(false);
        StartCoroutine(MiniGame());
    }

    // Coroutine pour le déroulement du mini-jeu
    private IEnumerator MiniGame()
    {
        var firstRoundAns = ChooseRandomColor(firstRoundDifficulty);
        var secondRoundAns = ChooseRandomColor(secondRoundDifficulty);
        var finalRoundAns = ChooseRandomColor(finalRoundDifficulty);
        
        // Attente de 3 secondes avant de commencer
        yield return new WaitForSeconds(1.0f);

        // Démarrer le jeu, première manche
        StartCoroutine(ChangeCubeColor(firstRoundAns, _currentColor, firstRoundDifficulty));
        yield return new WaitUntil(() => !_isRoundInProgress);

        quitText.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        succeedSound.Play();
    }

    // Crée une liste avec les couleurs choisies au hasard
    private static List<ChangeColor.ColorType> ChooseRandomColor(int numberColors)
    {
        var ans = new List<ChangeColor.ColorType>();

        while (ans.Count != numberColors)
        {
            var value = (ChangeColor.ColorType)Random.Range(1, Enum.GetValues(typeof(ChangeColor.ColorType)).Length - 1); // Récupère une couleur au hasard
            if (ans.Count != 0)
            {
                // Éviter les doublons
                if (ans[^1] != value)
                {
                    ans.Add(value);
                }
            }
            else
            {
                ans.Add(value);
            }
        }

        PrintListDebug(ans);
        return ans;
    }

    // Fonction pour changer la couleur du cube en fonction de la liste de couleurs
    private IEnumerator ChangeCubeColor(List<ChangeColor.ColorType> answers, ChangeColor.ColorType lastValue, int numbersAns)
    {
        
        _isRoundInProgress = true;
        cubeText.SetActive(true);
        foreach (var val in answers)
        {
            IsMiniGamePlaying = true;
            clickSound.Play();
            ChangeMaterial(val);
            yield return new WaitForSeconds(1.0f);
        }
        ChangeMaterial(ChangeColor.ColorType.Default);
        IsMiniGamePlaying = false;
        cubeText.SetActive(false);
        buttonsText.SetActive(true);
        
        var failed = false;
        var correctColors = 0;
        while (correctColors < numbersAns)
        {
            var value = lastValue;
            yield return new WaitUntil(() => value != _currentColor); // Attente du changement de couleur

            lastValue = _currentColor;

            if (_currentColor == answers[correctColors]) // Vérification si la couleur actuelle correspond à la bonne réponse
            {
                correctColors++;
            }
            else
            {
                wrongSound.Play();
                failed = true;
                lastValue = ChangeColor.ColorType.Default;
                break;
            }
        }
        yield return new WaitForSeconds(1.0f);
        ChangeMaterial(ChangeColor.ColorType.Default);

        // Si l'utilisateur a échoué, redémarrer la manche
        if (failed)
        {
            StartCoroutine(ChangeCubeColor(answers, lastValue, numbersAns));
            buttonsText.SetActive(false);
            yield break;
        }

        rightSound.Play();
        buttonsText.SetActive(false);
        _isRoundInProgress = false;
    }

    // Affiche les couleurs sélectionnées dans la console pour débogage
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

    public void QuitTutorial()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
