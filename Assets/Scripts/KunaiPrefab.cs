using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    Transform rotate;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 50);
        Destroy(gameObject, 1);
    }
}
