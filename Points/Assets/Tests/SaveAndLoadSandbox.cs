using UnityEngine;
using System.Collections.Generic;
using Require;

public class SaveAndLoadSandbox : MonoBehaviour
{
    HasPoints points;
    HasFlags flags;

    void Awake()
    {
        points = transform.Require<HasPoints>();
        flags = transform.Require<HasFlags>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                points.Load("test");
                flags.Load("test");
            }
            else
            {
                points.Save("test");
                flags.Save("test");
            }
        }
    }
}
