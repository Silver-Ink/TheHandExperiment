using System.Collections.Generic;
using UnityEngine;

public class LevelsScores : MonoBehaviour
{

    public struct levelScore
    {
        public struct roundScore
        {
            int time;
            int trials;
        }

        Dictionary<int, roundScore> score;
    }

    public Dictionary<int, levelScore> userScore = new Dictionary<int, levelScore>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
