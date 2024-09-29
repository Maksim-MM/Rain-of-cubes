using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent ( typeof ( MeshRenderer ) )]
[RequireComponent ( typeof ( Rigidbody ) )]
public class Cube : MonoBehaviour
{
    public event Action<Cube> LifeStopped;

    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private int _platformId;
    private int _previousPlatformId;
    private bool _isFirstCollision;
    private int _minLifetime = 2;
    private int _maxLifetime = 6;
    private bool _isInPool = false;
    
    private Coroutine _countCoroutine;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _isFirstCollision = false;
    }

    private void Start()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Platform platform))
        {
            if (_isFirstCollision == false)
            {
                _isFirstCollision = true;
                _countCoroutine = StartCoroutine(RunLifetime());
            }
            
            _platformId = platform.GetInstanceID();
            
            if (_platformId != _previousPlatformId)
            {
                _renderer.material.color = Random.ColorHSV();
                _previousPlatformId = _platformId;
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

    public void SetOn()
    {
        gameObject.SetActive(true);
        _isInPool = false;
    }

    public void SetOff()
    {
        gameObject.SetActive(false);
        _isInPool = true;
    }

    public bool GetCubeStatus()
    {
        return _isInPool;
    }

    private void ResetToDefault()
    {
        _renderer.material.color = new Color(1, 1, 1);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _previousPlatformId = 0;
        _isFirstCollision = false;
    }
}
