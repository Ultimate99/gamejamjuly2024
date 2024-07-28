using System;
using UnityEngine;
using Utils;
using Utils.PowerupSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private MovementControler movementControler;
    [SerializeField] int CurrentHealth = 6;
    [SerializeField] int MaxiumumHealth = 6;
    [SerializeField] Dash dashing;

    public event EventHandler<HealedEventArgs> Healed;
    public event EventHandler<DamagedEventArgs> Damaged;

    public Animator animator;

    // Update is called once per frame
    private void Update()
    {
        movementControler.time += Time.deltaTime;
        movementControler.PlayerInput();
    }

    void FixedUpdate()
    {
        movementControler.CheckCollisions();
        
        movementControler.HandleJump();
        movementControler.HandleDirection();
        movementControler.HandleGravity();
        movementControler.WallSlide();
        movementControler.Dashing();

        movementControler.ApplyMovement();
    }

    public void Heal(int amount)
    {
        var newHealth = Math.Min(CurrentHealth + amount, MaxiumumHealth);
        if (Healed != null)
            Healed(this, new HealedEventArgs(newHealth - CurrentHealth));
        CurrentHealth = newHealth;
    }
    public void Damage(int amount)
    {
        var newHealth = Math.Max(CurrentHealth - amount, 0);
        if (Damaged != null)
            Damaged(this, new DamagedEventArgs(CurrentHealth - newHealth));
        CurrentHealth = newHealth;
    }

    public class HealedEventArgs : EventArgs
    {
        public HealedEventArgs(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; private set; }
    }

    public class DamagedEventArgs : EventArgs
    {
        public DamagedEventArgs(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; private set; }
    }

}
