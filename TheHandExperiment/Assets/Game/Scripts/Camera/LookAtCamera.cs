using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var newForward = (-1 * transform.position + _camera.transform.position).normalized;
        //transform.LookAt(_camera.transform);
        transform.forward = newForward;
        //Debug.Log(newForward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
