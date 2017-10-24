using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameActionEvent : UnityEvent<GameActionEvent.EventType>
{
    public enum EventType
    {
        TitleSceneEnd,
        SearchModeSceneEnd,
        ChaserModeSceneEnd,
        GameEnd,
        ChaserModeCountStart,
        ChaserModeCountEnd,
    }
}
