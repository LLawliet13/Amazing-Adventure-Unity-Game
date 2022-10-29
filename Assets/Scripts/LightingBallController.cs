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
    public Transform LightingBall;
    // Update is called once per frame
    void Update()
    {
       
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
