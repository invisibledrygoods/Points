using UnityEngine;
using System.Collections.Generic;
using Require;

public class BlinksWhenPointsAreNonzero : MonoBehaviour
{
    public string point;
    public float blinksPerSecond = 4.0f;

    Transform module;
    HasPoints points;
    HasVariableTimeScale time;

    float timeout;

    void Awake()
    {
        module = transform.GetModuleRoot();
        points = module.Require<HasPoints>();
        time = module.Require<HasVariableTimeScale>();
    }

    void Update()
    {
        if (points.Get(point) > 0.0f)
        {
            timeout -= time.DeltaTime("Gui");
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
