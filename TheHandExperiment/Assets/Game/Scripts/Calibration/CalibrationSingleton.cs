using UnityEngine;

public class CalibrationSingleton : MonoBehaviour
{
    // Instance unique du singleton
    public static CalibrationSingleton Instance;

    public float HeightAjustment = 0f;
    public float TableHeight = 0f;

    // Initialise le singleton et le rend persistant entre les scènes
    void Start()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Fonction appelée à chaque frame (vide ici)
    void Update()
    {

    }
}
