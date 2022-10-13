using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pet_Landing : MonoBehaviour
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
        Vector3 target = new Vector3(master.transform.position.x - 6, transform.position.y, 0);
        transform.localPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Debug.Log("X:"+target.x);
    }
}
