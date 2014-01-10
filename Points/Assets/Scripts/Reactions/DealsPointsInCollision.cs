using UnityEngine;
using System.Collections;
using Require;

public class DealsPointsInCollision : MonoBehaviour
{
    public enum AfterDealing
    {
        DoNothing,
        Destroy,
        Deactivate
    }

    public string source;
    public float amount;
    public AfterDealing afterDealing;

    void Awake()
    {
        Receives2DCollisionEvents collisions2D = transform.GetComponent<Receives2DCollisionEvents>();
        if (collisions2D != null)
        {
            collisions2D.waitForCollision.Then(_ =>
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
        HasPoints points = collidedWith.Require<HasPoints>();

        if (points.Deal(source, amount))
        {
            if (afterDealing == AfterDealing.Deactivate)
            {
                gameObject.SetActive(false);
            }
            else if (afterDealing == AfterDealing.Destroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
