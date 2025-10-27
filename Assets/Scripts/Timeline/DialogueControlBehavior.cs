using UnityEngine;
using UnityEngine.Playables;

public class DialogueControlBehaviour : PlayableBehaviour
{
    // The Ink knot name (or entry point) to start the dialogue
    public string knotName;

    private bool dialogueStarted = false;
    private PlayableGraph playableGraph;

    public override void OnPlayableCreate(Playable playable)
    {
        // Store the PlayableGraph when the playable is created
        playableGraph = playable.GetGraph();
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // Only run once when the clip is active
        if (!dialogueStarted)
        {
            // IMPORTANT: Check if the graph is valid before using it
            if (playableGraph.IsValid())
            {
                // 1. Pause the Timeline using the stored graph reference
                playableGraph.GetRootPlayable(0).SetSpeed(0);

                // 2. Fire the event to start the dialogue
                gameEventsManager.instance.dialogueEvents.enterDialogue(knotName);

                // 3. Subscribe to the event that signals the dialogue is finished
                gameEventsManager.instance.dialogueEvents.onDialogueFinished += ResumeTimeline;
            }
            else
            {
                Debug.LogError("PlayableGraph is not valid when trying to start dialogue.");
            }

            dialogueStarted = true;
        }
    }

    // Called when the playable is stopped/destroyed (e.g., when the Timeline ends)
    public override void OnPlayableDestroy(Playable playable)
    {
        // Always unsubscribe to prevent memory leaks
        gameEventsManager.instance.dialogueEvents.onDialogueFinished -= ResumeTimeline;
    }

    // This method is the event handler called by gameEventsManager when dialogue exits
    private void ResumeTimeline()
    {
        // 1. Unsubscribe immediately 
        gameEventsManager.instance.dialogueEvents.onDialogueFinished -= ResumeTimeline;

        // 2. Resume the timeline using the stored graph reference
        if (playableGraph.IsValid())
        {
            playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }


    // public TextAsset inkJSON;
    // public dialogueManager dialogueManager;
    // private bool dialogueStarted = false;

    // // public override void OnGraphStart(Playable playable)
    // // {
    // //     // Find the Dialogue Manager if it's not set
    // //     if (dialogueManager == null)
    // //     {
    // //         // You can also get it from the bound object here
    // //         // dialogueManager = FindObjectOfType<dialogueManager>();
    // //     }
    // // }

    // public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    // {
    //     if (dialogueManager == null)
    //     {
    //         dialogueManager = playerData as dialogueManager;
    //     }

    //     // Only run once when the clip is active
    //     if (!dialogueStarted)
    //     {
    //         if (dialogueManager != null)
    //         {
    //             dialogueManager.enterDialogueMode(inkJSON);
    //             playable.GetGraph().GetRootPlayable(0).SetSpeed(0); // Pause the timeline
    //             dialogueStarted = true;
    //         }
    //     }
    // }
}