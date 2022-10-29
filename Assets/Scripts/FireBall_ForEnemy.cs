using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_ForEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public LayerMask userLayer;
    public int ATK;
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
        if (LayerMask.LayerToName(collision.gameObject.layer) != LayerMask.LayerToName(findLayerNumber(userLayer)))
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
            {
                collision.GetComponent<CharacterStats>().getDamage(ATK);
            }
            if (LayerMask.LayerToName(collision.gameObject.layer) == "enemy")
            {
                collision.GetComponent<EnemyStats>().getDamage(ATK);
            }
            Destroy(gameObject);
        }
    }
    int findLayerNumber(int x)
    {
        for (int i = 0; i < 100; i++)
        {
            if ((1 << i) == x)
            {
                return i;
            }
        }
        return -1;
    }
}
