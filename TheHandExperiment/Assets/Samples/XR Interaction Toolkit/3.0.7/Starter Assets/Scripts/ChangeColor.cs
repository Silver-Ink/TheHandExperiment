using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    internal class ChangeColor : MonoBehaviour
    {
        public static event Action OnColorChanged;

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

        Dictionary<int, Material> materials = new Dictionary<int, Material>(7);

        int currentColor = 0;

        private bool isRoundInProgress = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            //Initialize dictionary of materials
            materials.Add(0, deFault);
            materials.Add(1, blue);
            materials.Add(2, red);
            materials.Add(3, yellow);
            materials.Add(4, pink);
            materials.Add(5, orange);
            materials.Add(6, purple);
            materials.Add(7, green);
        }


        private void Start()
        {
            ChangeMaterial(0);
            StartMiniGame();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void TriggerColorChange()
        {
            // Déclenche l'événement
            OnColorChanged?.Invoke();
        }

        public void ChangeMaterial(int color)
        {
            GetComponent<Renderer>().material = materials[color];
            currentColor = color;
            Debug.Log("<color=blue>Color : " + color +"</color>");
        }

        public void StartMiniGame()
        {
            StartCoroutine(MiniGame());
        }

        

        IEnumerator MiniGame()
        {

            //Set Up
            int firstRoundDifficulty = 3;
            int secondRoundDifficulty = 4;
            int finalRoundDifficulty = 5;

            List<int> firstRoundAns = ChooseRandomColor(firstRoundDifficulty);
            List<int> secondRoundAns = ChooseRandomColor(secondRoundDifficulty);
            List<int> finalRoundAns = ChooseRandomColor(finalRoundDifficulty);

            //Wait 5 seconds before start game
            yield return new WaitForSeconds(3.0f);

            //Start Game
            //First Round

            StartCoroutine(changeCubeColor(firstRoundAns, currentColor, firstRoundDifficulty));

            yield return new WaitUntil(() => !isRoundInProgress);

            yield return new WaitForSeconds(2.0f);

            //Second Round
            StartCoroutine(changeCubeColor(secondRoundAns, currentColor, secondRoundDifficulty));

            yield return new WaitUntil(() => !isRoundInProgress);

            yield return new WaitForSeconds(2.0f);

            //Final Round
            StartCoroutine(changeCubeColor(finalRoundAns, currentColor, finalRoundDifficulty));


        }

        //Create a list with the wanted colors
        private List<int> ChooseRandomColor(int numberColors)
        {
            List<int> ans = new List<int>();

            while (ans.Count != numberColors)
            {
                int value = Random.Range(1, 8);
                Material mat = materials[value];
                if (ans.Count != 0)
                {
                    //Avoid following duplicates
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

        //Function to change the color of the cube
        private IEnumerator changeCubeColor(List<int> answers, int lastValue, int numbersAns)
        {
            isRoundInProgress = true;
            foreach (int val in answers)
            {
                ChangeMaterial(val);
                yield return new WaitForSeconds(1.0f);
            }
            ChangeMaterial(0);

            bool failed = false;
            int correctColors = 0;
            while (correctColors < numbersAns)
            {
                yield return new WaitUntil(() => lastValue != currentColor);

                lastValue = currentColor;

                //OnColorChangedInvoked = false;

                if (currentColor == answers[correctColors])
                {
                    correctColors++;
                }
                //Got an answer wrong, end the verification
                else
                {
                    failed = true;
                    break;
                }

            }
            yield return new WaitForSeconds(1.0f);
            ChangeMaterial(0);

            //If player failed, restart the round
            if (failed)
            {
                StartCoroutine(changeCubeColor(answers, lastValue, numbersAns));
            }
            else
            {
                isRoundInProgress = false;
            }
        }

        void printListDebug(List<int> list)
        {
            if (list.Count == 0)
            {
                Debug.Log("<color=red>La liste est vide.</color>");
            }
            string res = "<color=red>[";

            foreach (int i in list)
            {
                res +=  i + ", ";
            }

            res += "]</color>";

            Debug.Log(res);
        }
    }
}