using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera _camera;
    private CharacterController _controller;
    
    private const float Gravity = -9.81f;

    private Vector3 _moveInput, _velocity, _camR, _camFwd, _targetR, _targetFwd;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (TargetFinder.OnTarget)
        {
            _targetFwd = new Vector3((TargetFinder.CurrentTarget.position - _camera.transform.position).x, 0f, (TargetFinder.CurrentTarget.position - _camera.transform.position).z);
            _targetR = Vector3.Cross( Vector3.up, _targetFwd.normalized);
            
            OnTargetMovement(_targetR, _targetFwd, PlayerInput.MoveInput);
        }
        else
        {
            _camR = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z);
            _camFwd = new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z);
            
            BaseMovement(_camR, _camFwd, PlayerInput.MoveInput);
        }
    }

    private void BaseMovement(Vector3 camR, Vector3 camFwd, Vector2 moveInput)
    {
        if (_controller.isGrounded)
        {
            var move = moveInput.x * camR + moveInput.y * camFwd;
            _controller.Move(move.normalized * (10 * Time.deltaTime));

            if (move != Vector3.zero)
            {
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(move, Vector3.up), 15);
            }
        }
        else
        {
            _velocity.y += Gravity;
            _controller.Move(_velocity);
        }
    }
    
    private void OnTargetMovement(Vector3 targetR, Vector3 targetFwd, Vector2 moveInput)
    { 
        if (_controller.isGrounded)
        {
            var move = moveInput.x * targetR + moveInput.y * targetFwd;
            _controller.Move(move.normalized * (10 * Time.deltaTime));
            
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetFwd, Vector3.up), 5);
        }
        else
        {
            _velocity.y += Gravity;
            _controller.Move(_velocity);
        }
    }
}
