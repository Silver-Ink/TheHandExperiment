using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    internal class ChangeColor : MonoBehaviour
    {
        public static event Action OnColorChanged;

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
            Green = 7
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
            StartMiniGame(); // Démarrer le mini-jeu
        }

        // Méthode pour déclencher un changement de couleur
        public void TriggerColorChange()
        {
            OnColorChanged?.Invoke();
        }

        // Méthode pour changer le matériau en fonction de la couleur choisie
        public void ChangeMaterial(ColorType color)
        {
            GetComponent<Renderer>().material = materials[color];
            currentColor = color;
            Debug.Log("<color=blue>Color : " + color + "</color>");
        }

        // Démarre le mini-jeu
        public void StartMiniGame()
        {
            StartCoroutine(MiniGame());
        }

        // Coroutine pour le déroulement du mini-jeu
        IEnumerator MiniGame()
        {
            int firstRoundDifficulty = 3;
            int secondRoundDifficulty = 4;
            int finalRoundDifficulty = 5;

            List<ColorType> firstRoundAns = ChooseRandomColor(firstRoundDifficulty);
            List<ColorType> secondRoundAns = ChooseRandomColor(secondRoundDifficulty);
            List<ColorType> finalRoundAns = ChooseRandomColor(finalRoundDifficulty);

            // Attente de 3 secondes avant de commencer
            yield return new WaitForSeconds(3.0f);

            // Démarrer le jeu, première manche
            StartCoroutine(changeCubeColor(firstRoundAns, currentColor, firstRoundDifficulty));
            yield return new WaitUntil(() => !isRoundInProgress);

            yield return new WaitForSeconds(2.0f);

            // Deuxième manche
            StartCoroutine(changeCubeColor(secondRoundAns, currentColor, secondRoundDifficulty));
            yield return new WaitUntil(() => !isRoundInProgress);

            yield return new WaitForSeconds(2.0f);

            // Dernière manche
            StartCoroutine(changeCubeColor(finalRoundAns, currentColor, finalRoundDifficulty));
        }

        // Crée une liste avec les couleurs choisies au hasard
        private List<ColorType> ChooseRandomColor(int numberColors)
        {
            List<ColorType> ans = new List<ColorType>();

            while (ans.Count != numberColors)
            {
                ColorType value = (ColorType)Random.Range(1, Enum.GetValues(typeof(ColorType)).Length); // Récupère une couleur au hasard
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
        private IEnumerator changeCubeColor(List<ColorType> answers, ColorType lastValue, int numbersAns)
        {
            isRoundInProgress = true;
            foreach (ColorType val in answers)
            {
                ChangeMaterial(val);
                yield return new WaitForSeconds(1.0f);
            }
            ChangeMaterial(ColorType.Default);

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
                    failed = true;
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
            ChangeMaterial(ColorType.Default);

            // Si l'utilisateur a échoué, redémarrer la manche
            if (failed)
            {
                StartCoroutine(changeCubeColor(answers, lastValue, numbersAns));
            }
            else
            {
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
    }
}
