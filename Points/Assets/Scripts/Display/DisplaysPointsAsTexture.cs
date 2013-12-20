using UnityEngine;
using System;
using Require;
using System.Collections.Generic;

public class DisplaysPointsAsTexture : MonoBehaviour
{
    [Serializable]
    public class TextureInfo
    {
        public float threshold;
        public float blinkThreshold;
        public Material material;
    }

    public List<TextureInfo> thresholds;
    public string pointType;
    public Renderer[] renderers;
    public bool alsoSetChildRenderers;
    public Transform addEFfect;
    public Transform loseEffect;

    HasPoints points;
    Material lastMaterial;
    int lastThreshold;

    void Awake()
    {
        points = transform.Require<HasPoints>();
        thresholds.Sort((a, b) => a.threshold.CompareTo(b.threshold));
    }

    void Update()
    {
        if (thresholds.Count == 0)
        {
            return;
        }

        float p = points.Get(pointType);
        int currentThreshold = 0;
        Material material = thresholds[0].material;

        for (int i = 0; i < thresholds.Count; i++)
        {
            if (p >= thresholds[i].threshold)
            {
                if (p < thresholds[i].blinkThreshold)
                {
                    if (Mathf.Repeat(Time.time, 0.4f) > 0.2f)
                    {
                        material = thresholds[i - 1].material;
                    }
                    else
                    {
                        material = thresholds[i].material;
                    }
                }
                else
                {
                    material = thresholds[i].material;
                }

                currentThreshold = i;
            }
        }

        if (material != lastMaterial)
        {
            foreach (Renderer renderer in renderers)
            {
                if (alsoSetChildRenderers)
                {
                    foreach (Renderer childRenderer in renderer.GetComponentsInChildren<Renderer>())
                    {
                        childRenderer.material = material;
                    }
                }
                else
                {
                    renderer.material = material;
                }
            }
        }

        if (currentThreshold < lastThreshold)
        {
            if (addEFfect != null)
            {
                Instantiate(addEFfect, transform.position, Quaternion.identity);
            }
        }
        else if (currentThreshold > lastThreshold)
        {
            if (loseEffect != null)
            {
                Instantiate(loseEffect, transform.position, Quaternion.identity);
            }
        }

        lastMaterial = material;
        lastThreshold = currentThreshold;
    }
}
