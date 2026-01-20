using System;

public class playerEvents
{
    public event Action onDisablePlayerMovement;
    public void DisablePlayerMovement()
    {
        onDisablePlayerMovement?.Invoke();
    }

    public event Action onEnablePlayerMovement;
    public void EnablePlayerMovement()
    {
        onEnablePlayerMovement?.Invoke();
    }

    public event Action<int> onExperienceGained;
    public void ExperienceGained(int experience)
    {
        if (onExperienceGained != null)
        {
            onExperienceGained(experience);
        }
    }

    public event Action<int> onPlayerLevelChange;
    public void PlayerLevelChange(int level)
    {
        if (onPlayerLevelChange != null)
        {
            onPlayerLevelChange(level);
        }
    }

    public event Action<int> onPlayerExperienceChange;
    public void PlayerExperienceChange(int experience)
    {
        if (onPlayerExperienceChange != null)
        {
            onPlayerExperienceChange(experience);
        }
    }

    public event Action onPlayerTookDamage;
    public void playerTookDamage()
    {
        onPlayerTookDamage?.Invoke();
    }
}