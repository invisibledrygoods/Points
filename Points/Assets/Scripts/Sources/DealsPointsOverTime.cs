using UnityEngine;
using System.Collections.Generic;
using Require;

public class DealsPointsOverTime : MonoBehaviour
{
    public string source;
    public float interval;
    public float amount;

    Transform module;
    HasPoints points;

    float timeout;

    void Awake()
    {
        module = transform.GetModuleRoot();
        points = module.Require<HasPoints>();
    }

    void Start()
    {
        timeout = interval;
    }

    void Update()
    {
        timeout -= Time.deltaTime;

        while (timeout < 0)
        {
            timeout += interval;
            points.Deal(source, amount);
        }
    }
}
