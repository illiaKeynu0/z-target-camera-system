using UnityEngine;

public class PivotController : MonoBehaviour
{
    private Transform _player;
    private Camera _camera;

    private bool _wasTargeting, _isRecoveringRotation;
    private float _vRotation, _hRotation;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.position = _player.position;
        
        if (_wasTargeting && !TargetFinder.OnTarget)
        {
            _isRecoveringRotation = true;

            _vRotation = Mathf.DeltaAngle(0, transform.eulerAngles.x);
            _hRotation = transform.eulerAngles.y;
        }
        
        if (TargetFinder.OnTarget)
        {
            var targetViewPos = _camera.WorldToViewportPoint(TargetFinder.CurrentTarget.transform.position);
            var dir = TargetFinder.CurrentTarget.transform.position - transform.position;

            switch (targetViewPos.y)
            {
                case > 0.7f:
                    _vRotation -= .1f;
                    break;
                case < 0.65f:
                    _vRotation += .1f;
                    break;
            }
            _hRotation = Mathf.MoveTowardsAngle(_hRotation,Quaternion.LookRotation(dir, transform.up).eulerAngles.y, 2.5f);
        }
        else
        {
            if (_isRecoveringRotation)
            {
                switch (_vRotation)
                {
                    case > 70:
                        _vRotation = Mathf.MoveTowards(_vRotation, 70, 20);
                        break;
                    case < -20:
                        _vRotation = Mathf.MoveTowards(_vRotation, -20, 20);
                        break;
                    default:
                        _isRecoveringRotation = false;
                        break;
                }
            }
            else
            {
                _vRotation -= PlayerInput.MouseInput.y * .1f;
                _hRotation += PlayerInput.MouseInput.x * .1f;
            }
        }
        _vRotation = Mathf.Clamp(_vRotation, -20, 70);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_vRotation, _hRotation, 0), 10);

        _wasTargeting = TargetFinder.OnTarget;
    }
}
