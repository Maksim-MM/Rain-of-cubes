using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    private int _poolCapacity = 5;
    private int _poolMaxSize = 10;
    
    private float _randomAngle;
    private float _randomRadius;
    private float _radiusScaler = 2f;
    private float _spawnDelay = 2f;
    
    private Vector3 _spawnOffset = new Vector3(0f, -0.75f, 0f);
    
    private ObjectPool<Cube> _cubePool;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => SpawnCube(obj),
            actionOnRelease: (obj) => ReturnToPool(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _spawnDelay);
    }
    
    private void SpawnCube(Cube cube)
    {
        cube.transform.position = GetSpawnPoint();
        cube.LifeStopped += ReturnToPool;
        cube.SetOn();
    }
    
    private void ReturnToPool(Cube cube)
    {
        if (cube.GetCubeStatus()) return;
        
        cube.LifeStopped -= ReturnToPool;
        cube.SetOff();
        _cubePool.Release(cube);
    }
    
    private void GetCube()
    {
        _cubePool.Get();
    }
    
    private Vector3 GetSpawnPoint()
    {
        _randomAngle = Random.Range(0f, 360f);
        _randomRadius = Random.Range(0f, GetSpawnerRadius());
        
        Vector3 centerPosition = transform.position;
        Vector3 randomVector = Quaternion.Euler(0f, _randomAngle, 0f) * Vector3.right * _randomRadius;
        Vector3 randomPoint = centerPosition + _spawnOffset + randomVector;

        return randomPoint;
    }
    
    private float GetSpawnerRadius()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        return mesh.bounds.size.x/_radiusScaler;
    }
}
