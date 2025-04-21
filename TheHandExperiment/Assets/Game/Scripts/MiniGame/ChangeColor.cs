using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ChangeColor : MonoBehaviour
{
    // Enumération des différentes couleurs utilisées dans le jeu
    public enum ColorType
    {
        Default = 0,
        Blue = 1,
        Red = 2,
        Yellow = 3,
        Pink = 4,
        Orange = 5,
        Purple = 6,
        Green = 7,
        Start = 8
    }

    // Déclaration des matériaux associés à chaque couleur
    [SerializeField] Material deFault;
    [SerializeField] Material red;
    [SerializeField] Material blue;
    [SerializeField] Material green;
    [SerializeField] Material yellow;
    [SerializeField] Material pink;
    [SerializeField] Material orange;
    [SerializeField] Material purple;

    // Sons associés aux actions du jeu
    public AudioSource wrongSound;
    public AudioSource rightSound;
    public AudioSource succeedSound;
    public AudioSource clickSound;

    // Variable pour définir la difficulté du round
    public int roundDifficulty;

    // Evenement déclenché lorsque le jeu est terminé
    public UnityEvent OnGameComplete;

    private bool isMiniGamePlaying = false;
    private LevelsScores levelsScores;
    private float timer;

    // Getter pour vérifier si un mini-jeu est en cours
    public bool IsMiniGamePlaying
    {
        get { return isMiniGamePlaying; }
    }

    // Setter pour changer l'état du mini-jeu
    public void SetIsMiniGamePlaying(bool value)
    {
        isMiniGamePlaying = value;
    }

    // Dictionnaire des matériaux avec ColorType comme clé
    Dictionary<ColorType, Material> materials = new Dictionary<ColorType, Material>();

    // Couleur actuelle du cube
    ColorType currentColor = ColorType.Default;

    private bool isRoundInProgress = false;

    // Initialisation des matériaux et autres composants
    void Awake()
    {
        materials.Add(ColorType.Default, deFault);
        materials.Add(ColorType.Blue, blue);
        materials.Add(ColorType.Red, red);
        materials.Add(ColorType.Yellow, yellow);
        materials.Add(ColorType.Pink, pink);
        materials.Add(ColorType.Orange, orange);
        materials.Add(ColorType.Purple, purple);
        materials.Add(ColorType.Green, green);
    }

    // Initialisation au démarrage du jeu
    private void Start()
    {
        ChangeMaterial(ColorType.Default); // Initialisation avec la couleur par défaut
        levelsScores = GetComponent<LevelsScores>();
    }

    // Mise à jour du timer pour la durée du jeu
    private void Update()
    {
        if (isRoundInProgress)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }

    // Changement du matériau en fonction de la couleur choisie
    public void ChangeMaterial(ColorType color)
    {
        GetComponent<Renderer>().material = materials[color];
        currentColor = color;
    }

    // Démarre le mini-jeu en appelant la coroutine
    public void StartMiniGame()
    {
        StartCoroutine(MiniGame());
    }

    // Récupère la difficulté du joueur et l'assigne à la variable roundDifficulty
    private void GetDifficulty()
    {
        switch (PlayerScoreManager.Instance.difficulty)
        {
            case PlayerScoreManager.DifficultyType.Easy:
                roundDifficulty = 2;
                break;
            case PlayerScoreManager.DifficultyType.Normal:
                roundDifficulty = 3;
                break;
            case PlayerScoreManager.DifficultyType.Intermediate:
                roundDifficulty = 4;
                break;
            case PlayerScoreManager.DifficultyType.Hard:
                roundDifficulty = 5;
                break;
            case PlayerScoreManager.DifficultyType.Expert:
                roundDifficulty = 6;
                break;
        }
    }

    // Coroutine qui gère le déroulement du mini-jeu
    IEnumerator MiniGame()
    {
        GetDifficulty();

        // Génère une série de couleurs pour chaque round
        List<ColorType> firstRoundAns = ChooseRandomColor(roundDifficulty);
        List<ColorType> secondRoundAns = ChooseRandomColor(roundDifficulty);
        List<ColorType> finalRoundAns = ChooseRandomColor(roundDifficulty);

        // Attente avant de commencer
        yield return new WaitForSeconds(1.0f);

        // Première manche
        StartCoroutine(changeCubeColor(firstRoundAns, ColorType.Default, roundDifficulty, 1));
        yield return new WaitUntil(() => !isRoundInProgress);
        yield return new WaitForSeconds(2.0f);

        // Deuxième manche
        StartCoroutine(changeCubeColor(secondRoundAns, currentColor, roundDifficulty, 2));
        yield return new WaitUntil(() => !isRoundInProgress);
        yield return new WaitForSeconds(2.0f);

        // Dernière manche
        StartCoroutine(changeCubeColor(finalRoundAns, currentColor, roundDifficulty, 3));
        yield return new WaitUntil(() => !isRoundInProgress);
        yield return new WaitForSeconds(1.0f);

        succeedSound.Play();
        OnGameComplete.Invoke();

        levelsScores.DebugCurrentScore();
        levelsScores.AddTotalScore();
        levelsScores.DisplayScore();
    }

    // Crée une liste de couleurs au hasard
    private List<ColorType> ChooseRandomColor(int numberColors)
    {
        List<ColorType> ans = new List<ColorType>();

        while (ans.Count != numberColors)
        {
            ColorType value = (ColorType)Random.Range(1, Enum.GetValues(typeof(ColorType)).Length - 1);
            if (ans.Count != 0)
            {
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

        printListDebug(ans);
        return ans;
    }

    // Gère le changement de couleur du cube pendant chaque manche
    private IEnumerator changeCubeColor(List<ColorType> answers, ColorType lastValue, int numbersAns, int round, int errors = 0)
    {
        isRoundInProgress = true;
        foreach (ColorType val in answers)
        {
            isMiniGamePlaying = true;
            clickSound.Play();
            ChangeMaterial(val);
            yield return new WaitForSeconds(1.0f);
        }
        ChangeMaterial(ColorType.Default);
        isMiniGamePlaying = false;

        bool failed = false;
        int correctColors = 0;
        while (correctColors < numbersAns)
        {
            yield return new WaitUntil(() => lastValue != currentColor); // Attente du changement de couleur

            lastValue = currentColor;

            if (currentColor == answers[correctColors]) // Vérification si la couleur actuelle correspond à la bonne réponse
            {
                correctColors++;
            }
            else
            {
                wrongSound.Play();
                failed = true;
                lastValue = ColorType.Default;
                break;
            }
        }
        yield return new WaitForSeconds(1.0f);
        ChangeMaterial(ColorType.Default);

        // Si l'utilisateur a échoué, recommencer la manche
        if (failed)
        {
            errors++;
            StartCoroutine(changeCubeColor(answers, lastValue, numbersAns, round, errors));
            yield break;
        }
        // Si réussi, passer à la manche suivante
        else
        {
            levelsScores.UpdateScore(round, errors, timer);
            rightSound.Play();
            isRoundInProgress = false;
        }
    }

    // Affiche les couleurs sélectionnées dans la console pour débogage
    void printListDebug(List<ColorType> list)
    {
        if (list.Count == 0)
        {
            Debug.Log("<color=red>La liste est vide.</color>");
        }
        else
        {
            string res = "<color=red>[" + string.Join(", ", list) + "]</color>";
            Debug.Log(res);
        }
    }

    // Charge le niveau suivant
    public void LoadNextLevel(int levelNumber)
    {
        if (levelNumber < 0)
            SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene("Level" + levelNumber);
    }
}
