using UnityEngine;
using System.Collections.Generic;
using Require;

public class ReceivesPointsUnlessItHas : ReceivesPointsFromSource
{
    public string unlessItHasPointsIn;

    void Awake()
    {
        points = transform.Require<HasPoints>();
    }

    public override bool Deal(float amount)
    {
        if (points.Get(unlessItHasPointsIn) == 0)
        {
            return base.Deal(amount);
        }

        return false;
    }
}
