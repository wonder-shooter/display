﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commons : MonoBehaviour {

    public static Vector3[][] CheckPoints = new Vector3[][]
    {
        new Vector3[]
        {
            new Vector3(-183.5f, 1f, -40f ),
            new Vector3(-183.5f, 1f, -1.5f), 
            new Vector3(-84f, 1f, -1.5f), 
            new Vector3(-84f, 1f, 47f), 
            new Vector3(-132f, 1f, 47f), 
            new Vector3(-132f, 1f, 54f), 
            new Vector3(-84f, 1f, 54f), 
            new Vector3(-84f, 1f, 160f), 
        }, 
    };

    public static float ChaserSpeed = 2f;
}
