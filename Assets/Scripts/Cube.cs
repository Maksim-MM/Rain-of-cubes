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
    private int _planeId;
    private int _previousPlaneId;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Plane plane))
        {
            _planeId = plane.GetInstanceID();
            
            if (_planeId != _previousPlaneId)
            {
                _renderer.material.color = Random.ColorHSV();
                _previousPlaneId = _planeId;
            }
        }
    }

    public void ResetToDefault()
    {
        _renderer.material.color = new Color(1, 1, 1);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _previousPlaneId = 0;
    }
}
