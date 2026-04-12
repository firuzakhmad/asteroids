using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Linq;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField]
    private int m_startingAsteroids;

    [SerializeField]
    private int m_maxAsteroids;

    [SerializeField]
    private Rect m_spawnArea;

    [SerializeField]
    private float m_asteroidSpawnDelay;

    private int m_currentAsteroidCount;

    [SerializeField]
    private List<GameObject> m_asteroidsPrefabs;

    [SerializeField]
    private int[] m_numToSpawnOnDeath;

    void Start()
    {
        StartCoroutine(SpawnInitialAsteroids());
    }

    private IEnumerator SpawnInitialAsteroids()
    {
        for (int i = 0; i < m_startingAsteroids; i++)
        {
            yield return new WaitForSeconds(0.1f);

            SpawnRandomAsteroid(3, GetSpawnPointRandom());
        }

        StartCoroutine(AsteroidSpawner());
    }

    private IEnumerator AsteroidSpawner()
    {
        while (m_currentAsteroidCount >= m_maxAsteroids)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(m_asteroidSpawnDelay);

        SpawnRandomAsteroid(3,GetSpawnPointRandom());
        StartCoroutine(AsteroidSpawner());
    }

    private Vector2 GetSpawnPointRandom()
    {
        Vector2 spawnPoint = new Vector2(
            UnityEngine.Random.Range(m_spawnArea.xMin, m_spawnArea.xMax),
            UnityEngine.Random.Range(m_spawnArea.yMin, m_spawnArea.yMax)
        );
        return spawnPoint;
    }

    private void SpawnRandomAsteroid(int size, Vector2 spawnPoint)
    {
        IEnumerable<GameObject> sizePrefabs = m_asteroidsPrefabs.Where((x) => x.GetComponent<AsteroidController>().Size == size);

        if (sizePrefabs == null || sizePrefabs.Count() <= 0) 
            return;
        
        int index = UnityEngine.Random.Range(0, sizePrefabs.Count());
        GameObject asteroidToSpawn = Instantiate(sizePrefabs.ElementAt(index), transform);

        asteroidToSpawn.transform.position = spawnPoint;

        AsteroidController controller = asteroidToSpawn.GetComponent<AsteroidController>();
        controller.onAsteroidDie += OnAsteroidDie;

        if (size == 3)
        {
            m_currentAsteroidCount++;
        }
    }

    private void OnAsteroidDie(AsteroidController asteroid)
    {
        int size = asteroid.Size;
        Vector2 asteroidPoint = asteroid.transform.position;

        Destroy(asteroid.gameObject); 

        GameEvents.Instance.AddToScore(1);

        if (size == 3)
        {
            m_currentAsteroidCount--;
        } 

        size--;

        int numToSpawn = m_numToSpawnOnDeath[size];

        if (size > 0)
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                SpawnRandomAsteroid(size, (UnityEngine.Random.insideUnitCircle * 5f) + asteroidPoint);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 center = new Vector3(
            m_spawnArea.center.x,
            m_spawnArea.center.y,
            0f
        );

        Vector3 size = new Vector3(
            m_spawnArea.width,
            m_spawnArea.height,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }
}
