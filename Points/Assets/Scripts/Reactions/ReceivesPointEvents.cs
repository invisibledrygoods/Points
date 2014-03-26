using UnityEngine;
using System.Collections.Generic;
using Require;

public class ReceivesPointEvents : MonoBehaviour
{
    public class PointEventListener
    {
        public WaitFor toFall = new WaitFor();
        public WaitFor toRise = new WaitFor();
        public WaitFor toReachMax = new WaitFor();
        public WaitFor toReachZero = new WaitFor();

        Dictionary<float, WaitFor> fallingTriggers = new Dictionary<float, WaitFor>();
        Dictionary<float, WaitFor> risingTriggers = new Dictionary<float, WaitFor>();

        public WaitFor ToFallBelow(float threshold)
        {
            if (!fallingTriggers.ContainsKey(threshold))
            {
                fallingTriggers[threshold] = new WaitFor();
            }

            return fallingTriggers[threshold];
        }

        public WaitFor ToRiseAbove(float threshold)
        {
            if (!risingTriggers.ContainsKey(threshold))
            {
                risingTriggers[threshold] = new WaitFor();
            }

            return risingTriggers[threshold];
        }

        public void Compare(float previous, float current, float max)
        {
            if (previous < current)
            {
                toRise.Happened();
            }

            if (previous > current)
            {
                toFall.Happened();
            }

            if (current == 0.0f)
            {
                toReachZero.Happened();
            }

            if (current == max)
            {
                toReachMax.Happened();
            }

            foreach (KeyValuePair<float, WaitFor> risingTrigger in risingTriggers)
            {
                if (previous <= risingTrigger.Key && current > risingTrigger.Key)
                {
                    risingTrigger.Value.Happened();
                }
            }

            foreach (KeyValuePair<float, WaitFor> fallingTrigger in fallingTriggers)
            {
                if (previous >= fallingTrigger.Key && current < fallingTrigger.Key)
                {
                    fallingTrigger.Value.Happened();
                }
            }
        }
    }

    Dictionary<string, float> previousValues = new Dictionary<string, float>();
    Dictionary<string, PointEventListener> listeners = new Dictionary<string, PointEventListener>();

    Transform module;
    HasPoints points;

    void Awake()
    {
        module = transform.GetModuleRoot();
        points = module.Require<HasPoints>();
    }

    void Start()
    {
        foreach (Point point in points.points)
        {
            previousValues[point.type] = point.amount;
        }
    }

    void Update()
    {
        foreach (KeyValuePair<string, PointEventListener> listener in listeners)
        {
            listener.Value.Compare(previousValues[listener.Key], points.Get(listener.Key), points.GetMax(listener.Key));
        }

        foreach (Point point in points.points)
        {
            previousValues[point.type] = point.amount;
        }
    }

    public PointEventListener WaitFor(string type)
    {
        if (!listeners.ContainsKey(type))
        {
            listeners[type] = new PointEventListener();
        }

        return listeners[type];
    }
}
