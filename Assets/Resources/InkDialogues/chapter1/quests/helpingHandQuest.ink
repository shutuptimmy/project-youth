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
Hey, Harry. It's urgent. #name:You #char:you
Ah, {playerName}. Sorry, my hands are full. I'm getting irritated with these boxes, not knowing where to put them. #name:Harry #char:harry
..I know! Can you help me? When we're done, i'll share notes with you.
    + [Great]
		Sounds great. Teach me how to sort them out.
        ~ startQuest(helpingHandId)
    + [Not now.]
        Maybe not now. #name:You #char:you
	Aww. Okay. #name:Harry #char:harry
- -> END

= canFinish
	Phew. Thanks a bunch, {playerName}! Here you go. This is all I wrote. #name:Harry #char:harry
	~itemReward("forceChar")
	Thanks, Harry! #name:You #char:you
	~finishQuest(helpingHandId)
-> END

= finished
Say. After this, you mind we head to your house and watch other animes? #name:Harry #char:harry
Sure! Feel free to. #name:You #char:you
(Wait, I feel like i'm forgetting something...)
-> END
