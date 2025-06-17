using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform lookAtTarget;
    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private float rotationSmoothSpeed = 0.1f;
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 2, -5);
    
    private Quaternion _targetRotation;

    private void Awake()
    {
        SetCameraOnTarget();
    }

    private void SetCameraOnTarget()
    {
        Vector3 targetPosition = playerTarget.TransformPoint(positionOffset);
        transform.position = targetPosition;
        
        _targetRotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);
        transform.rotation = _targetRotation;
    }

    private void FixedUpdate()
    {
        if (playerTarget == null) return;
        
        Vector3 targetPosition = playerTarget.TransformPoint(positionOffset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
        if (lookAtTarget == null) return;
        
        _targetRotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSmoothSpeed);
    }
}
