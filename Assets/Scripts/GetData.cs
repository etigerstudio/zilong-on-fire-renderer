﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class Data
{
    public Level level;
    public string[] actions;
}

public class Level
{
    public int width;
    public int height;
    public Entities[] entities;
}

public class Entities
{
    public  int x;
    public  int y;
    public  string type;
}

// public class Actions
// {
//     public static string[] actions;
// }

public class GetData
{

    public static Level getBuild()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Levels/data.json");

        Data data = JsonConvert.DeserializeObject<Data>(jsonString);
        Level level = data.level;
        return level;
    }
    
    public static string[] getActions()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Levels/data.json");
        Data data = JsonConvert.DeserializeObject<Data>(jsonString);
        string[] actions = data.actions;
        return actions;
    }

}
