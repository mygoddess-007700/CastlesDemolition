/*
 ��ײ���͸��壺
1��ֻҪ�и�������ײ����������������ײ����ײ�ͻ�����������ײ������
2��û�и��壬����ײ�������屻һ��ӵ�и������ײ����ײ����������������ײ��������
3��û�и����������ײ����ײ��������κ���ײ������

 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    //��Unity������������õ��ֶ�
    [Header("Set in Inspecter")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //��̬���õ��ֶ�
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

    private void Awake() //�����Ľ���������Ⱦ
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

    private void OnMouseExit() //Ӧ��Ϊ��ס����֮ǰ�й⻷������Ӧ��Ҳ�й⻷
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //������������ͣ�ڵ����Ϸ�ʱ������������
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;

        //���õ�ǰ��isKinematic
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode == true)
            return;

        //��ȡ�������2D���ڵ�����
        Vector3 mousePos2D = Input.mousePosition;
        //print(mousePos2D); ���½���ԭ��,����3D�������������½ǲ���ԭ��

        //������2D����ת����3D����
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //����launchPos��mousePos3D����֮��������
        Vector3 mouseDelta = mousePos3D - launchPos; //Delta x
        //��mouseDelta����������ڵ�������״��ײ���뾶��Χ��
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //��projectile�ƶ�����λ��
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            //������ɿ����
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
