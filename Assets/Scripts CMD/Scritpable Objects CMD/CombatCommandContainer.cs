﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CombatCommandContainer : ScriptableObject {

    public string commandName;
    public GameObject commandPrefab;

    public void Init(GameObject o)
    {
        
        commandPrefab.GetComponent<ICommandable>().spawnCommand(o, commandName);

    }

}


