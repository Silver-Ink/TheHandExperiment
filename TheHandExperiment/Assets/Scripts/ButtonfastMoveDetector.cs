using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonfastMoveDetector : MonoBehaviour
{
    public PhysicsButton Button;
    public LayerMask playerMask;
    private void OnTriggerEnter(Collider other)
    {
        if ((playerMask & (1 << other.gameObject.layer)) != 0)
        {
            Button.FastMoveEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((playerMask & (1 << other.gameObject.layer)) != 0)
        {
            Button.FastMoveExited();
        }
    }
}
