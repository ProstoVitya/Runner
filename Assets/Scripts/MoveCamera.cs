using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Tooltip("The position the camera should follow")]
    [SerializeField] private Transform _cameraPosition;

    private void Update()
    {
        transform.position = _cameraPosition.position;
    }
}
