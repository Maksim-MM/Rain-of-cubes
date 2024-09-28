using UnityEngine;

[RequireComponent ( typeof ( MeshRenderer ) )]
[RequireComponent ( typeof ( Rigidbody ) )]
public class Cube : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    
    private int _platformId;
    private int _previousPlatformId;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Platform platform))
        {
            _platformId = platform.GetInstanceID();
            
            if (_platformId != _previousPlatformId)
            {
                _renderer.material.color = platform.GetColor();
                _previousPlatformId = _platformId;
            }
        }
    }

    public void ResetToDefault()
    {
        _renderer.material.color = new Color(1, 1, 1);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _previousPlatformId = 0;
    }
}
