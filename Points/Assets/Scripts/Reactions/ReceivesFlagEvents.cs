using UnityEngine;
using System.Collections.Generic;
using Require;

public class ReceivesFlagEvents : MonoBehaviour
{
    public class FlagEventListener
    {
        public WaitFor toRaise = new WaitFor();
        public WaitFor toLower = new WaitFor();

        public void Compare(bool previous, bool current)
        {
            if (previous && !current)
            {
                toLower.Happened();
            }

            if (!previous && current)
            {
                toRaise.Happened();
            }
        }
    }

    Dictionary<string, bool> previousValues = new Dictionary<string, bool>();
    Dictionary<string, FlagEventListener> listeners = new Dictionary<string, FlagEventListener>();

    Transform module;
    HasFlags flags;

    void Awake()
    {
        module = transform.GetModuleRoot();
        flags = module.Require<HasFlags>();
    }

    void Update()
    {
        foreach (KeyValuePair<string, FlagEventListener> listener in listeners)
        {
            if (!previousValues.ContainsKey(listener.Key))
            {
                previousValues[listener.Key] = flags.Get(listener.Key);
            }

            listener.Value.Compare(previousValues[listener.Key], flags.Get(listener.Key));
        }

        foreach (HasFlags.Flag flag in flags.flags)
        {
            previousValues[flag.name] = flag.raised;
        }
    }

    public FlagEventListener WaitFor(string name)
    {
        if (!listeners.ContainsKey(name))
        {
            listeners[name] = new FlagEventListener();
        }

        return listeners[name];
    }
}
