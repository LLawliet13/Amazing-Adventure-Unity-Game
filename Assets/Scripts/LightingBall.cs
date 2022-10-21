using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    public Transform lighting_ball_point_center;
    public float radius = 5f;
    public LayerMask Player;
    private float delayAttackTime = 1f;
    private float attackTime = 0;
    void Update()
    {
        if (lighting_ball_point_center == null) return;
        if (Time.time > attackTime)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(lighting_ball_point_center.position, radius, Player);
            foreach (Collider2D e in collisions)
            {

                e.GetComponent<CharacterStats>().getDamage(5000);

            }
            attackTime = Time.time + delayAttackTime;
        }
    }

    internal void setCenter(Transform firePointPosition)
    {
        lighting_ball_point_center = firePointPosition;
    }
}
