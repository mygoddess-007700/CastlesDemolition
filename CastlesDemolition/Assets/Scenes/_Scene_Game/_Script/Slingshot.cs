/*
 碰撞器和刚体：
1、只要有刚体有碰撞器，并且与其他碰撞器碰撞就会进入自身的碰撞器方法
2、没有刚体，有碰撞器的物体被一个拥有刚体的碰撞器碰撞，都会调用自身的碰撞器方法。
3、没有刚体的两个碰撞器相撞不会调用任何碰撞器方法

 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    //在Unity监视面板中设置的字段
    [Header("Set in Inspecter")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //动态设置的字段
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;

            return S.launchPos;
        }
    }

    private void Awake() //将来改进：缓慢渲染
    {
        S = this;

        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit() //应改为按住键且之前有光环，后来应该也有光环
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //玩家在鼠标光标悬停在弹弓上方时按下了鼠标左键
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;

        //设置当前的isKinematic
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode == true)
            return;

        //获取鼠标光标在2D窗口的坐标
        Vector3 mousePos2D = Input.mousePosition;
        //print(mousePos2D); 左下角是原点,但在3D场景场景中左下角不是原点

        //将鼠标的2D坐标转换成3D坐标
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //计算launchPos到mousePos3D两点之间的坐标差
        Vector3 mouseDelta = mousePos3D - launchPos; //Delta x
        //将mouseDelta坐标差限制在弹弓的球状碰撞器半径范围内
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //将projectile移动到新位置
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            //如果已松开鼠标
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
