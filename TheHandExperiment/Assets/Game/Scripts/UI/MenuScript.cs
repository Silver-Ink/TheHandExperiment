using System.Security.Principal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Scenes.MainMenu
{
    public class MenuScript : MonoBehaviour
    {
        // Charge le niveau "Level1" lorsqu'il est appelé (ex: via un bouton)
        public void ToLevel()
        {
            SceneManager.LoadScene("Level1");  // Charge la scène "Level1"
        }

        // Quitte l'application lorsqu'il est appelé (ex: via un bouton)
        public void ToQuit()
        {
            Application.Quit();  // Ferme l'application
        }

        // Charge la scène "Tutorial_Scene" lorsqu'il est appelé (ex: via un bouton)
        public void ToTutorial()
        {
            SceneManager.LoadScene("Tutorial_Scene");  // Charge la scène du tutoriel
        }

        // Charge la scène "CalibrationLevel" lorsqu'il est appelé (ex: via un bouton)
        public void ToCalibration()
        {
            SceneManager.LoadScene("CalibrationLevel");  // Charge la scène de calibration
        }
    }
}
