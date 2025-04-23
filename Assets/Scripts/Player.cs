using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //DontDestroyOnLoad(gameObject);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if(!isAlive)
            return;
            
        base.ReceiveDamage(dmg);    
        GameManager.instance.OnHitpointChange();
    }

    protected override void Death()
    {
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
    }
    
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        if(isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinId)
    {
        GetComponent<SpriteRenderer>().sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHitpoint++;
        hitpoint = maxHitpoint;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    public void Respawn()
    {
        hitpoint = maxHitpoint;
        isAlive = true;
        lastImmune = Time.time;
    }
}