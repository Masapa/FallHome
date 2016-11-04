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
        Vector3 tmp = target1.position + (target2.position - target1.position) / 2 ;
        tmp.z = -10;
        Camera.main.transform.position = tmp;
        Camera.main.orthographicSize = Vector3.Distance(target1.position, target2.position) / 2 ;
        Debug.Log(Camera.main.orthographicSize);




    }
}
