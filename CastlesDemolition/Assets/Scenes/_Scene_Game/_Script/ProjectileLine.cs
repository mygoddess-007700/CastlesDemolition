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
        //获取对线渲染器的引用
        line = GetComponent<LineRenderer>();
        //在需要使用LineRenderer之前，将其禁用
        line.enabled = false;
        //初始化三维向量点的list
        points = new List<Vector3>();
    }

    //这是一个属性
    public GameObject poi
    {
        get { return (_poi); }
        set 
        {
            _poi = value;
            if(_poi != null)
            {
                //当把_poi设置为新对象时，将复位其所有内容
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //这个函数用于直接清除线条
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //用于在线条上添加一个点
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //如果该点与上一个点的位置不够远，则返回
            return;
        }
        if(points.Count == 0)
        {
            //如果是发射点
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            //添加一根线条，帮助之后瞄准
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2; //设置前两个点
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //启用线渲染器
            line.enabled = true;
        }
        else
        {
            //正常添加点的操作
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //返回最近添加过的点的位置
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
            //如果兴趣点不存在，则找出一个
            if(FollowCam.POI != null)
            {
                //如果兴趣点不存在，则找出一个
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
        //如果存在兴趣点，则在FixUpdate中它的位置增加一个点
        AddPoint();
        if(FollowCam.POI == null)
        {
            //当FollowCam.POI为null时，使当前poi也为null
            poi = null;
        }
    }
}
