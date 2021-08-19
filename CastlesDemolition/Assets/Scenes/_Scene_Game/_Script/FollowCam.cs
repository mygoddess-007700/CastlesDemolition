using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //Point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamiacally")]
    public float camZ;

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        /*        if (POI == null)
                    return;

                //获取兴趣点的位置
                Vector3 destination = POI.transform.position;*/

        Vector3 destination;
        //如果兴趣点（POI）不存在，返回到P:[0, 0, 0]
        if(POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //获取兴趣点的位置
            destination = POI.transform.position;
            //如果兴趣点是一个Projectile实例，检查它是否已经静止
            if(POI.tag == "Projectile")
            {
                //如果它处于sleeping状态（即为移动）
                if(POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //返回到默认视图
                    POI = null;
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //在摄像机当前位置和目标位置之间增添插值
        destination = Vector3.Lerp(transform.position, destination, easing);
        //保持destination.z的值为camZ，使摄像机足够远
        destination.z = camZ;
        //将摄像机位置设置到destination
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}
