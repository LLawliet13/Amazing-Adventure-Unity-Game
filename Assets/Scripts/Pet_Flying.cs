using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Flying : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform master;
    public float speed = 10;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(master.transform.position.x - 6, master.transform.position.y+6, 0);
        transform.localPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Debug.Log("X:" + target.x);
    }
}
