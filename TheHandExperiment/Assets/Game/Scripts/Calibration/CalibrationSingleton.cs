using UnityEngine;

public class CalibrationSingleton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static CalibrationSingleton Instance;

    public float HeightAjustment = 0f;
    public float TableHeight = 0f;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
