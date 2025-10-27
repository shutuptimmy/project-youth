using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ModuleInteraction : InteractableBase
{
    private CircleCollider2D circleCollider;
    private void Reset()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = .25f;
    }

    public override void Interact()
    {
        if (!moduleManager.GetInstance().isModuleActive)
        {
            moduleManager.GetInstance().enterModuleMode();
        }
    }

}
