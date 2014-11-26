using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Require;

public class HasFlags : SavesData
{
    [Serializable]
    public class Flag
    {
        public string name;
        public bool raised;
        public bool savable;

        public Flag(string name, bool raised)
        {
            this.name = name;
            this.raised = raised;
        }
    }

    public List<Flag> flags = new List<Flag>();

    public void Lower(string flag)
    {
        foreach (Flag currentFlag in flags)
        {
            if (currentFlag.name == flag)
            {
                currentFlag.raised = false;
                return;
            }
        }

        flags.Add(new Flag(flag, false));
    }

    public void Raise(string flag)
    {
        foreach (Flag currentFlag in flags)
        {
            if (currentFlag.name == flag)
            {
                currentFlag.raised = true;
                return;
            }
        }

        flags.Add(new Flag(flag, true));
    }

    public bool Get(string flag)
    {
        foreach (Flag currentFlag in flags)
        {
            if (currentFlag.name == flag)
            {
                return currentFlag.raised;
            }
        }

        throw new KeyNotFoundException(name + " does not contain a flag named " + flag);
    }

    public bool GetOrFalse(string flag)
    {
        foreach (Flag currentFlag in flags)
        {
            if (currentFlag.name == flag)
            {
                return currentFlag.raised;
            }
        }

        return false;
    }

    public void SetSavable(string flag, bool savable)
    {
        foreach (Flag currentFlag in flags)
        {
            if (currentFlag.name == flag)
            {
                currentFlag.savable = savable;
                return;
            }
        }

        throw new KeyNotFoundException(name + " does not contain a flag named " + flag);
    }

    public bool GetSavable(string flag)
    {
        foreach (Flag currentFlag in flags)
        {
            if (currentFlag.name == flag)
            {
                return currentFlag.savable;
            }
        }

        throw new KeyNotFoundException(name + " does not contain a flag named " + flag);
    }

    public override string Serialize()
    {
        List<string> serialized = new List<string>();

        foreach (Flag flag in flags)
        {
            if (flag.savable)
            {
                serialized.Add(flag.name + ":" + flag.raised);
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
            string[] keyValuePair = savedValue.Split(':');

            if (GetSavable(keyValuePair[0]))
            {
                if (keyValuePair[1].ToLower() == "false")
                {
                    Lower(keyValuePair[0]);
                }
                else
                {
                    Raise(keyValuePair[0]);
                }
            }
        }
    }
}
