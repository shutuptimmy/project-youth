using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialoguePlayableAsset : PlayableAsset
{
    // String for the Ink knot/stitch name
    public string knotName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph);
        DialogueControlBehaviour behaviour = playable.GetBehaviour();

        // Pass the knot name to the behaviour
        behaviour.knotName = knotName;

        return playable;
    }


    // public TextAsset inkJSON;

    // public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    // {
    //     // Create the PlayableBehaviour and attach it to the graph
    //     var playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph);
    //     DialogueControlBehaviour behaviour = playable.GetBehaviour();
    //     behaviour.inkJSON = inkJSON;
    //     behaviour.dialogueManager = owner.GetComponent<dialogueManager>();
    //     return playable;
    // }

    // expression body alternative method
    // public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) =>
    //     ScriptPlayable<DialogueControlBehaviour>.Create(graph, new DialogueControlBehaviour { inkJSON = inkJSON, dialogueManager = owner.GetComponent<dialogueManager>() });
}