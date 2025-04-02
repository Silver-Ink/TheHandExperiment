using UnityEngine;

public class CalibrationUpdater : MonoBehaviour
{
    [SerializeField] private GameObject RemotePlayer;
    [SerializeField] private GameObject InatePlayer;
    [SerializeField] private GameObject Table1;
    [SerializeField] private GameObject Table2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RemotePlayer)
		{
            RemotePlayer.transform.position += new Vector3(0f, CalibrationSingleton.Instance.HeightAjustment, 0f);
            Debug.Log("Player pos + " + CalibrationSingleton.Instance.HeightAjustment.ToString());
		}
        if (InatePlayer)
            InatePlayer.transform.position += new Vector3(0f, CalibrationSingleton.Instance.HeightAjustment, 0f);

        if (Table1)
		{
            Table1.transform.position += new Vector3(0f, CalibrationSingleton.Instance.TableHeight, 0f);
            Debug.Log("Table pos + " + CalibrationSingleton.Instance.HeightAjustment.ToString());
		}
        if (Table2)
            Table2.transform.position += new Vector3(0f, CalibrationSingleton.Instance.TableHeight, 0f);
    }
}
