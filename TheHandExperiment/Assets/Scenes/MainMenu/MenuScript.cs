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
            SceneManager.LoadScene("Leo_Scene");
        }

        public void ToQuit()
        {
            Application.Quit();
        }
    }
}
