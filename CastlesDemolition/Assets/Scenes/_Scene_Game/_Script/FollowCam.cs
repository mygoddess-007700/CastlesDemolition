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

                //��ȡ��Ȥ���λ��
                Vector3 destination = POI.transform.position;*/

        Vector3 destination;
        //�����Ȥ�㣨POI�������ڣ����ص�P:[0, 0, 0]
        if(POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //��ȡ��Ȥ���λ��
            destination = POI.transform.position;
            //�����Ȥ����һ��Projectileʵ����������Ƿ��Ѿ���ֹ
            if(POI.tag == "Projectile")
            {
                //���������sleeping״̬����Ϊ�ƶ���
                if(POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //���ص�Ĭ����ͼ
                    POI = null;
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //���������ǰλ�ú�Ŀ��λ��֮�������ֵ
        destination = Vector3.Lerp(transform.position, destination, easing);
        //����destination.z��ֵΪcamZ��ʹ������㹻Զ
        destination.z = camZ;
        //�������λ�����õ�destination
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}
