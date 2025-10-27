=== moduleQuest ===
// quest ids (questId + "Id" for var name)
VAR moduleId = "module"

// quest states (questId + "State" for var name)
VAR moduleState = "REQ_NOT_MET"

{ moduleState :
	// - "REQ_NOT_MET": -> reqNotMet
	// - "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

// = reqNotMet
// sumthin aint right. fix it via scripts #name:You
// -> END

// = canStart
// 	~ startQuest(helpingHandId)
// -> END

= inProgress
Once you're reading, come back to me. #name:PurpleGuy
-> END

= canFinish
~ finishQuest(moduleId)
My buddy redback here needs some help for your wisdom. Go talk to him. (Level 5 required) #name:PurpleGuy
-> END

= finished
The bettle dude have some questions for you first. Talk to me later. #name:PurpleGuy
-> END