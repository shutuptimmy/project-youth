using System;
using UnityEngine;

public class miscEvents
{
    public event Action<string> onBoxPuzzleCompleted;
    public void boxPuzzleCompleted(string puzzleId)
    {
        onBoxPuzzleCompleted?.Invoke(puzzleId);
    }

    public event Action<string> onQuestReward;
    public void questReward(string rewardId)
    {
        onQuestReward?.Invoke(rewardId);
    }

    public event Action<string, bool> onBoxDraggingStateChanged;
    public void boxDraggingStateChanged(string boxId, bool isDragging)
    {
        onBoxDraggingStateChanged?.Invoke(boxId, isDragging);
    }

    public event Action<lessonInfoSO> onShowLessonPanel;
    public void showLessonPanel(lessonInfoSO lesson)
    {
        onShowLessonPanel?.Invoke(lesson);
    }

    public event Action onPlayerTookDamage;
    public void playerTookDamage()
    {
        onPlayerTookDamage?.Invoke();
    }
}
