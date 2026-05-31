using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public static Transform CurrentTarget;

    public static bool OnTarget;
    
    private static TargetFinder Instance;
    private static List<Transform> _targets;
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
        
        OnTarget = false;
        CurrentTarget = null;
        _cd = false;
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
        var tCycle = new List<Transform>();

        foreach (var t in _targets)
        {
            if (_camera.WorldToViewportPoint(t.position).x is < 0 or > 1)
                continue;
            tCycle.Add(t);
        }
        
        tCycle.Sort((a,b) => _camera.WorldToViewportPoint(a.position).x.CompareTo(_camera.WorldToViewportPoint(b.position).x));

        if (!_cd && tCycle.Count > 1)
        {
            var currenIndex = tCycle.FindIndex(a => a.transform == CurrentTarget);
            
            var nextIndex = currenIndex + i;
            if (nextIndex >= 0 && nextIndex < tCycle.Count)
            {
                CurrentTarget = tCycle[nextIndex];
                _cd = true;
                Instance.StartCoroutine("ResetCd");
            }
        }
    }

    private static Transform TargetFind()
    {
        var topDot = 0f;
        Transform pickedTarget = null;

        foreach (var target in _targets)
        {
            var targetDot =
                Vector3.Dot(_camera.transform.forward.normalized, (target.transform.position - _camera.transform.position).normalized);
            if (targetDot < .9f) continue;

            if (targetDot >= topDot)
            {
                topDot = targetDot;
                pickedTarget = target;
            }
        }
        
        return pickedTarget;
    }
    
    private IEnumerator ResetCd()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _cd = false;
    }
}