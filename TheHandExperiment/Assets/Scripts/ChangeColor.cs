using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ChangeColor : MonoBehaviour
    {

    // Définir l'énumération pour les couleurs
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

    // Déclaration des matériaux
    [SerializeField]
    Material deFault;

    [SerializeField]
    Material red;

    [SerializeField]
    Material blue;

    [SerializeField]
    Material green;

    [SerializeField]
    Material yellow;

    [SerializeField]
    Material pink;

    [SerializeField]
    Material orange;

    [SerializeField]
    Material purple;

    public AudioSource wrongSound;
    public AudioSource rightSound;
    public AudioSource succeedSound;
    public AudioSource clickSound;

    public int roundDifficulty;

    public UnityEvent OnGameComplete;

    private bool isMiniGamePlaying = false;
    private LevelsScores levelsScores;
    private float timer;
    // Getter
    public bool IsMiniGamePlaying
    {
        get { return isMiniGamePlaying; }
    }

    // Setter
    public void SetIsMiniGamePlaying(bool value)
    {
        isMiniGamePlaying = value;
    }

    // Dictionnaire des matériaux avec ColorType comme clé
    Dictionary<ColorType, Material> materials = new Dictionary<ColorType, Material>();

    // Variable pour suivre la couleur actuelle
    ColorType currentColor = ColorType.Default;

    private bool isRoundInProgress = false;

    // Start est appelé une seule fois avant le premier Update
    void Awake()
    {
        // Initialisation du dictionnaire avec les couleurs et les matériaux associés
        materials.Add(ColorType.Default, deFault);
        materials.Add(ColorType.Blue, blue);
        materials.Add(ColorType.Red, red);
        materials.Add(ColorType.Yellow, yellow);
        materials.Add(ColorType.Pink, pink);
        materials.Add(ColorType.Orange, orange);
        materials.Add(ColorType.Purple, purple);
        materials.Add(ColorType.Green, green);
    }

    private void Start()
    {
        ChangeMaterial(ColorType.Default); // Initialiser avec la couleur par défaut
        levelsScores = GetComponent<LevelsScores>();
    }

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

    // Méthode pour déclencher un changement de couleur

    // Méthode pour changer le matériau en fonction de la couleur choisie
    public void ChangeMaterial(ColorType color)
    {
        GetComponent<Renderer>().material = materials[color];
        currentColor = color;
        //Debug.Log("<color=blue>Color : " + color + "</color>");
    }

    // Démarre le mini-jeu
    public void StartMiniGame()
    {
        StartCoroutine(MiniGame());
    }

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

    // Coroutine pour le déroulement du mini-jeu
    IEnumerator MiniGame()
    {
        GetDifficulty();

        List<ColorType> firstRoundAns = ChooseRandomColor(roundDifficulty);
        List<ColorType> secondRoundAns = ChooseRandomColor(roundDifficulty);
        List<ColorType> finalRoundAns = ChooseRandomColor(roundDifficulty);

        // Attente de 3 secondes avant de commencer
        yield return new WaitForSeconds(1.0f);

        // Démarrer le jeu, première manche
        StartCoroutine(changeCubeColor(firstRoundAns, ColorType.Default, roundDifficulty,1));
        yield return new WaitUntil(() => !isRoundInProgress);

        yield return new WaitForSeconds(2.0f);

        // Deuxième manche
        StartCoroutine(changeCubeColor(secondRoundAns, currentColor, roundDifficulty,2));
        yield return new WaitUntil(() => !isRoundInProgress);

        yield return new WaitForSeconds(2.0f);

        // Dernière manche
        StartCoroutine(changeCubeColor(finalRoundAns, currentColor, roundDifficulty,3));
        yield return new WaitUntil(() => !isRoundInProgress);

        yield return new WaitForSeconds(1.0f);

        succeedSound.Play();
        OnGameComplete.Invoke();

        levelsScores.DebugCurrentScore();
        levelsScores.AddTotalScore();

        levelsScores.DisplayScore();
    }

    // Crée une liste avec les couleurs choisies au hasard
    private List<ColorType> ChooseRandomColor(int numberColors)
    {
        List<ColorType> ans = new List<ColorType>();

        while (ans.Count != numberColors)
        {
            ColorType value = (ColorType)Random.Range(1, Enum.GetValues(typeof(ColorType)).Length - 1); // Récupère une couleur au hasard
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

        printListDebug(ans);
        return ans;
    }

    // Fonction pour changer la couleur du cube en fonction de la liste de couleurs
    private IEnumerator changeCubeColor(List<ColorType> answers, ColorType lastValue, int numbersAns, int round,  int errors = 0)
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

        // Si l'utilisateur a échoué, redémarrer la manche
        if (failed)
        {
            errors++;
            
            StartCoroutine(changeCubeColor(answers, lastValue, numbersAns, round, errors));
            yield break;
        }
        //Si le joueur a réussi, lancer la musique et passer au round suivant
        else
        {
            levelsScores.UpdateScore(round, errors, timer);
            //levelsScores
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
        string res = "<color=red>[";

        foreach (ColorType c in list)
        {
            res += c.ToString() + ", ";
        }

        res += "]</color>";

        Debug.Log(res);
    }

    public void LoadNextLevel(int levelNumber)
    {
        if (levelNumber < 0)
            SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene("Level" + levelNumber);
    }
}

