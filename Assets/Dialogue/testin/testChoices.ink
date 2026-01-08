INCLUDE ../globals.ink

{ choice == "": -> main | -> already_chose}

=== main ===
Hey, foxhound. Think ya can spare me meat of yours? heheheheh.. #charName:NPCChoice #charPortrait:npcChoiceNeutral
    + [Funny]
        -> chosen("Very funny, lil' rotten mute")
    + [Nah] #lvlReq:5
        -> chosen("Too bad, freaky ahh doggo")
    + [???]
        -> chosen("Was wong wid ya?")

=== chosen(talk) ===
~ choice = talk
{talk}. Anyway, imma head off. #charName:You #charPortrait:default
Later, weirdo.

- Hey, over here yo! #charName:NPC #charPortrait:npcNeutral
-> END

=== already_chose ===
What brings you here, again? Get outta here. #charName:NPCChoice #charPortrait:npcChoiceBad
-> END
