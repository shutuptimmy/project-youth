using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class desk : MonoBehaviour
{
    [Header("Desk Config")]
    [SerializeField] private float maxHP;
    [SerializeField] private float crackingPercentage;
    [SerializeField] private float breakingPercentage;
    [SerializeField] private float weightSensitivityFactor;
    [SerializeField] private Collider2D surfaceCollider;
    [SerializeField] private Image meterBarFill;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip thudSFX;
    [SerializeField] private AudioClip crackSFX;
    [SerializeField] private AudioClip destroyedSFX;

    public List<Rigidbody2D> objInZone { get; private set; } = new List<Rigidbody2D>();
    private float currentDamage = 0f;
    private int lastStressLevel = 0;
    private Animator animator;

    public bool isBroken { get; private set; } = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        updateMeterBar();
    }

    void Update()
    {
        if (isBroken) return;
        updateMeterBar();
        if (currentDamage >= maxHP) collapseDesk();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;

        if (collision.gameObject.CompareTag("MassObj") && collision.otherCollider == surfaceCollider)
        {
            float impactForce = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
            if (impactForce > 1.5f) applyImpactDamage(impactForce);
        }
    }

    // Called when an object is lifted off from the desk
    void OnCollisionExit2D(Collision2D collision)
    {
        if (objInZone.Contains(collision.rigidbody)) objInZone.Remove(collision.rigidbody);
    }

    public void addObjToZone(Rigidbody2D rb)
    {
        if (!objInZone.Contains(rb)) objInZone.Add(rb);
    }

    public void removeObjectFromZone(Rigidbody2D rb)
    {
        if (objInZone.Contains(rb)) objInZone.Remove(rb);
    }

    public void applyImpactDamage(float rawForce)
    {
        if (isBroken) return;

        float multiplier = sensitivityMultiplier();
        float damageTaken = rawForce * multiplier;

        currentDamage += damageTaken;
        soundFXManager.instance.playSoundClip(thudSFX, this.transform, 1f);
        Debug.Log($"IMPACT! Raw: {rawForce:F1} | Damage: {damageTaken:F1}");
    }

    void updateMeterBar()
    {
        meterBarFill.fillAmount = Mathf.Clamp01(currentDamage / maxHP);
        float percentage = (currentDamage / maxHP) * 100f;

        int currentStress;
        if (percentage >= breakingPercentage) currentStress = 2;
        else if (percentage >= crackingPercentage) currentStress = 1;
        else currentStress = 0;

        // Only trigger if the stress level has changed (increased)
        if (currentStress > lastStressLevel) soundFXManager.instance.playSoundClip(crackSFX, this.transform, 1f);

        animator.SetInteger("stress", currentStress);
        
        lastStressLevel = currentStress;
    }

    float sensitivityMultiplier()
    {
        float healthMultiplier = 1f;
        float damagePercentage = (currentDamage / maxHP) * 100f;

        if (damagePercentage >= breakingPercentage) healthMultiplier = 1.4f;
        else if (damagePercentage >= crackingPercentage) healthMultiplier = 1.2f;

        float currentMass = getTotalPlacedMass();
        float weightMultiplier = currentMass * weightSensitivityFactor;

        return healthMultiplier + weightMultiplier;
    }

    // for individual objects
    public bool isObjectResting(Rigidbody2D rb)
    {
        // Must be inside the weight zone
        if (!objInZone.Contains(rb)) return false;

        // Must not be held by player
        draggableMassObject objScript = rb.GetComponent<draggableMassObject>();
        if (objScript != null && objScript.isDragging) return false;

        // Must not be moving (falling/flying)
        if (rb.velocity.magnitude > 1f) return false;

        return true;
    }

    public int getTotalRestedObjs()
    {
        int count = 0;
        foreach (Rigidbody2D rb in objInZone)
        {
            if (isObjectResting(rb)) count++;
        }
        return count;
    }

    public float getTotalPlacedMass()
    {
        float totalMass = 0f;

        // Loop backwards to safely remove nulls
        for (int i = objInZone.Count - 1; i >= 0; i--)
        {
            Rigidbody2D rb = objInZone[i];

            // Cleanup Check
            if (rb == null)
            {
                objInZone.RemoveAt(i);
                continue;
            }

            if (isObjectResting(rb)) totalMass += rb.mass;
        }
        return totalMass;
    }

    // check win condition if all objects are resting on the desk
    public bool areObjectsStable()
    {
        if (objInZone.Count == 0) return false;

        foreach (Rigidbody2D rb in objInZone)
        {
            // if (rb == null) continue;
            if (!isObjectResting(rb)) return false;
        }
        return true;
    }

    void collapseDesk()
    {
        isBroken = true;
        animator.SetBool("break", true);
        soundFXManager.instance.playSoundClip(destroyedSFX, this.transform, 1f);

        // Add force to books to make them fly
        foreach (Rigidbody2D rb in objInZone)
        {
            rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }

        // disable all its collider so the objects fall off
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders) col.enabled = false;
    }

    public void resetDesk()
    {
        isBroken = false;
        currentDamage = 0f;
        lastStressLevel = 0;
        objInZone.Clear();

        animator.SetBool("break", false);
        animator.SetInteger("stress", 0);
        updateMeterBar();

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders) col.enabled = true;
    }
}
