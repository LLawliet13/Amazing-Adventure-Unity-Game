using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_ForEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Destroy(gameObject);
        else
            gameObject.transform.position 
                = Vector3.MoveTowards(gameObject.transform.position, 
                player.transform.position, 30 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var c = GetComponent<Collider2D>();
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
    
}
