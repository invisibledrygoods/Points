using UnityEngine;
using System.Collections;
using Require;

public class DealsPointWhenOtherPointIncreases : MonoBehaviour
{
    public string point;
    public string source;
    public float amount;

    ReceivesPointEvents pointEvents;
    Transform module;
    HasPoints points;

    void Awake()
    {
        module = transform.GetModuleRoot();
        points = module.Require<HasPoints>();
        pointEvents = module.Require<ReceivesPointEvents>();
    }

    void Start()
    {
        pointEvents.WaitFor(point).toRise.Then(() => points.Deal(source, amount));
    }
}
