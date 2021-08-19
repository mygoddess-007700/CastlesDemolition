using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    public LineRenderer line;
    private GameObject _poi;
    public List<Vector3> points;

    private void Awake()
    {
        S = this;
        //��ȡ������Ⱦ��������
        line = GetComponent<LineRenderer>();
        //����Ҫʹ��LineRenderer֮ǰ���������
        line.enabled = false;
        //��ʼ����ά�������list
        points = new List<Vector3>();
    }

    //����һ������
    public GameObject poi
    {
        get { return (_poi); }
        set 
        {
            _poi = value;
            if(_poi != null)
            {
                //����_poi����Ϊ�¶���ʱ������λ����������
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //�����������ֱ���������
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //���������������һ����
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //����õ�����һ�����λ�ò���Զ���򷵻�
            return;
        }
        if(points.Count == 0)
        {
            //����Ƿ����
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            //���һ������������֮����׼
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2; //����ǰ������
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //��������Ⱦ��
            line.enabled = true;
        }
        else
        {
            //������ӵ�Ĳ���
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //���������ӹ��ĵ��λ��
    public Vector3 lastPoint
    {
        get 
        {
            if(points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    private void FixedUpdate()
    {
        if(poi == null)
        {
            //�����Ȥ�㲻���ڣ����ҳ�һ��
            if(FollowCam.POI != null)
            {
                //�����Ȥ�㲻���ڣ����ҳ�һ��
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        //���������Ȥ�㣬����FixUpdate������λ������һ����
        AddPoint();
        if(FollowCam.POI == null)
        {
            //��FollowCam.POIΪnullʱ��ʹ��ǰpoiҲΪnull
            poi = null;
        }
    }
}
