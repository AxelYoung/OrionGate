using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Animation", menuName = "Easel Animation", order = 0)]
public class EaselClip : ScriptableObject {

    public string name;
    public Segment[] segments;
}

[System.Serializable]
public struct Segment {
    public Sprite[] sprites;
    public float length;

    public float frameLength { get { return sprites.Length / length; } }
}
