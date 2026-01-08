using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope : MonoBehaviour
{
    [Header("Components")]
    // [SerializeField] private BoxCollider2D flagCollider;
    [SerializeField] private tugOfWarManager manager;

    [Header("Configuration")]
    public float currentRopeValue = 0;

    // Total distance required to win/lose. Must match the max/min in manager.
    private float maxRopeDistance;

    // Physical X position limits where the flag can move.
    [SerializeField] private float minMaxHorizontalX = .7f;
    // [SerializeField] private float maxHorizontalX = .7f;

    private void Start()
    {
        // if (flagCollider == null)
        // {
        //     Debug.LogError("Flag Collider must be assigned to the Rope script!");
        // }
        maxRopeDistance = manager.getMaxRopeDistance();
    }

    private void Update()
    {

        float normalizedValue = Mathf.InverseLerp(-maxRopeDistance, maxRopeDistance, currentRopeValue);

        float targetX = Mathf.Lerp(-minMaxHorizontalX, minMaxHorizontalX, normalizedValue);

        Vector2 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetX, 0.1f);

        transform.position = pos;
    }

    // [SerializeField] private BoxCollider2D flag;
    // public int ropeValue;

    // [SerializeField] private Vector2 minMaxValue;
    // [SerializeField] private Vector2 minMaxHorizontalValue;


    // private void Update()
    // {
    //     var pos = transform.position;
    //     var normalize = Mathf.InverseLerp(minMaxValue.x, minMaxValue.y, ropeValue);
    //     var newPos = Mathf.Lerp(minMaxHorizontalValue.x, minMaxHorizontalValue.y, ropeValue);
    //     var currentHorizontalPos = Mathf.Lerp(transform.position.y, newPos, 0.1f);
    //     pos.x = currentHorizontalPos;
    //     transform.position = pos;
    // }
}
