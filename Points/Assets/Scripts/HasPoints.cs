using UnityEngine;
using System.Collections.Generic;
using System;
using Require;

[Serializable]
public class Modifier
{
    public string source;
    public float modifier;
    public Transform effect;

    public Modifier(string source, float modifier)
    {
        this.source = source;
        this.modifier = modifier;
    }
}

[Serializable]
public class Point
{
    public string name;
    public float amount;
    public float max;
    public List<Modifier> modifiers;
    public List<Block> blocks;

    public Point(string name, float amount)
    {
        this.name = name;
        this.amount = amount;
        this.modifiers = new List<Modifier>();
    }
}

[Serializable]
public class Block
{
    public string source;
    public string to;

    public Block(string source, string to)
    {
        this.source = source;
        this.to = to;
    }
}

public class HasPoints : MonoBehaviour
{
    public List<Point> points;

    public void Set(string name, float amount)
    {
        points = points ?? new List<Point>();

        foreach (Point point in points)
        {
            if (point.name == name)
            {
                point.amount = amount;
                return;
            }
        }

        points.Add(new Point(name, amount));
    }
}
