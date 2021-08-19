using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { idle, playing, levelEnd };

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //私有单例对象，即实例化的脚本组件

    [Header("Set in Inspector")]
    public Text uitLevel; //UIText_Level文本
    public Text uitShots; //UIText_Shots文本
    public Text uitButton; //UIButton_View上的文本
    public Vector3 castlePos; //放置城堡的位置
    public GameObject[] castles; //存储所有城堡对象的数组

    [Header("Set Dynamically")]
    public int level; //当前级别
    public int levelMax; //级别的数量
    public int shotsTaken; //
    public GameObject castle; //当前城堡
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //摄像机的模式

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

        //检查是否已完成该级别
        if (mode == GameMode.playing && Goal.goalMet)
        {
            //当完成级别时，改变mode，停止检查
            mode = GameMode.levelEnd;
            //缩小画面比例
            SwitchView("Show Both");
            //在2秒后开始下一级别
            Invoke("NextLevel", 2f);
        }
    }

    void StartLevel()
    {
        //如果城堡已经存在，则清除原有的城堡
        if(castle != null)
        {
            Destroy(castle);
        }

        //清除原有的弹丸
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //实例化新城堡
        castle = Instantiate(castles[level], castlePos, Quaternion.identity);
        shotsTaken = 0;

        //重置摄像机位置
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //重置目标状态
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        //显示GUITexts中的数据
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

    //允许在代码任意位置增加发射次数的静态方法
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
