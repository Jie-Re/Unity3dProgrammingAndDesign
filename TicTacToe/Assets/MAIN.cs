using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAIN : MonoBehaviour
{
    private int dimension = 3;
    private int[,] map;
    private int player;
    private float buttonWidth = 50;
    private float buttonHeight = 50;
    private float originX, originY;
    private float restartWidth = 100;
    private float restartHeight = 50;
    private float winboxWidth = 200;
    private float winboxHeight = 50;
    public Texture2D backgrounImg;
    public Texture2D img1;
    public Texture2D img2;

    void reset()//重置
    {
        map = new int[dimension, dimension];
        player = 1;
        for (int i = 0; i < dimension; i ++)
        {
            for (int j = 0; j < dimension; j ++)
            {
                map[i, j] = 0;
            }
        }
    }

    //返回0 - 游戏中，返回1 - player1胜利，返回2 - player2胜利
    int check()
    {
        //一行连续三个为同一种棋子，则该种棋子胜利
        for (int i = 0; i < dimension; i ++)
        {
            for (int j = 0; j <= dimension-3; j ++)
            {
                if (map[i, j] == map[i, j + 1] && map[i, j] == map[i, j + 2] && map[i,j] != 0)
                {
                    return map[i, j];
                }
            }
        }

        //一列连续三个为同一种棋子，则该种棋子胜利
        for (int i = 0; i < dimension; i ++)
        {
            for (int j = 0; j <= dimension-3; j ++)
            {
                if (map[j, i] == map[j+1, i] && map[j+2, i] == map[j, i] && map[j,i] != 0)
                    return map[j, i];
            }
        }

        //对角线(left-to-right)连续三个为同一种棋子，则该种棋子胜利
        for (int i = 0; i <= dimension-3; i ++)
        {
            for (int j = 0; j <= dimension-3; j ++)
            {
                if (map[i, j] == map[i + 1, j + 1] && map[i, j] == map[i + 2, j + 2] && map[i,j] != 0) return map[i, j];
            }
        }
        //对角线(right-to-left)连续三个为同一种棋子，则该种棋子胜利
        for (int i = 2; i < dimension; i ++)
        {
            for (int j = 0; j <= dimension-3; j ++)
            {
                if (map[i, j] == map[i - 1, j + 1] && map[i, j] == map[i - 2, j + 2] && map[i, j] != 0) return map[i, j];
            }
        }

        return 0;
    }
    // Start is called before the first frame update
    void Start()//开局重置
    {
        originX = (float)(Screen.width / 2.0 - dimension / 2.0 * buttonWidth);
        originY = (float)(Screen.height / 2.0 - dimension / 20 * buttonHeight);
        reset();
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUIStyle wholeStyle = new GUIStyle();
        wholeStyle.normal.background = backgrounImg;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", wholeStyle);

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 45;
        titleStyle.normal.textColor = new Color(255 / 255f, 288 / 255f, 181 / 255f) ;
        titleStyle.fontStyle = FontStyle.Italic;
        GUI.Label(new Rect(0, 0, Screen.width, 50), "TicTacToe", titleStyle);

        GUI.backgroundColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);
        if (GUI.Button(new Rect((float)(Screen.width/2.0-restartWidth/2.0), (float)(originY+dimension*buttonHeight), restartWidth, restartHeight), "<color=#ADD8E6>Restart</color>"))
            reset();
        int result = check();
        if (result == 1)
            GUI.Label(new Rect((float)(Screen.width / 2.0 - winboxWidth / 2.0), (float)(originY -winboxHeight), winboxWidth, winboxHeight), "<color=#FFA500><size=30>Player 1 Wins!</size></color>");
        else if (result == 2)
            GUI.Label(new Rect((float)(Screen.width / 2.0 - winboxWidth / 2.0), (float)(originY -winboxHeight), winboxWidth, winboxHeight), "<color=#FFA500><size=30>Player 2 Wins!</size></color>");

        for (int i = 0; i < dimension; i ++)
        {
            for (int j = 0; j < dimension; j ++)
            {
                float positionX = originX + buttonWidth * i;
                float positionY = originY + buttonHeight * j;
                if (map[i, j] == 1)
                    GUI.Button(new Rect(positionX, positionY, buttonWidth, buttonHeight), img1);
                else if (map[i, j] == 2)
                    GUI.Button(new Rect(positionX, positionY, buttonWidth, buttonHeight), img2);
                else
                {
                    if (GUI.Button(new Rect(positionX, positionY, buttonWidth, buttonHeight), "")) {
                        if (result == 0)
                        {
                            if (player == 1)
                            {
                                map[i, j] = 1;
                                player = 2;
                            }
                            else
                            {
                                map[i, j] = 2;
                                player = 1;
                            }
                        }
                    }
                }
            }
        }
    }
}
