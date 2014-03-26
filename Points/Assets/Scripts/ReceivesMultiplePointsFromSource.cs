using UnityEngine;
using System.Collections.Generic;
using Require;

public class ReceivesMultiplePointsFromSource : ReceivesPointsFromSource
{
    [System.Serializable]
    public class Modifier
    {
        public string type;
        public float modifier;
    }

    public Modifier[] modifiers;

    void Awake()
    {
        points = transform.Require<HasPoints>();
    }

    public override bool Deal(float amount)
    {
        foreach (Modifier modifier in modifiers)
        {
            points.Set(modifier.type, points.Get(modifier.type) + amount * modifier.modifier);

            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }

        return true;
    }
}
