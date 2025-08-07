using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunSegment
{
    public SerializableDictionary<float, string> timesOfInterest = new();
    public string startBench;
    public string endBench;

    public float startTime;
    public float elapsedTime;
    public float damageTaken;
    public float sandLost;

    public void AddPOI(float time, string type)
    {
        if (!GameManager.instance.inArktis)
            timesOfInterest.Add(time, type);
    }

    public RunSegment Stitch(RunSegment segmentA, RunSegment segmentB)
    {
        RunSegment newSegment = new();

        newSegment.startBench = segmentA.startBench;
        newSegment.endBench = segmentB.endBench;
        newSegment.elapsedTime = segmentA.elapsedTime + segmentB.elapsedTime;
        newSegment.sandLost = segmentA.sandLost + segmentB.sandLost;
        newSegment.damageTaken = segmentA.damageTaken + segmentB.damageTaken;

        newSegment.timesOfInterest = segmentA.timesOfInterest;
        foreach (float time in segmentB.timesOfInterest.Keys)
        {
            newSegment.timesOfInterest.Add(time + segmentA.elapsedTime, segmentB.timesOfInterest[time]);
        }

        return newSegment;
    }

    public bool CanStitch(RunSegment segmentA, RunSegment segmentB)
    {
        if (segmentA.sandLost + segmentB.sandLost >= GameManager.instance.hourglassCapacity)
        {
            return false;  // too much time elapsed!
        }

        if (segmentA.damageTaken + segmentB.damageTaken >= GameManager.instance.playerMaxHealth)
        {
            return false;  // too much damage taken! I might allow this later but it's too complicated for now.
        }

        return true;
    }

    public RunSegment Splice(RunSegment segment, string benchA, string benchB, int timesVisitedFirstBench = 1)
    {
        RunSegment newSegment = new();

        if (!segment.timesOfInterest.ContainsValue(benchA))
        {
            Debug.LogError("Starting Bench Not Found");
            return null;
        }

        if (!segment.timesOfInterest.ContainsValue(benchB))
        {
            Debug.LogError("Ending Bench Not Found");
            return null;
        }

        newSegment.startBench = benchA;
        newSegment.endBench = benchB;

        float timeMultiplier = 1f;
        bool inSegment = false;
        float startTime = 0f;
        float lastTime = 0f;
        int runningBenchVisits = 0;

        foreach (float time in segment.timesOfInterest.Keys)
        {
            if (segment.timesOfInterest[time] == "Shatter")
            {
                timeMultiplier /= GameManager.shatterMultiplier;
            }

            if (segment.timesOfInterest[time] == benchA)
            {
                runningBenchVisits++;
                if (runningBenchVisits == timesVisitedFirstBench)
                {
                    startTime = time;
                    inSegment = true;
                }
            }

            if (inSegment)
            {
                newSegment.timesOfInterest.Add(time, segment.timesOfInterest[time]);
                newSegment.sandLost += (time - lastTime) * timeMultiplier;

                if (segment.timesOfInterest[time] == "Damage" || segment.timesOfInterest[time] == "Shatter")
                {
                    newSegment.damageTaken++;
                }
            }

            if (inSegment && segment.timesOfInterest[time] == benchB)
            {
                newSegment.elapsedTime = time - startTime;
                return newSegment;
            }

            lastTime = time;
        }

        Debug.LogError("Splice Construction Failed");
        return null;
    }   
}
