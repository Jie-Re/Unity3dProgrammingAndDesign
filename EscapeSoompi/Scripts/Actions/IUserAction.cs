using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    //移动玩家
    void MovePlayer(float translationX, float translationZ);
    //得到分数
    int GetScore();
    //得到硬币数量
    int GetCoinNumber();
    //得到游戏结束标志
    bool GetGameover();
    //重新开始
    void Restart();

}
