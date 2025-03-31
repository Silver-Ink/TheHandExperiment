using UnityEngine;

public class FacingCameraText : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.forward = -_camera.transform.forward;
	}
}
