using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    private int _poolCapacity = 5;
    private int _poolMaxSize = 5;
    private float _randomAngle;
    private float _randomRadius;
    private float _radiusScaler = 2f;
    private float _spawnDelay = 2f;
    private Vector3 _spawnOffset = new Vector3(0f, -0.75f, 0f);
    private ObjectPool<Cube> _cubesPool;
    private Coroutine _countCoroutine;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
        
        _wait = new WaitForSeconds(_spawnDelay);
    }

    private void Start()
    {
        StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn()
    {
        while (true)
        {
            yield return _wait;
            
            Cube cube = _cubesPool.Get();
            cube.transform.position = GetSpawnPoint();

            cube.LifeStopped += ReturnToPool;
        }
    }

    private void ReturnToPool(Cube cube)
    {
        cube.LifeStopped -= ReturnToPool;
        
        _cubesPool.Release(cube);
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
