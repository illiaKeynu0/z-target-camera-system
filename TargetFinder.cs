using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public static Transform CurrentTarget;

    public static bool OnTarget;
    
    private static TargetFinder Instance;
    private static List<Transform> _targets, _targetCycle;
    private static Camera _camera;

    private static bool _cd;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _camera = Camera.main;
        
        _targets = new List<Transform>();
        _targetCycle = new List<Transform>();

        OnTarget = false;
        CurrentTarget = null;
        _cd = false;
    }

    private void Update()
    {
        if (OnTarget)
        {
            TargetFind();
        }
    }

    public static void TargetLockOn()
    {
        CurrentTarget = TargetFind();
        
        if (CurrentTarget)
        {
            OnTarget = true;
        }
    }

    public static void TargetLockOff()
    {
        CurrentTarget = null;
        OnTarget = false;
    }
    
    public static void AddTarget(Transform transform)
    {
        if (!_targets.Contains(transform))
        {
            _targets.Add(transform);
        }
    }

    public static void RemoveTarget(Transform transform)
    {
        if (_targets.Contains(transform))
        {
            _targets.Remove(transform);
        }
    }

    public static void TargetSelect(int i)
    {
        if (!_cd && _targetCycle != null && _targetCycle.Count > 1)
        {
            var currenIndex = _targetCycle.FindIndex(a => a.transform == CurrentTarget);
            
            var nextIndex = currenIndex + i;
            if (nextIndex >= 0 && nextIndex < _targetCycle.Count)
            {
                CurrentTarget = _targetCycle[nextIndex];
                _cd = true;
                Instance.StartCoroutine("ResetCd");
            }
        }
    }

    private static Transform TargetFind()
    {
        _targetCycle.Clear();
        
        foreach (var target in _targets)
        {
            var targetDot =
                Vector3.Dot(_camera.transform.forward.normalized, (target.transform.position - _camera.transform.position).normalized);
            if (targetDot < .835f) continue;
            
            _targetCycle.Add(target);
        }

        _targetCycle.Sort((a,b) => _camera.WorldToViewportPoint(a.position).x.CompareTo(_camera.WorldToViewportPoint(b.position).x));
        
        return _targetCycle[^1];
    }
    
    private IEnumerator ResetCd()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _cd = false;
    }
}
