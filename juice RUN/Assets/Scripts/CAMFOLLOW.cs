using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
public class CAMFOLLOW : MonoBehaviour
{
    public static CAMFOLLOW inst;
    public Transform target;
    public float smoothspeed = 0.12f;
    public Vector3 offset;
    public SplineFollower SF;
    private void Awake()
    {
        inst = this;
        SF = transform.parent.GetComponent<SplineFollower>();

    }
    // Start is called before the first frame update
   
    public bool follow = false,rotatearound=false;
    private void Start()
    {
       
    }
    private void LateUpdate()
    {
        if (follow)
        {
            if (target != null)
            {

                Vector3 desirepos = new Vector3(target.position.x, 0, target.position.z) + offset;
                Vector3 smoothpos = Vector3.Lerp(transform.position, desirepos, smoothspeed);
                transform.position = smoothpos;

            }
        }
        if (rotatearound)
        {
            transform.RotateAround(PLAYER.inst.transform.position, new Vector3(0.0f, 1.0f, 0.0f), -10 * Time.deltaTime);
        }
    }
}
