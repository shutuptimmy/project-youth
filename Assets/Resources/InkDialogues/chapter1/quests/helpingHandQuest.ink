=== helpingHandQuest ===

// quest ids (questId + "Id" for var name)
VAR helpingHandId = "helpingHand"

// quest states (questId + "State" for var name)
VAR helpingHandState = "REQ_NOT_MET"

{ helpingHandState :
	- "REQ_NOT_MET": -> reqNotMet
    - "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
I can handle the topic just fine. #name:ClassmateBeetle
-> END

= canStart
I heard you have learned something in the module. #name:ClassmateBeetle
Mind helping me out?
    + [Yeah]
        ~ startQuest(helpingHandId)
    + [Not now.]
        Not now. #name:You
- -> END

= inProgress
Okay then. Which of the following best describes a force? #name:ClassmateBeetle
    + [Energy stored in an object]
        -> wrong
    + [A push or a pull acting on an object]
        -> correct
    + [The mass of an object]
        -> wrong

= wrong
That doesn't sound right. Oh well we tried. #name:ClassmateBeetle
    ~ advanceQuest(helpingHandId)
-> END

= correct
Yeah.. yeah, it sounds about right! Thanks! #name:ClassmateBeetle
    ~ expGained(20)
    ~ advanceQuest(helpingHandId)
-> END

= canFinish
Thanks for your knowledge, I guess. #name:ClassmateBeetle
    ~ finishQuest(helpingHandId)
-> END

= finished
It's almost afternoon. #name:ClassmateBeetle
-> END
