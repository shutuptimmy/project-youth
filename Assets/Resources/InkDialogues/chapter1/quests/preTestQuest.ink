=== preTestQuest ===
// quest ids (questId + "Id" for var name)
VAR preTestTimeId = "preTestTime"

// quest states (questId + "State" for var name)
VAR preTestTimeState = "REQ_NOT_MET"

{ preTestTimeState :
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
	Good morning, {playerName}. You seem to have missed our important lessons yesterday. #name:Teacher #char:default
	I.. had a fever. And what do you mean by "important lessons"? #name:You #char:you
	Oh? For tomorrow's exam, of course. Haven't you forgot? #name:Teacher #char:default
	Ah. Oh.. #name:You #char:you
	I've told all of you not to absent yesterday, have I not? #name:Teacher #char:default
	Y-yes, yes you did, Teacher. #name:You #char:you
	(I shouldn't have stayed up all night binge-watching Two Piece)
	Since I can't help you with your missed lectures, how about a friendly test? #name:Teacher #char:default
	I would like to see how much you know about the lessons I have discussed yesterday.
	+ [I'm ready]
		~ startQuest(preTestTimeId)
	+ [Wait a moment]
- -> END

= inProgress
I'm supposed to be in a minigame #name:You
-> END

= canFinish
	~ finishQuest(preTestTimeId)
	Not bad. Now, you know what's going to come out tomorrow. #name:Teacher #char:default
	Thank you, Teacher. #name:You #char:you
	Don't thank me, yet. Pray that you will pass during the exam. #name:Teacher #char:default
	...However, as a teacher, no student should be left uneducated. I'm giving you this note as a chance to redeem yourself.
	~itemReward("introForce2") 
	In a meantime, try asking your classmates if they're willing to share notes with you.
	-> END

= finished
{aftermathState:
	- "IN_PROGRESS":
		Good luck for tomorrow {playerName}. #name:Teacher #char:default
		-> END

	- else:
		And don't even think about skipping class next time, hm? #name:Teacher #char:default
		Eheheh. #name:You #char:you
		-> END
}

/*
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
*/
