using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    //游戏结束变化
    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverChange;
    //金币数量变化
    public delegate void CoinEvent();
    public static event CoinEvent CoinChange;
    //减少金币数量
    public void ReduceCoinNum()
    {
        if (CoinChange != null)
        {
            CoinChange();
        }
    }
    //游戏结束
    public void PlayerGameover()
    {
        if (GameoverChange != null)
        {
            GameoverChange();
        }
    }
}
