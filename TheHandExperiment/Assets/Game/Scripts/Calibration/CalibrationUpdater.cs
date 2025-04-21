using UnityEngine;

public class CalibrationUpdater : MonoBehaviour
{
    [SerializeField] private GameObject RemotePlayer;
    [SerializeField] private GameObject InatePlayer;
    [SerializeField] private GameObject Table1;
    [SerializeField] private GameObject Table2;

    // Applique les ajustements de hauteur aux objets au lancement de la scène
    void Start()
    {
        // Ajuste la position du RemotePlayer en fonction de la calibration
        if (RemotePlayer)
        {
            RemotePlayer.transform.position += new Vector3(0f, CalibrationSingleton.Instance.HeightAjustment, 0f);
            Debug.Log("Player pos + " + CalibrationSingleton.Instance.HeightAjustment.ToString());
        }

        // Ajuste la position du InatePlayer
        if (InatePlayer)
            InatePlayer.transform.position += new Vector3(0f, CalibrationSingleton.Instance.HeightAjustment, 0f);

        // Ajuste la hauteur de la première table
        if (Table1)
        {
            Table1.transform.position += new Vector3(0f, CalibrationSingleton.Instance.TableHeight, 0f);
            Debug.Log("Table pos + " + CalibrationSingleton.Instance.HeightAjustment.ToString());
        }

        // Ajuste la hauteur de la deuxième table
        if (Table2)
            Table2.transform.position += new Vector3(0f, CalibrationSingleton.Instance.TableHeight, 0f);
    }
}
