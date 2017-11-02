using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker {
    public enum DeviceType
    {
        LeftController,
        RightController,
        PinkTracker,
        GreenTracker,
        PurpleTracker,
    }

    private Tracker() { }
    public Tracker(DeviceType type, Vector3 position) {
        this.Type = type;
        this.Position = position;
    }

    public DeviceType Type{
        get; private set;
    }

    public Vector3 Position
    {
        get; private set;
    }
}
