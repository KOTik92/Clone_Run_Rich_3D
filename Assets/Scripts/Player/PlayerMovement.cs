using System;
using UnityEngine;

[Serializable]
public class PlayerMovement
{
    [SerializeField] private Transform player;
    [Space]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float laneChangeSpeed = 5f;
    [SerializeField] private float swipeSensitivity = 3f;
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 5f;

    private float _targetXPosition;
    private bool _isDragging;
    private float _dragStartX;
    private float _dragDelta;
    private bool _isRotating;
    private float _targetRotationAngle;
    private float _currentRotationAngle;
    private Vector3 _rotationCenter;
    private Quaternion _startRotation;
    private bool _shouldResetTilt;
    private Transform _transform;
    private Animator _animator;

    public void Init(Transform transform, Animator animator)
    {
        _transform = transform;
        _animator = animator;
        _targetXPosition = 0;
        _shouldResetTilt = false;

        Vector3 newPos = new Vector3(
            _transform.position.x, 
            player.transform.position.y, 
            _transform.position.z);

        player.transform.position = newPos;
        
        _animator.transform.localEulerAngles = new Vector3(
            _animator.transform.localEulerAngles.x, 
            0, 
            _animator.transform.localEulerAngles.z);
    }
    
    public void Move()
    {
        MovePlayer();
        HandleInput();
        HandleTilt();
    }
    
    private void MovePlayer()
    {
        _animator.SetBool("Move", true);
        _transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        Vector3 newPos = new Vector3(
            _transform.position.x, 
            player.transform.position.y, 
            _transform.position.z);
        
        Vector3 targetPosition = newPos + _transform.right * _targetXPosition;
        
        player.transform.position = Vector3.Lerp(
            player.transform.position, 
            targetPosition, 
            laneChangeSpeed * Time.deltaTime);

        if (_isRotating)
        {
            Rotation();
        }
    }

    private void Rotation()
    {
        float rotationStep = rotationSpeed * Time.deltaTime * Mathf.Sign(_targetRotationAngle);
        float remainingAngle = Mathf.Abs(_targetRotationAngle - _currentRotationAngle);
        
        if (remainingAngle <= Mathf.Abs(rotationStep))
        {
            _transform.RotateAround(
                _rotationCenter, 
                Vector3.up, 
                _targetRotationAngle - _currentRotationAngle);
            
            _currentRotationAngle = _targetRotationAngle;
            _isRotating = false;
            
            _transform.rotation = Quaternion.Euler(
                0, 
                _startRotation.eulerAngles.y + _targetRotationAngle, 
                0);
        }
        else
        {
            _transform.RotateAround(_rotationCenter, Vector3.up, rotationStep);
            _currentRotationAngle += rotationStep;
        }
    }
    
    private void HandleInput()
    {
        if (Input.GetMouseButton(0) && !_isDragging)
        {
            _dragStartX = Input.mousePosition.x;
            _shouldResetTilt = false;
            _isDragging = true;
        }
        else if(!Input.GetMouseButton(0) && _isDragging)
        {
            _shouldResetTilt = true;
            _isDragging = false;
        }

        if (_isDragging)
        {
            _dragDelta = (Input.mousePosition.x - _dragStartX) / Screen.width * 2f * swipeSensitivity;
            _targetXPosition = Mathf.Clamp(
                _dragDelta, 
                -laneDistance, 
                laneDistance);
        }
    }

    private void HandleTilt()
    {
        float targetTiltAngle;
        
        if (_shouldResetTilt)
        {
            targetTiltAngle = 0f;
            if (Mathf.Abs(_animator.transform.localEulerAngles.y) < 0.1f)
            {
                _animator.transform.localEulerAngles = new Vector3(
                    _animator.transform.localEulerAngles.x,
                    0,
                    _animator.transform.localEulerAngles.z);
                
                _shouldResetTilt = false;
            }
        }
        else
        {
            targetTiltAngle = maxTiltAngle * (_targetXPosition / laneDistance);
        }
        
        float currentTiltAngle = _animator.transform.localEulerAngles.y;
        if (currentTiltAngle > 180) currentTiltAngle -= 360f;
        
        float newTiltAngle = Mathf.Lerp(
            currentTiltAngle, 
            targetTiltAngle, 
            tiltSpeed * Time.deltaTime);
        
        _animator.transform.localEulerAngles = new Vector3(
            _animator.transform.localEulerAngles.x, 
            newTiltAngle, 
            _animator.transform.localEulerAngles.z);
    }

    public void StartRotation(Vector3 center, float targetAngle)
    {
        if (_isRotating) return;

        _rotationCenter = center;
        _startRotation = _transform.rotation;
        _targetRotationAngle = targetAngle;
        float currentAngle = _transform.eulerAngles.y;
        _targetRotationAngle = Mathf.DeltaAngle(currentAngle, _targetRotationAngle);
        _currentRotationAngle = 0f;
        
        _isRotating = true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            _transform.position + Vector3.up + (-_transform.right * laneDistance), 
            _transform.position + Vector3.up + (_transform.right * laneDistance));
    }
}
