using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    private int score;//分数
    private int life;//生命值
    void Start()
    {
        score = 0;
        life = 6;
    }
    //记录分数
    public void Record(GameObject disk)
    {
        int temp = disk.GetComponent<UFOData>().score;
        score = temp + score;
    }
    //扣除生命值
    public void ReduceLife()
    {
        life--;
    }
    //获取分数
    public int GetJudgeScore()
    {
        return score;
    }
    //获取生命值
    public int GetJudgeLife()
    {
        return life;
    }
    //死亡
    public void Over()
    {
        life = 0;
    }
    //重置
    public void Reset()
    {
        score = 0;
        life = 6;
    }
}
