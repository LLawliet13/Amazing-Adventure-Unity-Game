using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBallController : MonoBehaviour
{
    Transform firePoint;
    void Start()
    {
        firePoint = transform.parent.GetComponent<Transform>();
        mc = transform.parent.parent.parent;
        scale = Mathf.Abs(mc.transform.localScale.x);

    }
    public float delayTime = 0.5f;
    float firedTime = 0;
    public Transform LightingBall;
    // Update is called once per frame
    void Update()
    {
        //if (delayTime == 0)
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        shoot();
        //    }
        //}
        //else
        //{
        //    if (Input.GetMouseButton(0) && Time.time > firedTime)
        //    {
        //        shoot();
        //        firedTime = Time.time + delayTime;
        //    }
        //}
    }
    Transform mc;
    float scale;
    public void shoot()
    {
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        Transform lightingball = Instantiate(LightingBall, firePointPosition, firePoint.rotation);
        lightingball.transform.localScale *= Mathf.Abs(mc.transform.localScale.x) / scale;
        lightingball.GetComponent<LightingBall>().setCenter(firePoint);
    }

}
