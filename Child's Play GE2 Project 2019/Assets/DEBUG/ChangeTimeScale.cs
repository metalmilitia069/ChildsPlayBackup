﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScale : MonoBehaviour
{
    [SerializeField] private float timeScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
    }
}
