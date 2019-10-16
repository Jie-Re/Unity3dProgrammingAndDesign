using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    //每个GUI的style
    GUIStyle bold_style = new GUIStyle();
    GUIStyle score_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    GUIStyle over_style = new GUIStyle();
    GUIStyle green_style = new GUIStyle();
    private int high_score = 0;            //最高分
    public int sign = 0;


    void Start()
    {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;

        bold_style.normal.textColor = new Color(1, 0, 0);
        bold_style.fontSize = 16;
        text_style.normal.textColor = new Color(0, 0, 0, 1);
        text_style.fontSize = 16;
        score_style.normal.textColor = new Color(1, 0, 1, 1);
        score_style.fontSize = 16;
        over_style.normal.textColor = new Color(1, 0, 0);
        over_style.fontSize = 25;
    }

    void OnGUI()
    {
        if (action.GetLife() == 0)
        {
            sign = 2;
            action.GameOver();
        }

        if (sign == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 30, Screen.width / 2 - 350, 100, 100), "HitUFO!", over_style);
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.width / 2 - 220, 400, 100), "大量UFO出现，点击它们，即可消灭，快来加入战斗吧", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.width / 2 - 150, 100, 50), "游戏开始"))
            {
                action.StartGame();
                sign = 1;
            }
        }
        else if (sign == 1)
        {
            //用户射击
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 pos = Input.mousePosition;
                action.Hit(pos);
            }

            GUI.Label(new Rect(10, 5, 200, 50), "分数:", text_style);
            GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), score_style);

            GUI.Label(new Rect(Screen.width - 120, 5, 50, 50), "生命:", text_style);
        }
        else if (sign == 2)
        {
            high_score = high_score > action.GetScore() ? high_score : action.GetScore();
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.width / 2 - 250, 100, 100), "游戏结束", over_style);
            GUI.Label(new Rect(Screen.width / 2 - 10, Screen.width / 2 - 200, 50, 50), "最高分:", text_style);
            GUI.Label(new Rect(Screen.width / 2 + 50, Screen.width / 2 - 200, 50, 50), high_score.ToString(), text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.width / 2 - 150, 100, 50), "重新开始"))
            {
                action.ReStart();
                sign = 1;
            }
        }

        //显示当前生命值
        if (sign == 1 && (action.GetLife() == 6 || action.GetLife() == 5))//安全
        {
            for (int i = 0; i < action.GetLife(); i++)
            {
                GUIStyle green_style = new GUIStyle();
                green_style.normal.textColor = new Color(0, 1, 0);
                green_style.fontSize = 16;
                GUI.Label(new Rect(Screen.width - 75 + 10 * i, 5, 50, 50), "X", green_style);
            }
        }
        else if (sign == 1 && (action.GetLife() == 4 || action.GetLife() == 3))//警告
        {
            for (int i = 0; i < action.GetLife(); i++)
            {
                GUIStyle yellow_style = new GUIStyle();
                yellow_style.normal.textColor = new Color(1, 1, 0);
                yellow_style.fontSize = 16;
                GUI.Label(new Rect(Screen.width - 75 + 10 * i, 5, 50, 50), "X", yellow_style);
            }
        }
        else if (sign == 1 && (action.GetLife() == 2 || action.GetLife() == 1))//危险
        {
            for (int i = 0; i < action.GetLife(); i++)
            {
                GUIStyle red_style = new GUIStyle();
                red_style.normal.textColor = new Color(1, 0, 0);
                red_style.fontSize = 16;
                GUI.Label(new Rect(Screen.width - 75 + 10 * i, 5, 50, 50), "X", red_style);
            }
        }
    }
}