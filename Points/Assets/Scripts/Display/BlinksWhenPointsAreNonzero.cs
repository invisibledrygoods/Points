using UnityEngine;
using System.Collections.Generic;
using Require;

public class BlinksWhenPointsAreNonzero : MonoBehaviour
{
    public string point;
    public float blinksPerSecond = 4.0f;

    Transform module;
    HasPoints points;

    float timeout;

    void Awake()
    {
        module = transform.GetModuleRoot();
        points = module.Require<HasPoints>();
    }

    void Update()
    {
        if (points.Get(point) > 0.0f)
        {
            timeout -= Time.deltaTime;
            renderer.enabled = timeout < 0.5f / blinksPerSecond;

            if (timeout < 0.0f)
            {
                timeout = 1.0f / blinksPerSecond;
            }
        }
        else
        {
            renderer.enabled = true;
        }
    }
}
