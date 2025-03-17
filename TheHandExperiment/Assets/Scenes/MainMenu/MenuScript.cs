using System.Security.Principal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Scenes.MainMenu
{
    public class MenuScript : MonoBehaviour
    {
        public GameObject button1;
        public GameObject button2;

        private bool _previouslyPressed = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            if (button1.TryGetComponent<XRPokeFollowAffordance>(out var me1))
            {
                switch (me1.isButtonPressed)
                {
                    case true:
                        _previouslyPressed = true;
                        break;
                    case false:
                    {
                        if (_previouslyPressed)
                        {
                            SceneManager.LoadScene("Leo_Scene");
                            _previouslyPressed = false;
                        }
                        break;
                    }
                }
            }
            
            else if (button2.TryGetComponent<XRPokeFollowAffordance>(out var me2))
            {
                switch (me2.isButtonPressed)
                {
                    case true: 
                        _previouslyPressed = true; 
                        break;
                    case false: 
                    {
                        if (_previouslyPressed)
                        {
                            Application.Quit();
                            _previouslyPressed = false;
                        }
                        break;
                    }
                }
            }
        }
    }
}
