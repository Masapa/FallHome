using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
     
    Transform target1, target2;
    float vertExtent;
    float horzExtent;
    void Start()
    {
        target1 = GameObject.Find("Player").transform;
        target2 = GameObject.Find("Earth").transform;    
    }

    void LateUpdate()
    {
        Vector3 tmp = target1.position;
        tmp.z = -10;
        transform.position = tmp;



    }
}
