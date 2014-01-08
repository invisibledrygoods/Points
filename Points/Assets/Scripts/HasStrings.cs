using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using Require;

[Serializable]
public class StringEntry
{
    public string name;
    public string text;
    public bool savable;

    public StringEntry(string name, string text)
    {
        this.name = name;
        this.text = text;
    }
}

public class HasStrings : SavesData
{
    public List<StringEntry> strings = new List<StringEntry>();

    public void Set(string name, string text)
    {
        foreach (StringEntry entry in strings)
        {
            if (entry.name == name)
            {
                entry.text = text;
                return;
            }
        }

        strings.Add(new StringEntry(name, text));
    }

    public string Get(string entry)
    {
        foreach (StringEntry currentEntry in strings)
        {
            if (currentEntry.name == entry)
            {
                return currentEntry.text;
            }
        }
        
        throw new KeyNotFoundException(name + " does not contain a string named " + entry);
    }

    public bool Has(string entry)
    {
        foreach (StringEntry currentEntry in strings)
        {
            if (currentEntry.name == entry)
            {
                return true;
            }
        }

        return false;
    }

    public string Interpolate(string source)
    {
        return Regex.Replace(source, @"#{(\.|)(.+?)}", _ =>
        {
            string entryName = _.Groups[2].Value;
            if (_.Groups[1].Value == ".")
            {
                HasPoints points = transform.Require<HasPoints>();

                if (points.Has(entryName))
                {
                    return "" + (int)points.Get(entryName);
                }
                else
                {
                    return _.Value;
                }
            }
            else
            {
                if (Has(entryName))
                {
                    return Get(entryName);
                }
                else
                {
                    return _.Value;
                }
            }
        });
    }

    public override string Serialize()
    {
        throw new NotImplementedException();
    }

    public override void Deserialize(string serialized)
    {
        throw new NotImplementedException();
    }
}
