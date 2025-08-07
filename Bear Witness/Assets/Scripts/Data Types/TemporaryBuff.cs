using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TemporaryBuff
{
    public enum BuffType
    {
        Resilience,
        Vigor,
        Fury,
        Integrity
    }

    public BuffType buff;
    public float endTime;

    public TemporaryBuff(BuffType newBuff, float newEndTime)
    {
        buff = newBuff;
        endTime = newEndTime;
    }
}
