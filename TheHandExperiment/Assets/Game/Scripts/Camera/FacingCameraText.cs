using UnityEngine;

public class FacingCameraText : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    // Oriente l'objet pour qu'il fasse toujours face à la caméra
    void Update()
    {
        transform.forward = -_camera.transform.forward;
    }
}
