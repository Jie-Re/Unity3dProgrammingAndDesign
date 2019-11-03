using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    private GUIStyle score_style = new GUIStyle();
    private GUIStyle text_style = new GUIStyle();
    private GUIStyle over_style = new GUIStyle();
    public int show_time = 8;//展示提示的时间长度
    //private int old_move_state = 0;
    //private int cur_move_state = 0;

    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
        text_style.normal.textColor = new Color(0, 0, 0, 1);
        text_style.fontSize = 16;
        score_style.normal.textColor = new Color(1, 0.92f, 0.016f, 1);
        score_style.fontSize = 16;
        over_style.fontSize = 25;
        //展示提示
        StartCoroutine(ShowTip());
    }

    // Update is called once per frame
    void Update()
    {
        //获取方向键的偏移量
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");
        //移动玩家
        action.MovePlayer(translationX, translationZ);
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 200, 50), "分数：", text_style);
        GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), score_style);
        GUI.Label(new Rect(Screen.width - 170, 5, 50, 50), "剩余物品数：", text_style);
        GUI.Label(new Rect(Screen.width - 80, 5, 50, 50), action.GetCoinNumber().ToString(), score_style);
        if (action.GetGameover() && action.GetCoinNumber() != 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), "游戏结束", over_style);
            if (GUI.Button(new Rect(Screen.width/2-50,Screen.width/2-150,100,50),"重新开始"))
            {
                action.Restart();
                return;
            }
        }
        else if (action.GetCoinNumber() == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), "恭喜胜利！", over_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "重新开始"))
            {
                action.Restart();
                return;
            }
        }
        if (show_time > 0) {
            GUI.Label(new Rect(Screen.width / 2 - 128, 10, 100, 100), "耶啵又要为了大摩托努力营业挣钱了，", text_style);
            GUI.Label(new Rect(Screen.width / 2 - 80, 30, 100, 100), "可是路上好多私生饭！", text_style);
            GUI.Label(new Rect(Screen.width / 2 - 200, 50, 100, 100), "mtjj们快来帮耶啵摆脱私生饭并获取值钱的物品吧！", text_style);
            GUI.Label(new Rect(Screen.width / 2 - 80, 70, 100, 100), "按WSAD或方向键移动", text_style);
            GUI.Label(new Rect(Screen.width / 2 - 120, 90, 100, 100), "采集不同物品可以获得不同分数噢", text_style); 
                 GUI.Label(new Rect(Screen.width / 2 - 96, 110, 100, 100), "采集完所有的物品即可获胜", text_style);
        }
    }

    public IEnumerator ShowTip()
    {
        while (show_time >= 0)
        {
            yield return new WaitForSeconds(1);
            show_time--;
        }
    }
}
