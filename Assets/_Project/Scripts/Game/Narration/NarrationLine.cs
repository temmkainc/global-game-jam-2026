using System;
using UnityEngine;

[Serializable]
public class NarrationLine
{
    [TextArea]
    public string Subtitle;

    [Min(0f)]
    public float Duration = 2f;

    [Min(0f)]
    public float DelayBefore = 0f;
}
