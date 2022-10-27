using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject followObject;
    public Vector2 followOffset;
    private Vector2 threshold;
    public float speed = 15;
    Vector3 scaleFollowObject;
    void Start()
    {
        threshold = caculateThreshold();
        scaleFollowObject = followObject.transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followObject != null)
        {
            speed = speed * (Math.Abs(followObject.transform.localScale.x / scaleFollowObject.x));
            //Debug.Log(speed);
            scaleFollowObject = followObject.transform.localScale;
            Vector2 follow = followObject.transform.position;
            float xDiff = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
            float yDiff = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);
            Vector3 newPosition = transform.position;
            if (Mathf.Abs(xDiff) >= threshold.x)
            {
                newPosition.x = follow.x;
            }
            if (Mathf.Abs(yDiff) >= threshold.y)
            {
                newPosition.y = follow.y;
            }
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
        }
        else
        {
            followObject = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private Vector3 caculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }
    public float xOffset;
    private void OnDrawGizmos()
    {
        Vector3 positionGiz = new Vector3(transform.position.x - xOffset, transform.position.y,
            0);
        Gizmos.color = Color.green;
        Vector2 border = caculateThreshold();
        Gizmos.DrawWireCube(positionGiz, new Vector3(border.x * 2, border.y * 2, 1));

    }
}
