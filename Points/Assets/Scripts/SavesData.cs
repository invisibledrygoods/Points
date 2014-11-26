using UnityEngine;
using System;

public abstract class SavesData : MonoBehaviour
{
    public string fileSuffix;

    string filename;

    public abstract string Serialize();
    public abstract void Deserialize(string serialized);

    public void Save()
    {
        if (filename == "")
        {
            throw new Exception("cannot save without a filename");
        }

        Debug.Log("saving " + Serialize() + " as " + filename + "." + fileSuffix);

        PlayerPrefs.SetString(filename + "." + fileSuffix, Serialize());
    }

    public void Save(string filename)
    {
        this.filename = filename;
        Save();
    }

    public void Load()
    {
        if (filename == "")
        {
            throw new Exception("cannot load without a filename");
        }

        Debug.Log("loading " + PlayerPrefs.GetString(filename + "." + fileSuffix) + " from " + filename + "." + fileSuffix);

        Deserialize(PlayerPrefs.GetString(filename + "." + fileSuffix));
    }

    public void Load(string filename)
    {
        this.filename = filename;
        Load();
    }

    public void Delete()
    {
        if (filename == "")
        {
            throw new Exception("cannot delete without a filename");
        }

        PlayerPrefs.DeleteKey(filename + "." + fileSuffix);
    }

    public void Delete(string filename)
    {
        this.filename = filename;
        Delete();
    }
}
