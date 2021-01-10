using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public const string ACTOR = "ACTOR";
    public const string WALL = "WALL";
    public const string SPIKE = "SPIKE";
    public const string BARRIER = "BARRIER";
    public const string GROUND = "GROUND";
    public const string TREASURE = "TREASURE";
    public int x, y;
    public string type;
    public bool active;
    public GameObject obj;

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

// public class SEntity    {
//     public int x { get; set; } 
//     public int y { get; set; } 
//     public string type { get; set; } 
// }
//
// public class SLevel    {
//     public int width { get; set; } 
//     public int height { get; set; } 
//     public List<SEntity> entities { get; set; } 
// }
//
// public class Root    {
//     public SLevel level { get; set; } 
//     public List<List<string>> actions { get; set; } 
// }

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
    private Entity _actor;
    public Level level;
    // Start is called before the first frame update
    void Start()
    {
        level = GetData.getBuild();
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
        for (int i = 0; i < _world.width; i++)
        {
            for (int j = 0; j < _world.height; j++)
            {
                _grounds[i, j] = SpawnPrefab(i, j, 0, groundPrefab);
            }
        }

        for (int i = 0; i < _world._entities.Length; i++)
        {
            Entity e = _world._entities[i];
            int x = e.x, y = e.y;
            switch (e.type)
            {
                case Entity.ACTOR:
                    e.obj = SpawnPrefab(x, y, 0.55f, actorPrefab);
                    _actor = e;
                    break;
                case Entity.WALL:
                    e.obj = SpawnPrefab(x, y, 1, wallPrefab);
                    break;
                case Entity.BARRIER:
                    e.obj = SpawnPrefab(x, y, 0, barrierPrefab);
                    break;
                case Entity.SPIKE:
                    e.obj = SpawnPrefab(x, y, -1.5f, spikePrefab);
                    Destroy(_grounds[x, y]);
                    break;
                case Entity.TREASURE:
                    e.obj = SpawnPrefab(x, y, 0.5f, treasurePrefab);
                    break;
            }

            e.active = true;
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
        Entities[] entities = level.entities;
        RPGWorld world = new RPGWorld();
        world.width = level.width;
        world.height = level.height;
        world._entities = new Entity[entities.Length];
        for (int i = 0; i < entities.Length; i++)
        {            
            world._entities[i] = new Entity(x: entities[i].x, y:entities[i].y, type:entities[i].type);
        }
        // world.width = 4;
        // world.height = 4;
        //
        // world._entities = new []
        // {
            // new Entity(0, 0, Entity.ACTOR), 
            // new Entity(0, 3, Entity.TREASURE), 
            // new Entity(1, 2, Entity.BARRIER), 
            // new Entity(2, 3, Entity.SPIKE),  
            // new Entity(2, 2, Entity.WALL), 
            
            // new Entity(0, 0, Entity.ACTOR), 
            // new Entity(1, 1, Entity.BARRIER), 
            //
            // new Entity(0, 0, Entity.ACTOR), 
            // new Entity(0, 1, Entity.BARRIER), 
            // new Entity(1, 1, Entity.SPIKE), 
            // new Entity(2, 0, Entity.BARRIER), 
            // new Entity(0, 2, Entity.BARRIER), 
        // };

        return world;
    }

    // Find first entity encoutered
    public Entity FindEntity(int x, int y)
    {
        foreach (Entity e in _world._entities)
        {
            if (e.x == x && e.y == y && e.active)
            {
                return e;
            }
        }

        return null;
    }

    public void RemoveEntity(Entity e)
    {
        e.active = false;
    }
    
    public Entity GetActorEntity()
    {
        return _actor;
    }
}
