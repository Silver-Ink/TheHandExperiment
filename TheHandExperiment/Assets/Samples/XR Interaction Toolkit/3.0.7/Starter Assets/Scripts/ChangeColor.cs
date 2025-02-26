using System.Collections.Generic;
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

        [SerializeField]
        Material yellow;

        [SerializeField]
        Material pink;

        [SerializeField]
        Material orange;

        [SerializeField]
        Material purple;

        Dictionary<int, Material> materials = new Dictionary<int, Material>(7);

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            //Initialize dictionary of materials
            materials.Add(0, green);

            materials.Add(1, blue);
            materials.Add(2, red);
            materials.Add(3, yellow);
            materials.Add(4, pink);
            materials.Add(5, orange);
            materials.Add(6, purple);

            //Debug.Log("<color=red>Initialized ! : " + materials.Count + " </color>");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeMaterial(int color)
        {
            GetComponent<Renderer>().material = materials[color];
        }
    }
}