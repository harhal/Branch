using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Human {
    public int ID;
    public string Name;
    public enum SexType { Male, Female, NoMatter };
    public SexType Sex;
    public enum ProfessionType { Agent, Operative, Scientist, D_Personnel }
    public ProfessionType Profession;
    public enum ActivityType { Free, Going, Working, Blocked };
    public ActivityType Activity;
    //public float WalkingProgress;
    public Vector3 Location;
    [SerializeField]
    string photo;
    public int Level = 1;
    public float Experience;
    public string Biography;

    [System.NonSerialized]
    Sprite _photo;
    public Sprite Photo
    {
        get
        {
            if (_photo == null)
                _photo = Resources.Load<Sprite>(photo);
            return _photo;
        }
    }
    [System.NonSerialized]
    public object Destination;
    
    public Human(string Name, SexType Sex, string Photo)
    {
        this.Name = Name;
        this.Sex = Sex;
        this.photo = Photo;
    }

    public void Register()
    {
        SessionData.Data.ResourceStorage.Hire(this);
        SendTo(null);
    }

    public void Hire(object destination)
    {
        SessionData.Data.ResourceStorage.Send(this);
        SendTo(destination);
    }

    public void Fire()
    {
        SessionData.Data.ResourceStorage.Return(this);
        SendTo(null);
    }

    public void Kill()
    {
        SessionData.Data.ResourceStorage.Kill(this);
    }

    protected void SendTo(object destination)
    {
        Destination = destination;
        Activity = ActivityType.Going;
    }

    public void Arrived()
    {
        if (Destination == null)
            Activity = ActivityType.Free;
        else
            Activity = ActivityType.Working;
    }
}
