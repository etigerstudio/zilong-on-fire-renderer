using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Entity
{
    public const string ACTOR = "ACTOR";
    public const string WALL = "WALL";
    public const string SPIKE = "SPIKE";
    public const string BARRIER = "BARRIER";
    public const string GROUND = "GROUND";
    public const string TREASURE = "TREASURE";
    public int x, y;
    public string type;

    public Entity(int x, int y, string type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}

class RPGWorld
{
    public int width, height;
    public Entity[] _entities;
}

public class RPGWorldBuilder : MonoBehaviour
{
    
    public GameObject actorPrefab;
    public GameObject wallPrefab;
    public GameObject spikePrefab;
    public GameObject barrierPrefab;
    public GameObject groundPrefab;
    public GameObject treasurePrefab;

    public GameObject worldObject;
    public float spacing = 2;

    private RPGWorld _world;
    private GameObject[,] _grounds;

    // Start is called before the first frame update
    void Start()
    {
        _world = GetRPGWorld();
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateWorld()
    {
        // Ground
        _grounds = new GameObject[_world.width, _world.height];
        for (int i = 0; i < _world.height; i++)
        {
            for (int j = 0; j < _world.width; j++)
            {
                _grounds[j, i] = SpawnPrefab(j, i, 0, groundPrefab);
            }
        }

        foreach (Entity e in _world._entities)
        {
            int x = e.x, y = e.y;
            switch (e.type)
            {
                case Entity.ACTOR:
                    SpawnPrefab(x, y, 0.55f, actorPrefab);
                    break;
                case Entity.WALL:
                    SpawnPrefab(x, y, 1, wallPrefab);
                    break;
                case Entity.BARRIER:
                    SpawnPrefab(x, y, 0, barrierPrefab);
                    break;
                case Entity.SPIKE:
                    SpawnPrefab(x, y, -1.5f, spikePrefab);
                    Destroy(_grounds[x, y]);
                    break;
                case Entity.TREASURE:
                    SpawnPrefab(x, y, 0.5f, treasurePrefab);
                    break;
            }
        }
    }

    GameObject SpawnPrefab(float x, float y, float z, GameObject prefab)
    {
        return Instantiate(prefab, new Vector3(
                x * spacing + (prefab == groundPrefab || prefab == spikePrefab ? 0f : 0.5f), 
                z, 
                y * spacing + (prefab == groundPrefab || prefab == spikePrefab ? 0f : 0.5f)),
            Quaternion.identity, worldObject.transform);
    }
    
    RPGWorld GetRPGWorld()
    {
        RPGWorld world = new RPGWorld();
        world.width = 4;
        world.height = 4;
        
        world._entities = new []
        {
            new Entity(0, 0, Entity.ACTOR), 
            new Entity(0, 3, Entity.TREASURE), 
            new Entity(1, 2, Entity.BARRIER), 
            new Entity(2, 3, Entity.SPIKE),  
            new Entity(2, 2, Entity.WALL), 
        };

        return world;
    }
}
