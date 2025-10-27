=== preTestQuest ===
// quest ids (questId + "Id" for var name)
VAR preTestId = "preTest"

// quest states (questId + "State" for var name)
VAR preTestState = "REQ_NOT_MET"

{ preTestState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
sumthin aint right. fix it via scripts #name:You
-> END

= canStart
You ready to take the preTest? #name:Teacher
+ [Bring It]
	~ startQuest(preTestId)
+ [No.]
	No. Not Yet. #name:You
    Very well. Time only moves when you finish this test. #name:Teacher
- -> END

= inProgress
I'm supposed to be in a minigame #name:You
-> END

= canFinish
	~ finishQuest(preTestId)
Good job. Ain't that hard, was it? #name:Teacher
	~ startQuest(moduleId)
Go read the module I made. It's on your desk.
-> END

= finished
{moduleState:
	- "IN_PROGRESS": -> moduleTime
	- "CAN_FINISH": -> moduleDone
	- else: -> donnydone
	}

= moduleTime
Come back to me if you're finished reading them. #name:Teacher
-> END

= moduleDone
Great job! The topic i'm covering this week is the Forces in Action. #name:Teacher
Try to ask one of the students if they need any help for the lesson.
-> END

= donnydone
That purple one wants to ask something. #name:Teacher
-> END
