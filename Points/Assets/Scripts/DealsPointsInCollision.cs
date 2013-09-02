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

    void OnTriggerEnter(Collider collider)
    {
        HasPoints points = collider.transform.Require<HasPoints>();

        if (points.CanReceive(source))
        {
            points.Deal(source, amount);

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
