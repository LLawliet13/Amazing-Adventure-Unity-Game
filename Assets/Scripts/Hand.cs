using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public int rotationOffset;
    Vector3 diff;
    // Update is called once per frame
    void Update()
    {
        
        if (transform.parent.name == "MainCharacter"|| LayerMask.LayerToName(transform.parent.gameObject.layer) == "Player")
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition)- transform.position;
        if(transform.parent.name.Contains("Boss"))
        {
            GameObject mainCharacter = GameObject.FindGameObjectWithTag("Player");
            if (mainCharacter == null)
                return;

            diff = mainCharacter.transform.position - transform.position;
        }

        diff = diff.normalized;
        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ + rotationOffset);
    }
    
}
