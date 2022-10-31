using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Data.SqlTypes;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyFollowerType : MonoBehaviour
{
    // muc tieu se truy tim neu xuat hien trong tam cua eneny
    public Transform target;
    // so lan update waypoint tiep theo trong 1s
    public float updateRate = 2;
    private Seeker seeker;
    private Rigidbody2D rb;

    //duong toi vi tri target(neu co)
    public Path path;

    public float speed = 20f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnd = false;
    //khoang cach lon nhat giua game object toi dia diem di chuyen tiep theo tren path duoc coi la vat da cham waypoint
    public float nextWayPointDistance = 0.5f;
    //diem bat dau cua 1 path
    public int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            Debug.Log("No Target Found in start");
        }
        //tao duong toi vi tri target va tra ket quar cho ham OnPathComplete

        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }

    private void OnPathComplete(Path p)
    {
        if (p.error) {
            //Debug.LogError("this path has some problem to create");
        }
        else
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
                yield return false;
        }
        if (!StopPoint())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1 / updateRate);
        StartCoroutine(UpdatePath());
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            pathIsEnd = true;
            return;
        }
        pathIsEnd = false;
        //if (isStuck()) return;
        //prevPosition = transform.position;
        // vector toi diem tiep theo
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir = dir * speed * Time.fixedDeltaTime;

        // di chuyen
        if (!StopPoint())
            rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWayPointDistance)
            currentWaypoint++;



    }
    bool StopPoint()
    {
        if (target == null)
            return true;
        float distToTarget = Vector3.Distance(transform.position, target.position);
        if (distToTarget < nextWayPointDistance)
        {
            rb.velocity = new Vector3(0, 0, 0);
            return true;
        }
        return false;
    }
    //Vector3 prevPosition;
    //bool isStuck()
    //{
    //    if (prevPosition == null) return false;
    //    if (path.vectorPath.Count > currentWaypoint)
    //    {
    //        if (prevPosition == transform.position)
    //        {
    //            transform.Translate(path.vectorPath[currentWaypoint + 1]);
    //            currentWaypoint++;
    //            prevPosition = transform.position; 
    //            return true;
    //        }
    //        return false;
    //    }
    //    return false;
    //}
}
