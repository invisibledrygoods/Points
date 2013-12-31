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
    public bool savable;
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

public class HasPoints : SavesData
{
    public List<Point> points = new List<Point>();

    public void Set(string type, float amount)
    {
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

    public void SetModifier(string type, string source, float modifier)
    {
        Point point = GetPoint(type);

        foreach (Modifier mod in GetPoint(type).modifiers)
        {
            if (mod.source == source)
            {
                mod.modifier = modifier;
                return;
            }
        }

        point.modifiers.Add(new Modifier(source, modifier));
    }

    public void Deal(string source, float amount)
    {
        foreach (Point point in PointsThatCanReceive(source))
        {
            foreach (Modifier mod in point.modifiers.Where(i => i.source == source))
            {
                float max = point.max;
                if (max == 0)
                {
                    max = float.MaxValue;
                }

                point.amount = Mathf.Clamp(point.amount + mod.modifier * amount, 0, max);

                if (mod.effect)
                {
                    Instantiate(mod.effect, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public bool Has(string type)
    {
        foreach (Point point in points)
        {
            if (point.type == type)
            {
                return true;
            }
        }

        return false;
    }

    public bool CanReceive(string source)
    {
        return PointsThatCanReceive(source).Count() > 0;
    }

    public IEnumerable<Point> PointsThatCanReceive(string source)
    {
        foreach (Point point in points)
        {
            foreach (Modifier mod in point.modifiers.Where(i => i.source == source))
            {
                bool blocked = false;

                foreach (Block block in points.Where(i => i.amount > 0).SelectMany(i => i.blocks))
                {
                    if (block.source == source && block.to == point.type)
                    {
                        blocked = true;
                    }
                }

                if (!blocked)
                {
                    yield return point;
                }
            }
        }
    }

    public float Get(string type)
    {
        foreach (Point point in points)
        {
            if (point.type == type)
            {
                return point.amount;
            }
        }

        throw new KeyNotFoundException(name + " does not have any " + type);
    }

    public float GetMax(string type)
    {
        foreach (Point point in points)
        {
            if (point.type == type)
            {
                return point.max;
            }
        }

        throw new KeyNotFoundException(name + " does not have any " + type);
    }

    public Point GetPoint(string type)
    {
        foreach (Point point in points)
        {
            if (point.type == type)
            {
                return point;
            }
        }

        throw new KeyNotFoundException(name + " does not have any " + type);
    }

    public void SetMax(string type, float amount)
    {
        GetPoint(type).max = amount;
    }

    public void SetBlock(string type, string source, string toType)
    {
        GetPoint(type).blocks.Add(new Block(source, toType));
    }

    public void SetSavable(string type, bool savable)
    {
        GetPoint(type).savable = savable;
    }

    public bool GetSavable(string type)
    {
        return GetPoint(type).savable;
    }

    public override string Serialize()
    {
        List<string> serialized = new List<string>();

        foreach (Point point in points)
        {
            if (point.savable)
            {
                if (point.max == 0)
                {
                    serialized.Add(point.type + ":" + point.amount);
                }
                else
                {
                    serialized.Add(point.type + ":" + point.amount + "/" + point.max);
                }
            }
        }

        return string.Join(",", serialized.ToArray());
    }

    public override void Deserialize(string serialized)
    {
        if (serialized == "")
        {
            return;
        }

        foreach (string savedValue in serialized.Split(','))
        {
            string[] keyValuePair = savedValue.Split(":/".ToCharArray());

            if (GetSavable(keyValuePair[0]))
            {
                Set(keyValuePair[0], float.Parse(keyValuePair[1]));

                if (keyValuePair.Length == 3)
                {
                    SetMax(keyValuePair[0], float.Parse(keyValuePair[2]));
                }
            }
        }
    }
}
