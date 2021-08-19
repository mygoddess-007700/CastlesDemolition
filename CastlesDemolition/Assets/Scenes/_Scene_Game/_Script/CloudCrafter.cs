using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspecter")]
    public int numClouds = 40; //要创建云朵的数量
    /*    public GameObject[] cloudPrefabs; //云朵预设的数组>???????*/
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10); //云朵位置的下限
    public Vector3 cloudPosMax = new Vector3(150, 100, 10); //云朵位置的上限
    public float cloudScaleMin = 1; //云朵的最小缩放比例
    public float cloudScaleMax = 2; //云朵的最大缩放比例
    public float cloudSpeedMult = 0.5f; //调整云朵速度

    public GameObject[] cloudInstances;

    private void Awake()
    {
        //创建一个cloudInstances数组，用于存储所有云朵的实例
        cloudInstances = new GameObject[numClouds];
        //查找CloudAnchor父对象
        GameObject anchor = GameObject.Find("CloudAnchor");
        //遍历Cloud并创建实例
        GameObject cloud;
        for(int i=0; i < numClouds; i++)
        {
            //创建cloudPrefab实例
            cloud = Instantiate<GameObject>(cloudPrefab);
            //设置云朵位置
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //设置云朵缩放比例
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //较小的云朵（即scaleU值较小）离地面较近
            cPos.y = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //较小的距离较远
            cPos.z = 100 - 90 * scaleU;
            //将上述变换数值应用到云朵
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //使云朵成为CloudAnchor的子对象
            cloud.transform.parent = anchor.transform;
            //将云朵添加到CloudInstances数组中
            cloudInstances[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //遍历所有已创建的云朵
        foreach(GameObject cloud in cloudInstances)
        {
            //获取云朵的缩放比例和位置
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //云朵越大，移动速度越快
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //如果云朵已经位于画面左侧较远位置
            if(cPos.x <= cloudPosMin.x)
            {
                //则将它放置到最右侧
                cPos.x = cloudPosMin.x;
            }
            //将新位置应用到云朵上
            cloud.transform.position = cPos;
        }
    }
}
