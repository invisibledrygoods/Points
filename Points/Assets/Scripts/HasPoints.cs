using UnityEngine;
using System.Collections.Generic;
using System;
using Require;
using System.Linq;

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
    public string type;
    public float amount;
    public float max;
    public List<Modifier> modifiers;
    public List<Block> blocks;

    public Point(string type, float amount)
    {
        this.type = type;
        this.amount = amount;
        this.modifiers = new List<Modifier>();
        this.blocks = new List<Block>();
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

    public void Set(string type, float amount)
    {
        points = points ?? new List<Point>();

        foreach (Point point in points)
        {
            if (point.type == type)
            {
                point.amount = amount;
                return;
            }
        }

        points.Add(new Point(type, amount));
    }

    internal void SetModifier(string type, string source, float modifier)
    {
        foreach (Point point in points)
        {
            if (point.type == type)
            {
                foreach (Modifier mod in point.modifiers)
                {
                    if (mod.source == source)
                    {
                        mod.modifier = modifier;
                        return;
                    }
                }

                point.modifiers.Add(new Modifier(source, modifier));
                return;
            }
        }
        throw new KeyNotFoundException(name + " does not have any " + type);
    }

    internal void Deal(string source, float amount)
    {
        foreach (Point point in points)
        {
            foreach (Modifier mod in point.modifiers.Where(i => i.source == source))
            {
                foreach (Block block in points.Where(i => i.amount > 0).SelectMany(i => i.blocks))
                {
                    if (block.source == source && block.to == point.type)
                    {
                        return;
                    }
                }

                point.amount = Mathf.Clamp(point.amount + mod.modifier * amount, 0, point.max);

                if (mod.effect)
                {
                    Instantiate(mod.effect, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
