using UnityEngine;

public class TargetC : MonoBehaviour
{
    private Camera _camera;
    private LayerMask _layerMask;
    private MeshRenderer _meshRenderer;
    
    private bool _isVisible;

    private void Start()
    {
        _camera = Camera.main;
        _layerMask = LayerMask.GetMask("Obstacles");
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (TargetFinder.CurrentTarget == transform)
        {
            _meshRenderer.material.color = Color.coral;
        }
        else
        {
            _meshRenderer.material.color = Color.darkSlateGray;
        }
    }

    private void LateUpdate()
    {
        _isVisible = !Physics.Raycast(transform.position, _camera.transform.position - transform.position, out _, Vector3.Distance(transform.position, _camera.transform.position), _layerMask);
        
        if (_isVisible)
        {
            TargetFinder.AddTarget(transform);
        }
        else
        {
            TargetFinder.RemoveTarget(transform);
        }
    }
}
