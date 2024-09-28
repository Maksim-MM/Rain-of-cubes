using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;

    private int _poolCapacity = 5;
    private int _poolMaxSize = 5;
    
    private float _randomAngle;
    private float _randomRadius;
    private float _radiusScaler = 2f;
    private float _spawnDelay = 2f;
    
    private Vector3 _spawnOffset = new Vector3(0f, -0.75f, 0f);
    private Coroutine _countCoroutine;

    private ObjectPool<GameObject> _cubePool;

    private void Awake()
    {
        _cubePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => SpawnCube(obj),
            actionOnRelease: (obj) => ResetCube(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _spawnDelay);
    }
    
    private IEnumerator ReturnToPool(GameObject cube)
    {
        var wait = new WaitForSeconds(Random.Range(2,6));
        yield return wait;
        
        _cubePool.Release(cube);
    }

    private void SpawnCube(GameObject cubePrefab)
    {
        cubePrefab.transform.position = GetSpawnPoint();
        cubePrefab.SetActive(true);
        _countCoroutine = StartCoroutine(ReturnToPool(cubePrefab));
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

    private void ResetCube(GameObject cube)
    {
        Cube cubeComponent = cube.GetComponent<Cube>();
    
        if (cubeComponent != null)
        {
            cubeComponent.ResetToDefault();
        }
        
        cube.SetActive(false);
    }
}
