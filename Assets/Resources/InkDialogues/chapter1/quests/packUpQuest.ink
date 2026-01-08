=== packUpQuest ===
// quest ids (questId + "Id" for var name)
VAR PackUpId = "PackUp"

// quest states (questId + "State" for var name)
VAR PackUpState = "REQ_NOT_MET"

{ PackUpState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
sumthin aint right. fix it via scripts
-> END

= canStart
well welll well. #name:Croak #char:npcGood 
rise n shine ya lazy bum.
start packin up.
+ [Fine fine]
	~ startQuest(PackUpId)
+ [No.]
	no. #name:You #char:default
- -> END

= inProgress
Sumthin else's missin's before I go. #name:You
-> END

= canFinish
Mew, good job. You're ready to go #name:Cat
~ finishQuest(PackUpId)
-> END

= finished
meow. #name:Cat
-> END


// misc
=== bed ===
you make your bed and find your phone. #name:
... #name:You
dead battery.
-> END

=== drawer ===
you suit up for school nicely and neatly. #name:
(This will be your save point after school.)
-> END

=== computer ===
you picked up your keys laying beside your system unit. #name:
(You will able to play unlocked minigames here after school)
-> END
