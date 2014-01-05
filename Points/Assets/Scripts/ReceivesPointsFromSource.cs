using UnityEngine;
using System.Collections.Generic;
using Require;

public class ReceivesPointsFromSource : MonoBehaviour
{
    public string type;
    public string source;
    public float modifier;
    public Transform effect;

    protected HasPoints points;

    void Awake()
    {
        points = transform.Require<HasPoints>();
    }

    public virtual bool Deal(float amount)
    {
        points.Set(type, points.Get(type) + amount * modifier);

        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        return true;
    }
}
