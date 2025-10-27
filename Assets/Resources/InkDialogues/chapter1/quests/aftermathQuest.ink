=== aftermathQuest ===
// quest ids (questId + "Id" for var name)
VAR aftermathId = "aftermath"
VAR newguy = true

// quest states (questId + "State" for var name)
VAR aftermathState = "REQ_NOT_MET"

{ aftermathState :
	- "REQ_NOT_MET": -> reqNotMet
	- "CAN_START": -> canStart
	- "IN_PROGRESS": -> inProgress
	- "CAN_FINISH": -> canFinish
	- "FINISHED": -> finished
	- else: -> END
	}

= reqNotMet
{newguy == false: -> familiar | -> new }

= new
hello there. Never seen you around on these parts. #name:Student
Welcome to your new school. Just head over to that door, which is your classroom, and ask the teacher.
~ newguy = false
-> END

= familiar
Need something? I'll keep an eye out on here. #name:Student
He's late. I don't know where he went.
-> END


= canStart
Seems everything's settled. Wanna skip to dismissal? #name:Student
    + [Yes]
        ~ startQuest(aftermathId)
        -> END
    + [Not yet]
        Okay. Let me know.
        -> END

= inProgress
School's out. Go home and get some rest. #name:Student
-> END

= canFinish
texto heru.
-> END

= finished
texto heru.
-> END
