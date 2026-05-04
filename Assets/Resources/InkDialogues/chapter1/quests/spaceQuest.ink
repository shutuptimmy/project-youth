=== spaceQuest ===
// quest ids (questId + "Id" for var name)
VAR spaceId = "space"

// quest states (questId + "State" for var name)
VAR spaceState = "REQ_NOT_MET"

{ spaceState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
Hmm.. stars, planets, and wonders.. #name:Cassie #char:default
Oh. Hi, {playerName}.
-> END

= canStart
{ aftermathState == "IN_PROGRESS":
	It's time to head home. #name:Cassie #char:default
	-> END
- else:
	Hi, {playerName}. Do you know much about gravity? #name:Cassie #char:default
	Well.. maybe? #name:You #char:you
	What about space? I know you talked to April so why not we find out together? #name:Cassie #char:default
		+ [I accept]
			~ startQuest(spaceId)
			-> END
		+ [Hang on]
			-> END
}

= inProgress
I'm supposed to be in a minigame #name:You
-> END

= canFinish
~ finishQuest(spaceId)
-> finished

= finished
Thanks for the help, {playerName}. Hehe. #name:Cassie #char:default
~itemReward("rocketSpace")
-> END