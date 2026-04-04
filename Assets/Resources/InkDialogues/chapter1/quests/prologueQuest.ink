// this quest automatically starts in the new game
=== prologueQuest ===
VAR prologueId = "prologue"

VAR prologueState = "REQ_NOT_MET"

// for blue car dialogues
{ prologueState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
sum ting wong. #name:You
~startQuest(prologueId)
-> END

= canStart
Hell? This quest should be active rn.
~startQuest(prologueId)
-> END

= inProgress
time to move boxes
-> END

= canFinish
Mew. Great job. You can now leave the apartment. #name:Cat #char:bluecar
Thanks.. err. #name:You #char:you
~finishQuest(prologueId)
-> END

= finished
Mew. #name:Cat #char:bluecar
-> END

=== prologue ===
hey.
-> END

=== prologueBox ===
Huh. I forgot about that box. #name:You #char:you
Gotta move that aside before opening it..
~startQuest(prologueId)
-> END

=== prologueBoxCar ===
~itemReward("blueCar")
Hm- oh, what the!? #name:You #char:you
Mew. Kept ya waitin, huh? #name:Cat #char:bluecar
~itemReward("introForce")
Here's a piece of paper as a thank you.
~advanceQuest(prologueId)
-> END