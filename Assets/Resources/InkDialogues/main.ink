INCLUDE chapter1/quests/packUpQuest.ink
INCLUDE chapter1/quests/prologueQuest.ink
INCLUDE chapter1/quests/preTestQuest.ink
INCLUDE chapter1/quests/knowledgeTestQuest.ink
INCLUDE chapter1/quests/helpingHandQuest.ink
INCLUDE chapter1/quests/tugOfWarQuest.ink
INCLUDE chapter1/quests/gravitationalResearchQuest.ink
INCLUDE chapter1/quests/spaceQuest.ink
INCLUDE chapter1/quests/avoidAccountabilityQuest.ink
INCLUDE chapter1/quests/aftermathQuest.ink


EXTERNAL startQuest(questId)
EXTERNAL advanceQuest(questId)
EXTERNAL finishQuest(questId)
EXTERNAL finishDay(questId)
EXTERNAL expGained(exp)
EXTERNAL itemReward(itemId)

VAR playerName = ""
VAR guideNote = false

=== npc1 ===
Yo, bruh. #name:Classmate #char:default
-> END

=== npc2 ===
If you need anything, talk to my boss next to the stairs. #name:Classmate #char:default
-> END

=== computer ===
I can play after school. #name:You #char:you
-> END