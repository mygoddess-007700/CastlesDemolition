using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { idle, playing, levelEnd };

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //˽�е������󣬼�ʵ�����Ľű����

    [Header("Set in Inspector")]
    public Text uitLevel; //UIText_Level�ı�
    public Text uitShots; //UIText_Shots�ı�
    public Text uitButton; //UIButton_View�ϵ��ı�
    public Vector3 castlePos; //���óǱ���λ��
    public GameObject[] castles; //�洢���гǱ����������

    [Header("Set Dynamically")]
    public int level; //��ǰ����
    public int levelMax; //���������
    public int shotsTaken; //
    public GameObject castle; //��ǰ�Ǳ�
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //�������ģʽ

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void Update()
    {
        UpdateGUI();

        //����Ƿ�����ɸü���
        if (mode == GameMode.playing && Goal.goalMet)
        {
            //����ɼ���ʱ���ı�mode��ֹͣ���
            mode = GameMode.levelEnd;
            //��С�������
            SwitchView("Show Both");
            //��2���ʼ��һ����
            Invoke("NextLevel", 2f);
        }
    }

    void StartLevel()
    {
        //����Ǳ��Ѿ����ڣ������ԭ�еĳǱ�
        if(castle != null)
        {
            Destroy(castle);
        }

        //���ԭ�еĵ���
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //ʵ�����³Ǳ�
        castle = Instantiate(castles[level], castlePos, Quaternion.identity);
        shotsTaken = 0;

        //���������λ��
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //����Ŀ��״̬
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        //��ʾGUITexts�е�����
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if(eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
                {
                    FollowCam.POI = null;
                    uitButton.text = "Show Castle";
                    break;
                }
            case "Show Castle":
                {
                    FollowCam.POI = S.castle;
                    uitButton.text = "Show Both";
                    break;
                }
            case "Show Both":
                {
                    FollowCam.POI = GameObject.Find("ViewBoth");
                    uitButton.text = "Show Slingshot";
                    break;
                }
        }
    }

    //�����ڴ�������λ�����ӷ�������ľ�̬����
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
