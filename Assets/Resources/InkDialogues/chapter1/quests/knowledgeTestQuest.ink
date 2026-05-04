=== knowledgeTestQuest ===
// quest ids (questId + "Id" for var name)
VAR knowledgeTestId = "knowledgeTest"
VAR randId = "yes"

// quest states (questId + "State" for var name)
VAR knowledgeTestState = "REQ_NOT_MET"

{ knowledgeTestState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
{ preTestTimeState:
	- "FINISHED": { guideNote: -> notReady | -> guideNoteDialogue }
	- else:
		{playerName}. Where were you yesterday? #name:Christopher #char:chris
		Ah. Sickness. What did I miss? #name:You #char:you
		You better talk to the teacher. #name:Christopher #char:chris
		Was yesterday.. necessary? #name:You #char:you
		... #name:Christopher #char:chris
		(Oh boy.) #name:You #char:you
		-> END
}

= notReady
Talk to me again if you have enough knowledge (Level) about the topics. #name:Christopher #char:chris
-> END

= canStart
{ guideNote: -> readyToGo | -> guideNoteDialogue }

= readyToGo
I hope you're ready what i'm about to teach you. #name:Christopher #char:chris
	+ [I'm ready when you are]
		~ startQuest(knowledgeTestId)
	+ [Give me a second]
- -> END

= inProgress
Sumthin else's missin's before I go. #name:You
-> END

= canFinish
Good effort you made. You're learning quicker than I expected. #name:Christopher #char:chris
Does that make me one of Newton's descendant? #name:You #char:you
Pfft. You got me. #name:Christopher #char:chris
Go talk to anyone to know more about the lessons.
Each NPC has its own level and quest requirements. Check the quest log at the top right for more details.
~itemReward("callADay")
~ finishQuest(knowledgeTestId)
-> END

= finished
{aftermathState:
	- "IN_PROGRESS":
		Got everything you need? #name:Christopher #char:chris
		+ [Yep. Let's head to my appartment]
			~finishDay(randId)
			-> END
		+ [Hang on]
			-> END

	- "FINISHED":
		I appreciated that you invited me over, {playerName}. Know that I would have less problem in here compare to where I live. #name:Christopher #char:chris
		Anytime. The note you gave me is handful and all. #name:You #char:you
		And we can write more if we study sooner than later. #name:Christopher #char:chris
		-> END

	- else:
		Go talk to anyone to know more about the lessons before finishing today. #name:Christopher #char:chris
		-> END
}

= guideNoteDialogue
I.. screw it up. #name:You #char:you
Don't be. It's better to look after your health rather than forcing yourself to go. #name:Christopher #char:chris
(I feel even more guilty after I lie about it!) #name:You #char:you
Anyway, I have a note that you can copy. It's simply a vocabulary for certain words that you can learn faster. #name:Christopher #char:chris
Oh, thank you. You're a lifesaver. #name:You #char:you
~itemReward("contentVocab")
~guideNote = true
-> END
