using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCatcher : MonoBehaviour
{
    public MGR manager;
    public bool tracking;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            if(tracking)
                manager.UpdateMisses(1);

            Destroy(collision.gameObject);
        }
    }
}
