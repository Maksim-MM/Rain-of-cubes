using UnityEngine;

[RequireComponent ( typeof ( MeshRenderer ) )]
public class Platform : MonoBehaviour
{
    private MeshRenderer _renderer;
    
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
    
    private void Start()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public Color GetColor()
    {
        return _renderer.material.color;
    }
}
