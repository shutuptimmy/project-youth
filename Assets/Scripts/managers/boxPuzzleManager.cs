using UnityEngine;

public class boxPuzzleManager : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    [SerializeField] private moveTheBoxQuestStep moveTheBoxQuestStep;
    [SerializeField] private string boxPuzzleId;
    [SerializeField] pushableBox box;
    [SerializeField] boxGoalTrigger goal;

    public bool isPuzzleFinished { get; private set; }

    void Start()
    {
        isPuzzleFinished = false;
        box.setPuzzleId(boxPuzzleId);
        goal.setPuzzleId(boxPuzzleId);
    }

    public void puzzleComplete()
    {
        isPuzzleFinished = true;
        moveTheBoxQuestStep.puzzleCompleted(boxPuzzleId);
    }

    public string getPuzzleId()
    {
        return boxPuzzleId;
    }
}
