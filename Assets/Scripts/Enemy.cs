using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();

        // Attempt to grab player reference
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            playerTransform = GameManager.instance.player.transform;
        }
        else
        {
            GameObject found = GameObject.Find("Player");
            if (found != null)
                playerTransform = found.transform;
            else
            {
                Debug.LogWarning("Enemy.cs: Could not find player!");
                return;
            }
        }

        startingPosition = transform.position;

        if (transform.childCount > 0)
        {
            hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        }
        else
        {
            Debug.LogWarning("Enemy.cs: Missing hitbox child!");
        }
    }

    protected void FixedUpdate()
    {
        if (playerTransform == null)
            return;

        float distance = Vector3.Distance(playerTransform.position, startingPosition);

        if (distance < chaseLength)
        {
            if (distance < triggerLength)
                chasing = true;

            if (chasing && !collidingWithPlayer)
            {
                UpdateMotor((playerTransform.position - transform.position).normalized);
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        // Check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null) continue;

            if (hits[i].CompareTag("Fighter") && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            hits[i] = null; // Clean manually
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, playerTransform.position, Vector3.up * 40, 1.0f);
    }
}