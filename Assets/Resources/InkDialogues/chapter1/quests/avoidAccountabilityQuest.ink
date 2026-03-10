=== avoidAccountabilityQuest ===
// quest ids (questId + "Id" for var name)
VAR AvoidAccountabilityId = "AvoidAccountability"

// quest states (questId + "State" for var name)
VAR AvoidAccountabilityState = "REQ_NOT_MET"

{ AvoidAccountabilityState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
Hey. #name:You #char:you
(Swings his hand uncontrollably) #name:Classmate #char:default
(His music can hear all the way here.) #name:You #char:you
-> END

= canStart
Hey. Uh, do you have a minute? #name:Classmate #char:default
What's up? #name:You #char:you
It's.. erm.. #name:Classmate #char:default
I made a grave mistake to the teacher's desk.
What happened? #name:You #char:you
While I was doing a schizo drum thing, I hit it too hard and the sound..... #name:Classmate #char:default
It may look fine, but it's about to break. Can you help me place her things back carefully?
+ [I can help, but you have to do it on your own.]
	Eh!? Uh.. alright.
	~ startQuest(AvoidAccountabilityId)
+ [I think my hands are full]
	Oh man..
- -> END

= inProgress
I'm supposed to be in a minigame #name:You
-> END

= canFinish
Phew. I can't thank you enough, {playerName}. #name:Classmate #char:default
Don't mention it. (I'd still going to talk to the teacher regardless.) #name:You #char:you
~ finishQuest(AvoidAccountabilityId)
-> END

= finished
(Swings his hand uncontrollably again) #name:Classmate #char:default
This guy... #name:You #char:you
-> END