using Ink.Runtime;

public class inkExternalFunctions
{
    public void bind(Story story)
    {
        story.BindExternalFunction("startQuest", (string questId) => startQuest(questId));
        story.BindExternalFunction("advanceQuest", (string questId) => advanceQuest(questId));
        story.BindExternalFunction("finishQuest", (string questId) => finishQuest(questId));
        story.BindExternalFunction("expGained", (int exp) => expGained(exp));
        story.BindExternalFunction("itemReward", (string itemId) => itemReward(itemId));
    }
    public void unbind(Story story)
    {
        story.UnbindExternalFunction("startQuest");
        story.UnbindExternalFunction("advanceQuest");
        story.UnbindExternalFunction("finishQuest");
        story.UnbindExternalFunction("expGained");
        story.UnbindExternalFunction("itemReward");
    }

    private void startQuest(string questId)
    {
        gameEventsManager.instance.questEvents.startQuest(questId);
    }
    private void advanceQuest(string questId)
    {
        gameEventsManager.instance.questEvents.advanceQuest(questId);

    }
    private void finishQuest(string questId)
    {
        gameEventsManager.instance.questEvents.finishQuest(questId);

    }

    private void expGained(int exp)
    {
        gameEventsManager.instance.playerEvents.ExperienceGained(exp);
    }


    private void itemReward(string itemId)
    {
        gameEventsManager.instance.miscEvents.questReward(itemId);
    }
}
