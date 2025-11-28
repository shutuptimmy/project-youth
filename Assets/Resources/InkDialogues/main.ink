INCLUDE chapter1/cutscenes/prelude.ink
INCLUDE chapter1/quests/packUpQuest.ink
INCLUDE chapter1/quests/prologue.ink
INCLUDE chapter1/cutscenes/newDayInSchool.ink
INCLUDE chapter1/quests/preTestQuest.ink
INCLUDE chapter1/quests/moduleQuest.ink
INCLUDE chapter1/quests/helpingHandQuest.ink
INCLUDE chapter1/quests/aftermathQuest.ink


EXTERNAL startQuest(questId)
EXTERNAL advanceQuest(questId)
EXTERNAL finishQuest(questId)
EXTERNAL expGained(exp)

=== doorLocked ===
... Locked. #name:You
-> END
