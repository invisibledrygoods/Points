using UnityEngine;
using System.Collections;
using Require;

public class DisplaysPointsAsBar : MonoBehaviour
{
    public Transform leftBracket;
    public Transform barPrefab;
    public Transform rightBracket;
    public Vector3 distanceBetweenBars;
    public string type;

    Transform module;
    HasPoints points;

    void Awake()
    {
        module = transform.GetModuleRoot();
        points = module.Require<HasPoints>();
    }

    void Update()
    {
        for (int i = 1; i <= Mathf.RoundToInt(points.Get(type)); i++)
        {
            if (i >= leftBracket.childCount)
            {
                Transform newBar = Instantiate(barPrefab) as Transform;
                newBar.parent = leftBracket;
                newBar.localPosition = i * distanceBetweenBars;
                newBar.localScale = Vector3.one;
            }
        }

        for (int i = Mathf.RoundToInt(points.Get(type)); i < leftBracket.childCount; i++)
        {
            Destroy(leftBracket.GetChild(i).gameObject);
        }

        Transform oldParent = rightBracket.parent;
        rightBracket.parent = leftBracket;
        rightBracket.localPosition = ((int)points.GetMax(type) + 1) * distanceBetweenBars;
        rightBracket.parent = oldParent;
    }
}
