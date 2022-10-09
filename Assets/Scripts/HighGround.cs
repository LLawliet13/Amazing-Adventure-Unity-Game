using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighGround : MonoBehaviour
{
    Vector3 targetPositon;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        targetPositon = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != targetPositon)
        {
            transform.position =  Vector3.MoveTowards(transform.position, targetPositon, 100 * Time.deltaTime);
        }
        else
        {
            targetPositon = startPosition;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        targetPositon = startPosition + new Vector3(0, 6, 0);
        Debug.Log("nay len");
    }
}
