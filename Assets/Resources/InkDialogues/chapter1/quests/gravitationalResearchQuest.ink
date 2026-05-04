=== gravitationalResearchQuest ===
// quest ids (questId + "Id" for var name)
VAR gravitationalResearchId = "gravitationalResearch"

// quest states (questId + "State" for var name)
VAR gravitationalResearchState = "REQ_NOT_MET"

{ gravitationalResearchState :
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
		Hey, April. I'm hoping if I can copy your notes that I've missed yesterday. #name:You #char:you
		... #name:April #char:april
		..April? *Pokes her* #name:You #char:you
		Wha- huh? Oh, {playerName}. I'm currently reading my notes. Come back later. #name:April #char:april
		-> END
	- else:
		Hey, April. #name:You #char:you
		... #name:April #char:april
		(She seems too focused on her book.) #name:You #char:you
		Hey, {playerName}. #name:April #char:april
		-> END
}

= canStart
{ aftermathState == "IN_PROGRESS":
	That's all for today. I'll see you tomorrow, {playerName}. #name:April #char:april
	-> END

- else:
	Hey, April. Can you help me with something? #name:You #char:you
	I heard everything. #name:April #char:april
	You do? #name:You #char:you
	Mhm. However, I would also need help with my studies. #name:April #char:april
	Exchanging each other's favor is a fair deal. Agree?
	+ [Sounds fair]
		Great. Come with me at the rooftops.
		~ startQuest(gravitationalResearchId)
		-> END
	+ [Let me think of it.]
		If you made up your mind, come talk to me.
		-> END
}

= inProgress
Sumthin else's missin's before I go. #name:You
-> END

= canFinish
	Ugh, my head. #name:You #char:mcBoy
	Thanks for being my experi- uh, helping with my studies. #name:April #char:april
	Here's the note you've ask for.
	~itemReward("forceTypeNonContact")
	Oh, and would you like an apple?
	Uh, No. My head is spinning. #name:You #char:you
	~finishQuest(gravitationalResearchId)
-> END

= finished
{ aftermathState:
	- "IN_PROGRESS":
		{playerName}. You won't mind if I can hangout at your place? #name:April #char:april
		Of course! Feel free to come along. #name:You #char:you
		Excellent. For this, I can continue my experime- I mean research about this. #name:April #char:april
		(Experiment? What is she planning..?) #name:You #char:you
		-> END

	- "FINISHED":
		I came up some fascinating concepts of defying gravity. #name:April #char:april
		Is that going to help us for tomorrow? #name:You #char:you
		Maybe. Maybe not. This is more likely a part of my hobby rather than the exam. Unless.. huhu. #name:April #char:april
		Don't even think about involving me into one of your shenanigans. #name:You #char:you
		-> END	

	- else:
		Thanks again. Now, I have plenty of ideas to work with. #name:April #char:april
		-> END
}
