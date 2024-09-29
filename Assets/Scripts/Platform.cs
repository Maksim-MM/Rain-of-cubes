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
        _renderer.material.color = new Color(0.9f,0.9f,0.9f);
    }
}
