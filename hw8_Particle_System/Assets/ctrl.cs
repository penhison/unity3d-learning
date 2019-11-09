using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrl : MonoBehaviour
{
    ParticleSystem particleSystem;
    float time;
    ParticleSystem exhaust;

    void Start()
    {
        time = 0;
        particleSystem = GetComponent<ParticleSystem>();
    }

    [System.Obsolete]
    void Update()
    {
    }

    [System.Obsolete]
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 70, 30), "放大"))
        {
            particleSystem.startSize = particleSystem.startSize + 0.5f;
        }

        if (GUI.Button(new Rect(100, 200, 70, 30), "缩小"))
        {
            particleSystem.startSize = particleSystem.startSize - 0.5f;
        }

    }

}