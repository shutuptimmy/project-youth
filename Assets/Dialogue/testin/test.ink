INCLUDE ../globals.ink

{ choice == "": -> main | -> already_chose}

=== main ===
Go talk to that dawg behind ya. #charName:NPC #charPortrait:npcGood
Perhaps he knows sumthin' ya want.
That abnormal creature? You serious..? #charName:You #charPortrait:default
-> END

=== already_chose ===
So, how'd it go? #charName:NPC #charPortrait:npcNeutral
Well.. "{choice}." is what I said. Gonna get through his skin. How's that? #charName:You #charPortrait:default
Oh boy... #charName:NPC #charPortrait:npcBad
You never change.
-> END
