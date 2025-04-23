using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int pesosAmount = 5;
    protected override void onCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            
            // Give pesos to player
            GameManager.instance.pesos += pesosAmount;

            // Save the state
            GameManager.instance.SaveState();
            
            //+5 pesos
            GameManager.instance.ShowText("+" + pesosAmount + "pesos!", 25, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
            //Debug.Log("Grant " + pesosAmount + " pesos!");
            //Debug.Log("Total Pesos: " + GameManager.instance.pesos);
        }
    }
}
