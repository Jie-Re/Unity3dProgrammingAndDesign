using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : System.Object
{
    private static SSDirector _instance;

    //获取当前游戏的场景
    public ISceneController CurrentSceneController { get; set; }

    //创建单实例
    public static SSDirector GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }
}
