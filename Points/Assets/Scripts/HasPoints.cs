using UnityEngine;
using System.Collections.Generic;
using System;
using Require;
using System.Linq;

[Serializable]
public class Point
{
    public string type;
    public float amount;
    public float max;
    public bool savable;

    public Point(string type, float amount)
    {
        this.type = type;
        this.amount = amount;
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
                if (amount < 0)
                {
                    point.amount = 0;
                }
                else if (amount > point.max && point.max != 0)
                {
                    point.amount = point.max;
                }
                else
                {
                    point.amount = amount;
                }

                return;
            }
        }

        points.Add(new Point(type, amount));
    }

    public bool Deal(string source, float amount)
    {
        bool successful = false;

        foreach (ReceivesPointsFromSource receiver in transform.GetComponents<ReceivesPointsFromSource>())
        {
            if (receiver.source == source)
            {
                successful = receiver.Deal(amount) || successful;
            }
        }

        return successful;
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
        foreach (ReceivesPointsFromSource receiver in transform.GetComponents<ReceivesPointsFromSource>())
        {
            if (receiver.source == source)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerable<Point> PointsThatCanReceive(string source)
    {
        HashSet<string> canReceive = new HashSet<string>();

        foreach (ReceivesPointsFromSource receiver in transform.GetComponents<ReceivesPointsFromSource>())
        {
            if (receiver.source == source)
            {
                canReceive.Add(receiver.type);
            }
        }

        foreach (Point point in points)
        {
            if (canReceive.Contains(point.type))
            {
                yield return point;
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
