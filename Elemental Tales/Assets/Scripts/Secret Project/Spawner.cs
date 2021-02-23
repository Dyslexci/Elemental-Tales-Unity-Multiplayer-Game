using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    float nextAsteroidSpawnTime = 0f;
    public bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            SpawnAsteroids(ChooseSpawningSide());
        }

        if(Time.time >= nextAsteroidSpawnTime && isSpawning)
        {
            SpawnAsteroids(ChooseSpawningSide());
            nextAsteroidSpawnTime = Time.time + Random.Range(.5f, 1.5f);
        }
    }

    void SpawnAsteroids(int side)
    {
        if(side == 1)
        {
            int posX = Random.Range(-100, -110);
            int posY = Random.Range(-47, 47);
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector2(posX, posY), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            asteroid.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(10, 40), Random.Range(-15, 15)));
        } else if(side == 2)
        {
            int posX = Random.Range(100, 110);
            int posY = Random.Range(-47, 47);
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector2(posX, posY), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            asteroid.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-10, -40), Random.Range(-15, 15)));
        } else if(side == 3)
        {
            int posX = Random.Range(-93, 93);
            int posY = Random.Range(55, 70);
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector2(posX, posY), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            asteroid.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-15, 15), Random.Range(-10, -40)));
        } else
        {
            int posX = Random.Range(-93, 93);
            int posY = Random.Range(-55, -70);
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector2(posX, posY), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            asteroid.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-15, 15), Random.Range(10, 40)));
        }
    }

    /// <summary>
    /// 1 = left
    /// 2 = right
    /// 3 = top
    /// 4 = bottom
    /// </summary>
    int ChooseSpawningSide()
    {
        return Random.Range(1, 5);
    }
}
