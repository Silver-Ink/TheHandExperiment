using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    internal class ChangeColor : MonoBehaviour
    {

        [SerializeField]
        Material red;

        [SerializeField]
        Material blue;

        [SerializeField]
        Material green;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeMaterial(int color)
        {
            if (color == 0)
            {
                GetComponent<Renderer>().material = green;
            }
            if (color == 1)
            {
                GetComponent<Renderer>().material = blue;
            }
            if (color == 2)
            {
                GetComponent<Renderer>().material = red;
            }
        }
    }
}