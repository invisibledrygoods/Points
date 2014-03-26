using UnityEngine;
using System.Collections;
using Require;

public class DisplaysPointsAsMarkers : MonoBehaviour
{
    public Transform[] markers;
    public string type = "";
    public float pointsPerMarker = 1.0f;
    public Transform addEffect;
    public Transform loseEffect;
    public float blinkThreshold = 0;

    private HasPoints points;

    void Awake()
    {
        points = transform.Require<HasPoints>();
    }

    void Start()
    {
    }

    void Update()
    {
        for (int i = 0; i < (int)(points.Get(type) / pointsPerMarker) && i < markers.Length; i++)
        {
            if (markers[i].gameObject.activeSelf == false)
            {
                if (addEffect != null)
                {
                    Instantiate(addEffect, markers[i].transform.position, Quaternion.identity);
                }
                markers[i].gameObject.SetActive(true);
            }
        }

        for (int i = (int)(points.Get(type) / pointsPerMarker); i < markers.Length; i++)
        {
            if (markers[i].gameObject.activeSelf == true)
            {
                if (loseEffect != null)
                {
                    Instantiate(loseEffect, markers[i].transform.position, Quaternion.identity);
                }
                markers[i].gameObject.SetActive(false);
            }
        }

        foreach (Transform currentMarker in markers)
        {
            if (Mathf.Repeat(Time.time, 0.6f) > 0.3f && points.Get(type) < blinkThreshold)
            {
                foreach (Renderer renderer in currentMarker.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
            else
            {
                foreach (Renderer renderer in currentMarker.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }
        }
    }
}
