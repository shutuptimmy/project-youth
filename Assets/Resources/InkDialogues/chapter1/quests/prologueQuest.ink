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
Mew. Great job. You can now leave the apartment.
Thanks.. err.
~finishQuest(prologueId)
-> END

= finished
Mew.
-> END

=== prologue ===
hey.
-> END

=== prologueBox ===
Huh. I forgot about that box.
Gotta move that aside before opening it..
~startQuest(prologueId)
-> END

=== prologueBoxCar ===
Hm- oh, what the!? #name:You
Mew. Kept ya waitin, huh? #name:BlueCar
~itemReward("introForce")
Here's a piece of paper as a thank you.
~advanceQuest(prologueId)
-> END