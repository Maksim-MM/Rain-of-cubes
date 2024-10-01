using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent ( typeof ( MeshRenderer ) )]
[RequireComponent ( typeof ( Rigidbody ) )]
public class Cube : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private bool _isFirstTouch = true;
    private int _minLifetime = 2;
    private int _maxLifetime = 6;
    private Coroutine _countCoroutine;
    
    public event Action<Cube> LifeStopped;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Plane plane))
        {
            if (_isFirstTouch)
            {
                _countCoroutine = StartCoroutine(RunLifetime());
                _renderer.material.color = Random.ColorHSV();
                _isFirstTouch = false;
            }
        }
    }
    
    private IEnumerator RunLifetime()
    {
        var wait = new WaitForSeconds(Random.Range(_minLifetime,_maxLifetime));
        yield return wait;
        
        LifeStopped?.Invoke(this);
        
        ResetToDefault();
    }
    
    private void ResetToDefault()
    {
        _renderer.material.color = new Color(1, 1, 1);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _isFirstTouch = true;
    }
}
