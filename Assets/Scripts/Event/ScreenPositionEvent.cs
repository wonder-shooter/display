using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScreenPositionEvent : UnityEvent<Player.ColorType, Vector3> {}