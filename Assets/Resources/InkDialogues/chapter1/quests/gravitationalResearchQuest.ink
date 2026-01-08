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
		Hey, April. I was hoping if I can copy your notes that I've missed yesterday. #name:You #char:you
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
	Hey, April. Can you help me with something? #name:You #char:you
	I heard everything. #name:April #char:april
	You do? #name:You #char:you
	Mhm. However, I would also need help with my studies. #name:April #char:april
	Exchanging each other's favor is a fair deal. Agree?
	+ [Sounds fair]
		Great. Come with me at the rooftops.
		~ startQuest(gravitationalResearchId)
	+ [Let me think of it.]
		If you made up your mind, come talk to me.
- -> END

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
	Thanks again. Now, I have plenty of ideas to work with. #name:April #char:april
-> END
