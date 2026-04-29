=== aftermathQuest ===
// quest ids (questId + "Id" for var name)
VAR aftermathId = "aftermath"

// quest states (questId + "State" for var name)
VAR aftermathState = "REQ_NOT_MET"

{ aftermathState :
//	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= canStart
Is it time to wrap things up? When proceed, any unfinished quests would no longer be accessible.
    + [Yes]
        ~ startQuest(aftermathId)
	~ itemReward("gameFinish")
    + [Not yet]

- -> END

= inProgress
I should ask Chris so we can head home. #name:You #char:you
-> END

= canFinish
texto heru.
-> END

= finished
texto heru.
-> END

== OnAftermath ==
Everything's settled for today. I've also talked to my friends during class if they're willing to hangover at my place. #name:You #char:you
I wonder who will come later.
-> END