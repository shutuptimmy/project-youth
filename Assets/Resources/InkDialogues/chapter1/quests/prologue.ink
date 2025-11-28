=== prologue ===
VAR prologueId = "prologue"

VAR prologueState = "IN_PROGRESS"

// for blue car dialogues
{ prologueState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	// - "CAN_FINISH": -> canFinish
	// - "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
cant do that rn. #name:You
~startQuest(prologueId)
-> END

= canStart
hey
~startQuest(prologueId)
-> END

= inProgress
time to move boxes
-> END


