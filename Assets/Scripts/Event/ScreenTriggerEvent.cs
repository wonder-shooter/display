using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScreenTriggerEvent : UnityEvent<Player.ColorType, Vector3> {
}
