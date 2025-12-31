=== gravitationalResearchQuest ===
// quest ids (questId + "Id" for var name)
VAR gravitationalResearchId = "gravitationalResearch"

// quest states (questId + "State" for var name)
VAR gravitationalResearchState = "REQ_NOT_MET"

{ tugOfWarState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
sumthin aint right. fix it via scripts
-> END

= canStart
well welll well. #name:Croak #char:npcGood 
rise n shine ya lazy bum.
start packin up.
+ [Fine fine]
	~ startQuest(gravitationalResearchId)
+ [No.]
	no. #name:You #char:default
- -> END

= inProgress
Sumthin else's missin's before I go. #name:You
-> END

= canFinish
Mew, good job. You're ready to go #name:Cat
~ finishQuest(gravitationalResearchId)
-> END

= finished
meow. #name:Cat
-> END
