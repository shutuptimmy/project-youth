using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


[System.Serializable]
public struct TimelineBinding
{
    public TrackAsset track;       // Use TrackAsset instead of PlayableAsset for tracks
    public UnityEngine.Object bindableObject;
}
public class cutsceneEvents
{
    public event Action<TimelineAsset, List<TimelineBinding>> onCutsceneTrigger;
    public void cutsceneTrigger(TimelineAsset cutscene, List<TimelineBinding> bindings)
    {
        onCutsceneTrigger?.Invoke(cutscene, bindings);
    }
}
