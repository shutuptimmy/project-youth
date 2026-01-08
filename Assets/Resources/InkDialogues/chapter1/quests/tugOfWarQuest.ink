=== tugOfWarQuest ===
// quest ids (questId + "Id" for var name)
VAR tugOfWarId = "tugOfWar"

// quest states (questId + "State" for var name)
VAR tugOfWarState = "REQ_NOT_MET"

{ tugOfWarState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
{preTestTimeState:
	- "FINISHED":
		Hey, Weasel? #name:You #char:you
		Ha? #name:Wallace #char:wallace
		Erm.. The teacher said you have a paper from our previous lesson. Can I.. have it? #name:You #char:you
		Pfft, hahahah! Like hell i'm givin' this that easy! Now scram! #name:Wallace #char:wallace
		-> END
	- else:
		...What? #name:Wallace #char:wallace
		Uhm.. nothing. #name:You #char:you
		-> END
}

= canStart
Heard ya catchin' up your lessons pretty quickly. I'm impressed. #name:Wallace #char:wallace
...Yeah? #name:You #char:you
Aight. Here's the deal. I'm in no mood to give this to a simpleton so there's some certain activity I'd like to try with you. #name:Wallace #char:wallace
You in?
	+ [Sure?]
		What.. kind of activity? #name:You #char:you
		~ startQuest(tugOfWarId)
	+ [Maybe later]
		On second thought. Maybe next time. #name:You #char:you
		Man, you're no fun. #name:Wallace #char:wallace
- -> END

= inProgress
Sumthin else's missin's before I go. #name:You
-> END

= canFinish
How was that even..!? #name:Wallace #char:wallace
~itemReward("forceTypeContact")
Ugh, fine. As promised.
~ finishQuest(tugOfWarId)
-> END

= finished
To be honest, that note I gave ya was from Harry's. #name:Wallace #char:wallace
Too lazy to write your own? #name:You #char:you
Sybau. He owed me a favor so now we're even. #name:Wallace #char:wallace
-> END
