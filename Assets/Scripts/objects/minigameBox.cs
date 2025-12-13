using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigameBox : MonoBehaviour
{
    public boxDataSO data;
    [SerializeField] private sortingBoxesManager manager;

    private Vector3 startPosition;
    private bool isDragging = false;
    private SpriteRenderer spriteRenderer;

    public void Initialize(boxDataSO newData, sortingBoxesManager newManager)
    {
        data = newData;
        manager = newManager;

        spriteRenderer = GetComponent<SpriteRenderer>();
        // Optional: Set the sprite on the box itself, or keep it generic
        // spriteRenderer.sprite = data.illustration; 
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    public void ReturnToStart()
    {
        transform.position = startPosition;
    }
}
