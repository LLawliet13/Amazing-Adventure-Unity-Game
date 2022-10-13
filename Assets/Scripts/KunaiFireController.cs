using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiFireController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform firePoint;
    void Start()
    {
        firePoint = transform.parent.GetComponent<Transform>();
        mc = transform.parent.parent.parent;
        scale = Mathf.Abs(mc.transform.localScale.x);
       
    }
    public float delayTime = 0.5f;
    float firedTime = 0;
    public Transform KunaiPrefab;
    // Update is called once per frame
    void Update()
    {
        if(delayTime == 0)
        {
            if (Input.GetMouseButton(0))
            {
                shoot();
            }
        }
        else
        {
            if(Input.GetMouseButton(0)&& Time.time > firedTime)
            {
                shoot();
                firedTime = Time.time + delayTime;
            }
        }
    }
    Transform mc;
    float scale;
    void shoot()
    {
        
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x,firePoint.position.y);
        Transform kunai = Instantiate(KunaiPrefab, firePointPosition, firePoint.rotation);
        kunai.transform.localScale *= Mathf.Abs(mc.transform.localScale.x) / scale;
        Debug.Log(firePoint.rotation.z);
    }
    
}
