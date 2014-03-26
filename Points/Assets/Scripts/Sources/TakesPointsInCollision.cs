using UnityEngine;
using System.Collections;
using Require;

public class TakesPointsInCollision : MonoBehaviour
{
    public string type;

    HasPoints points;

    void Awake()
    {
        points = transform.Require<HasPoints>();

        Receives2DCollisionEvents collisions2D = transform.GetComponent<Receives2DCollisionEvents>();
        if (collisions2D != null)
        {
            collisions2D.waitForCollision.ThenAlways(_ =>
                {
                    DealPoints(_);
                });
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        DealPoints(collider.transform);
    }

    void DealPoints(Transform collidedWith)
    {
        HasPoints otherPoints = collidedWith.GetModuleRoot().GetComponent<HasPoints>();

        if (otherPoints && otherPoints.Has("Day"))
        {
            points.Set(type, otherPoints.Get(type));
        }
    }
}
