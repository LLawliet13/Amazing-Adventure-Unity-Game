using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiFireController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform firePoint;
    Transform mc;

    void Start()
    {
        firePoint = transform.parent.GetComponent<Transform>();
        mc = transform.parent.parent.parent;
        scale = Mathf.Abs(mc.transform.localScale.x);
       
    }
    public float delayTime = 0.5f;
    public Transform KunaiPrefab;
    public Transform CuuViFirePrefab;
    // Update is called once per frame
    void Update()
    {
        
    }
    float scale;
    int rotationOffset = 0;
    
    public float camEdgePosition(String edge)
    {
        Camera cam = Camera.main;
        if (cam)
        {
            if (edge == "bottom")
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
            if (edge == "right")
                return cam.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;
            if (edge == "top")
                return cam.ViewportToWorldPoint(new Vector3(0f, 1.0f, 0f)).y;
            if (edge == "left")
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        }
        return 0.0f;

    }
    public GameObject AutoDetect()
    {
        float left = camEdgePosition("left");
        float right = camEdgePosition("right");
        float bottom = camEdgePosition("bottom");
        float top = camEdgePosition("top");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            float x = enemy.transform.position.x;
            float y = enemy.transform.position.y;
            if (x >= left && x <= right &&
                y >= bottom && y <= top)
            {
                return enemy;
            }
        }
        return null;
    }
  
    public void shoot(string prefabName)
    {
        Transform weapon = null;
        if(prefabName == "kunai")
        {
            weapon = KunaiPrefab;
        }
        if(prefabName == "cuuviFire")
        {
            weapon = CuuViFirePrefab;
        }
        if (weapon == null) return;
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        Transform kunai = Instantiate(weapon, firePointPosition, firePoint.rotation);
        kunai.transform.localScale *= Mathf.Abs(mc.transform.localScale.x) / scale;
    }
    public void shootAuto(string prefabName)
    {
        Transform weapon = null;
        if (prefabName == "kunai")
        {
            weapon = KunaiPrefab;
        }
        if (prefabName == "cuuviFire")
        {
            weapon = CuuViFirePrefab;
        }
        if (weapon == null) return;
        GameObject target = AutoDetect();
        if (target != null)
        {
            Debug.Log(target.name);
            Vector3 diff = target.transform.position - transform.position;
            diff = diff.normalized;
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.parent.rotation = Quaternion.Euler(0, 0, rotZ + rotationOffset);

            Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
            Transform kunai = Instantiate(weapon, firePointPosition, transform.parent.rotation);
            kunai.transform.localScale *= Mathf.Abs(mc.transform.localScale.x) / scale;
        }
        else
        {
            Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
            Transform kunai = Instantiate(weapon, firePointPosition, transform.parent.rotation);
            kunai.transform.localScale *= Mathf.Abs(mc.transform.localScale.x) / scale;
        }
    }

}
