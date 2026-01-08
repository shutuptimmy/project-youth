using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

// [TrackColor(0.855f, 0.862f, 0.870f)]
[TrackClipType(typeof(DialoguePlayableAsset))]
[TrackBindingType(typeof(dialogueManager))]
public class DialogueTrackAsset : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // The track doesn't need a mixer, so we return a dummy playable
        return Playable.Create(graph, inputCount);
    }
}