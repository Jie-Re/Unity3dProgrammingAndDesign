using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction //用户动作“列表”接口
{
    //开始游戏
    void StartGame();
    //用户点击游戏界面
    void Hit(Vector3 pos);
    //游戏重新开始
    void ReStart();
    //游戏结束
    void GameOver();
    //获得分数
    int GetScore();
    //获得生命值
    int GetLife();
}