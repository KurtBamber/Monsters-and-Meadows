using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCollision : MonoBehaviour
{
    public FollowManager FM;

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(FM.inGarden == false)
            {
                FM.inGarden = true;
            }
            else if(FM.inGarden == true)
            {
                FM.inGarden = false;
            }
        }
    }
}
