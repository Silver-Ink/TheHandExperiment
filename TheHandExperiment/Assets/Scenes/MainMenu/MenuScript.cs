using System.Security.Principal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Scenes.MainMenu
{
    public class MenuScript : MonoBehaviour
    {
        private LevelsScores script;
        private void Start()
        {
            script = GetComponent<LevelsScores>();
        }
        public void ToLevel()
        {
            SceneManager.LoadScene("Level1"); 
        }

        public void ToQuit()
        {
            if (!script)
            {
                Debug.Log("<color=red>Null !</color>");            
            }
            script.WriteCSV();
            Application.Quit();
        }

        public void ToTutorial()
        {
            SceneManager.LoadScene("Tutorial_Scene");
        }
    }
}
