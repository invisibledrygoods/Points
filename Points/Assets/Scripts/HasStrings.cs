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

    public StringEntry GetEntry(string name)
    {
        foreach (StringEntry currentEntry in strings)
        {
            if (currentEntry.name == name)
            {
                return currentEntry;
            }
        }

        return null;
    }

    public void SetSavable(string name, bool value)
    {
        GetEntry(name).savable = value;
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

    public bool GetSavable(string name)
    {
        StringEntry entry = GetEntry(name);
        return entry == null ? false : entry.savable;
    }

    string Encode(string input)
    {
        return input.Replace("&", "&amp;").Replace(":", "&colon;").Replace(",", "&comma;");
    }

    string Decode(string input)
    {
        return input.Replace("&comma;", ",").Replace("&colon;", ":").Replace("&amp;", "&"); 
    }

    public override string Serialize()
    {
        List<string> serialized = new List<string>();

        foreach (StringEntry currentEntry in strings)
        {
            if (currentEntry.savable)
            {
                serialized.Add(currentEntry.name + ":" + Encode(currentEntry.text));
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
            string[] keyValuePair = savedValue.Split(":".ToCharArray());

            if (GetSavable(keyValuePair[0]))
            {
                Set(keyValuePair[0], Decode(keyValuePair[1]));
            }
        }
    }
}
