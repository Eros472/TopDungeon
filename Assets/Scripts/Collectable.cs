using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    //Logic
    protected bool collected;

    protected override void onCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            onCollect();
    }

    protected virtual void onCollect()
    {
        collected = true;
    }
}
