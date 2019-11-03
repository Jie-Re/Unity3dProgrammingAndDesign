using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour, IUserAction, ISceneController

{
    public SoompiFactory soompi_factory;
    public CoinFactory coin_factory;
    public Judge judge;
    public SoompiActionManager soompi_action_manager;
    //public SoompiFollowActionManager soompi_follow_manager;
    public int wall_sign = -1;//当前王一博所处哪个格子
    public GameObject wyb;
    public float wyb_speed = 3;//王一博移动速度
    public float rotate_speed = 130f;//王一博旋转速度
    private List<GameObject> coins;//场景中金币列表
    private List<GameObject> soompies;//场景中私生饭列表
    private bool game_over = false;//游戏结束标志
    public Camera main_camera; //主相机
    public SoompiCollide soompi_collide;
    public CoinCollide coin_collide;

    void Update()
    {
        for (int i = 0; i < soompies.Count; i++)
        {
            soompies[i].gameObject.GetComponent<SoompiData>().wall_sign = wall_sign;
        }
        //金币收集完毕
        if (judge.GetCoinNumber() == 0)
        {
            Gameover();
        }
    }
    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        soompi_factory = Singleton<SoompiFactory>.Instance;
        coin_factory = Singleton<CoinFactory>.Instance;
        soompi_action_manager = gameObject.AddComponent<SoompiActionManager>() as SoompiActionManager;
        //soompi_follow_manager = gameObject.AddComponent<SoompiFollowActionManager>() as SoompiFollowActionManager;
        LoadResources();
        judge = Singleton<Judge>.Instance;
        main_camera.GetComponent<CameraFollow>().follow = wyb;
        soompi_collide = Singleton<SoompiCollide>.Instance;
        coin_collide = Singleton<CoinCollide>.Instance;
    }

    //public void MovePlayer(int old_state, int cur_state)
    public void MovePlayer(float translationX, float translationZ)
    {
        if (!game_over)
        {
            
            //移动和旋转
            wyb.transform.Translate(translationX * wyb_speed * Time.deltaTime, 0, translationZ * wyb_speed * Time.deltaTime, Space.World);
            if (translationX < 0)
            {
                wyb.transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else if (translationX > 0)
            {
                wyb.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            if (translationZ < 0)
            {
                wyb.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (translationZ > 0)
            {
                wyb.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    public int GetScore()
    {
        return judge.GetScore();
    }

    public int GetCoinNumber()
    {
        return judge.GetCoinNumber();
    }

    public bool GetGameover()
    {
        return game_over;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
        game_over = false;
    }

    public void LoadResources()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/SnowTerrain"), new Vector3(-500,0,0), Quaternion.identity);
        wyb = Instantiate(Resources.Load("Prefabs/wyb"), new Vector3(0,1,20),Quaternion.identity) as GameObject;
        coins = coin_factory.GetCoins();
        soompies = soompi_factory.GetSoompies();
        //所有私生饭移动
        for (int i = 0; i < soompies.Count; i++)
        {
            soompi_action_manager.SoompiMove(soompies[i]);
        }
    }

    void OnEnable()
    {
        GameEventManager.GameoverChange += Gameover;
        GameEventManager.CoinChange += ReduceCoinNumber;
    }

    void OnDisable()
    {
        GameEventManager.GameoverChange -= Gameover;
        GameEventManager.CoinChange -= ReduceCoinNumber;
    }


    public void AddScore(int extra_score)
    {
        judge.AddScore(extra_score);
    }
    void ReduceCoinNumber()
    {
        judge.ReduceCoin();
    }
    void Gameover()
    {
        game_over = true;
        soompi_factory.StopSoompi();
        soompi_action_manager.DestroyAllAction();
    }
}
