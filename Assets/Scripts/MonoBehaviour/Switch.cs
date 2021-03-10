using System;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Sprite spritePressed;
    public Sprite spriteNotPressed;

    public static event Action SwitchPressed;

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            spriteRenderer.sprite = value ? spritePressed : spriteNotPressed;
            _isActive = value;
        }
    }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        IsActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !IsActive)
        {
            IsActive = true;
            SwitchPressed?.Invoke();
        }
    }
}
