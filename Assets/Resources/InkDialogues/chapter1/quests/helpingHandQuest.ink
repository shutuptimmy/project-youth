=== helpingHandQuest ===

// quest ids (questId + "Id" for var name)
VAR helpingHandId = "helpingHand"

// quest states (questId + "State" for var name)
VAR helpingHandState = "REQ_NOT_MET"

{ helpingHandState :
	- "REQ_NOT_MET": -> reqNotMet
    - "CAN_START": -> canStart
//	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
{preTestTimeState:
	- "FINISHED":
		Hey, Harry. Need a moment? #name:You #char:you
		Ah, {playerName}. Sorry, my hands are full. Come back when i'm done. #name:Harry #char:harry
		-> END
	- else:
		Oh, {playerName}. You're here. #name:Harry #char:harry
		What are you doing outside? #name:You #char:you
		The teacher assigned me to move those boxes from the other side later. #name:Harry #char:harry
		By the way, how was the anime I suggested to you?
		It was fantastic! But, I'm going to chit-chat later. Duty calls. #name:You #char:you
		Sure thing! Glad you're back. #name:Harry #char:harry
		-> END
}
-> END

= canStart
{ aftermathState == "IN_PROGRESS":
	Oh hey, {playerName}. I don't think I can able to come over to your home at the moment. I still have to sort all these boxes left in the hallway. #name:Harry #char:harry
	-> END
- else:
	Hey, Harry. It's urgent. #name:You #char:you
	Ah, {playerName}. Sorry, my hands are full. I'm getting irritated with these boxes, not knowing where to put them. #name:Harry #char:harry
	..I know! Can you help me? When we're done, i'll share notes with you.
	    + [Sounds great]
		Sounds great. Teach me how to sort them out. #name:You #char:you
		~ startQuest(helpingHandId)
		-> END
	    + [Not now.]
		Maybe not now. #name:You #char:you
		Aww. Okay. #name:Harry #char:harry
		-> END
}

= canFinish
	Phew. Thanks a bunch, {playerName}! Here you go. This is all I wrote. #name:Harry #char:harry
	~itemReward("forceChar")
	Thanks, Harry! #name:You #char:you
	~finishQuest(helpingHandId)
-> END

= finished
{ aftermathState == "FINISHED":
	You didn't tell me you have a cat. It looks so cute. #name:Harry #char:harry
	Mew. Pet me more. #name:Cat #char:bluecar
	What the!? It talks! #name:Harry #char:harry
	Yeah.. You don't hear that often. #name:You #char:you
	-> END
- else:
	Say. After this, you mind we head to your house and watch other animes? #name:Harry #char:harry
	Sure! Feel free to. #name:You #char:you
	(Wait, I feel like i'm forgetting something...)
	-> END
}
