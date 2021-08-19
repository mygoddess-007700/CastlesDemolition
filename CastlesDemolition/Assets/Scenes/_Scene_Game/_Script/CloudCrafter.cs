using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspecter")]
    public int numClouds = 40; //Ҫ�����ƶ������
    /*    public GameObject[] cloudPrefabs; //�ƶ�Ԥ�������>???????*/
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10); //�ƶ�λ�õ�����
    public Vector3 cloudPosMax = new Vector3(150, 100, 10); //�ƶ�λ�õ�����
    public float cloudScaleMin = 1; //�ƶ����С���ű���
    public float cloudScaleMax = 2; //�ƶ��������ű���
    public float cloudSpeedMult = 0.5f; //�����ƶ��ٶ�

    public GameObject[] cloudInstances;

    private void Awake()
    {
        //����һ��cloudInstances���飬���ڴ洢�����ƶ��ʵ��
        cloudInstances = new GameObject[numClouds];
        //����CloudAnchor������
        GameObject anchor = GameObject.Find("CloudAnchor");
        //����Cloud������ʵ��
        GameObject cloud;
        for(int i=0; i < numClouds; i++)
        {
            //����cloudPrefabʵ��
            cloud = Instantiate<GameObject>(cloudPrefab);
            //�����ƶ�λ��
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //�����ƶ����ű���
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //��С���ƶ䣨��scaleUֵ��С�������Ͻ�
            cPos.y = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //��С�ľ����Զ
            cPos.z = 100 - 90 * scaleU;
            //�������任��ֵӦ�õ��ƶ�
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //ʹ�ƶ��ΪCloudAnchor���Ӷ���
            cloud.transform.parent = anchor.transform;
            //���ƶ���ӵ�CloudInstances������
            cloudInstances[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //���������Ѵ������ƶ�
        foreach(GameObject cloud in cloudInstances)
        {
            //��ȡ�ƶ�����ű�����λ��
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //�ƶ�Խ���ƶ��ٶ�Խ��
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //����ƶ��Ѿ�λ�ڻ�������Զλ��
            if(cPos.x <= cloudPosMin.x)
            {
                //�������õ����Ҳ�
                cPos.x = cloudPosMin.x;
            }
            //����λ��Ӧ�õ��ƶ���
            cloud.transform.position = cPos;
        }
    }
}
