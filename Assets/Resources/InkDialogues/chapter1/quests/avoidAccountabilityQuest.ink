=== avoidAccountabilityQuest ===
// quest ids (questId + "Id" for var name)
VAR avoidAccountabilityId = "avoidAccountability"

// quest states (questId + "State" for var name)
VAR avoidAccountabilityState = "REQ_NOT_MET"

{ avoidAccountabilityState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
{ aftermathState == "IN_PROGRESS":
	Oh man.. oh man oh man. #name:Michael #char:default
	-> END
- else:
	Hey. #name:You #char:you
	(Blasting Rock & Roll in his headphones) #name:Michael #char:default
	His music can hear all the way here.. #name:You #char:you
	-> END
}

= canStart
{ aftermathState == "IN_PROGRESS":
	Oh man.. oh man oh man. #name:Michael #char:default
	-> END
- else:
	Hey. Uh, do you have a minute? #name:Michael #char:default
	What's up? #name:You #char:you
	It's.. erm.. #name:Michael #char:default
	I made a grave mistake to the teacher's desk.
	What happened? #name:You #char:you
	While I was tapping the desk like a drum, I hit it too hard and the sound... well.. not so good. #name:Michael #char:default
	It may look fine, but it's legs are about to break. Can you help me place her things back carefully?
	+ [I can help, but you have to do it on yourself.]
		Eh!? Uh.. alright.
		~ startQuest(avoidAccountabilityId)
		-> END
	+ [I think my hands are full]
		Oh man..
		-> END
}


= inProgress
I'm supposed to be in a minigame #name:You
-> END

= canFinish
Phew. I can't thank you enough, {playerName}. #name:Michael #char:default
Don't mention it. (I'm still going to tell Teacher regardless.) #name:You #char:you
~ itemReward("deskBooks")
~ finishQuest(avoidAccountabilityId)
-> END

= finished
(Back to jamming music like it never happened) #name:Michael #char:default
This guy... #name:You #char:you
-> END