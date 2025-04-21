using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    // Oriente l'objet vers la caméra au démarrage
    void Start()
    {
        var newForward = (-1 * transform.position + _camera.transform.position).normalized;
        // Applique la direction vers la caméra comme nouvelle orientation
        transform.forward = newForward;
    }
}
