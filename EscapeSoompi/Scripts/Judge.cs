using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public Controller sceneController;
    private int score = 0;//分数
    private int coin_number = 9;//金币数量
    // Start is called before the first frame update
    void Start()
    {
        sceneController = (Controller)SSDirector.GetInstance().CurrentSceneController;
        sceneController.judge = this;
    }

    public int GetScore()
    {
        return score;
    }
    public void AddScore(int extra_score)
    {
        score += extra_score;
    }
    public int GetCoinNumber()
    {
        return coin_number;
    }
    public void ReduceCoin()
    {
        coin_number --;
    }

}
