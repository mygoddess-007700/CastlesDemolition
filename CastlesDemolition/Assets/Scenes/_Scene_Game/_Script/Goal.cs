using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //���ڴ�������λ�÷��ʵľ�̬�ֶ�
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        //����������ײ��������ʱ
        //����Ƿ��ǵ���
        if(other.gameObject.tag == "Projectile")
        {
            //����ǵ��裬����goalMetΪtrue
            Goal.goalMet = true;
            //ͬʱ����ɫ�Ĳ�͸�������õĸ���

            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
