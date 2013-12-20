using UnityEngine;
using System.Collections;
using Require;

public class ReceivesPointsInCollision : MonoBehaviour
{
    public string source;
    public float amount;
    public string ifColliderCanReceive;

    Transform module;

    void Awake()
    {
        module = transform.GetModuleRoot();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.Require<HasPoints>().CanReceive(ifColliderCanReceive))
        {
            module.Require<HasPoints>().Deal(source, amount);
        }
    }
}
