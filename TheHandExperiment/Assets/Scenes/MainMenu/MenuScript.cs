using System.Security.Principal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Scenes.MainMenu
{
    public class MenuScript : MonoBehaviour
    {
        public void ToLevel()
        {
            SceneManager.LoadScene("Level1"); 
        }

        public void ToQuit()
        {
            Application.Quit();
        }
    }
}
