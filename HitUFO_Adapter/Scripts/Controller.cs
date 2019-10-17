using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, ISceneController, IUserAction
{
    private int round = 1;//回合
    private float speed = 2f;//发射一个飞碟的时间间隔
    private int score_round2 = 10;//去到第二回合所需分数
    private int score_round3 = 25;//去到第三回合所需分数

    public FlyActionManager action_manager;//动作管理
    public UFOFactory disk_factory;//飞碟工厂
    UserGUI user_gui;
    Judge judge;
    SpaceCraft spacecraft;

    private Queue<GameObject> disk_queue = new Queue<GameObject>();          //游戏场景中的飞碟队列
    private List<GameObject> disk_notshot = new List<GameObject>();          //没有被打中的飞碟队列
    private bool playing_game = false;                                       //游戏中
    private bool game_over = false;                                          //游戏结束
    private bool game_start = false;                                         //游戏开始

    private void SendDisk()
    {
        float position_x = 16;
        if (disk_queue.Count != 0)
        {
            GameObject disk = disk_queue.Dequeue();
            disk_notshot.Add(disk);
            disk.SetActive(true);
            //设置被隐藏了或是新建的飞碟的位置
            float ran_y = Random.Range(1f, 4f);
            float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<UFOData>().direction = new Vector3(ran_x, ran_y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<UFOData>().direction.x * position_x, ran_y, 0);
            disk.transform.position = position;
            //设置飞碟初始所受的力和角度
            float power = Random.Range(10f, 15f);
            float angle = Random.Range(15f, 28f);
            action_manager.UFOFly(disk, angle, power);
        }
    }

    private void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        disk_factory = Singleton<UFOFactory>.Instance;
        judge = Singleton<Judge>.Instance;
        action_manager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        spacecraft = new SpaceCraft();
    }

    void Update()
    {
        
        if (game_start)
        {
            //游戏结束，取消定时发送飞碟
            if (game_over)
            {
                CancelInvoke("LoadResources");
            }
            //设定一个定时器，发送飞碟，游戏开始
            if (!playing_game)
            {
                InvokeRepeating("LoadResources", 1f, speed);
                playing_game = true;
            }
            //发送飞碟
            SendDisk();
            //飞碟与飞机发生碰撞
            for (int i = 0; i < disk_notshot.Count; i++)
            {
                GameObject temp = disk_notshot[i];
                if ((spacecraft.GetJoyStickPosition() - temp.transform.position).magnitude < 0.5 && temp.gameObject.activeSelf == true)
                {
                    disk_factory.FreeDisk(disk_notshot[i]);
                    disk_notshot.Remove(disk_notshot[i]);
                    judge.Over();
                }
            }
            //减少生命值
            Debug.Log(disk_notshot.Count);
            for (int i = 0; i < disk_notshot.Count; i++)
            {
                GameObject temp = disk_notshot[i];
                //飞碟飞出摄像机视野也没被打中
                if (temp.transform.position.y < -3 && temp.gameObject.activeSelf == true)
                {
                    disk_factory.FreeDisk(disk_notshot[i]);
                    disk_notshot.Remove(disk_notshot[i]);
                    if (judge.GetJudgeLife() > 0) judge.ReduceLife();
                    //Debug.Log("Controller(84):ReduceBlood");
                    //Debug.Log(user_gui.life);
                }
            }
            //回合升级
            if (judge.GetJudgeScore() >= score_round2 && round == 1)
            {
                round = 2;
                //缩小飞碟发送间隔
                speed = speed - 0.6f;
                CancelInvoke("LoadResources");
                playing_game = false;
            }
            else if (judge.GetJudgeScore() >= score_round3 && round == 2)
            {
                round = 3;
                speed = speed - 0.5f;
                CancelInvoke("LoadResources");
                playing_game = false;
            }
        }
    }

    public void LoadResources()
    {
        //利用工厂加载UFO
        disk_queue.Enqueue(disk_factory.GetDisk(round));
        //加载spacecraft

    }

    //获得分数
    public int GetScore()
    {
        return judge.GetJudgeScore();
    }

    public int GetLife()
    {
        return judge.GetJudgeLife();
    }

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool not_hit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //射线打中物体
            if (hit.collider.gameObject.GetComponent<UFOData>() != null)
            {
                //射中的物体要在没有打中的飞碟列表中
                for (int j = 0; j < disk_notshot.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == disk_notshot[j].gameObject.GetInstanceID())
                    {
                        not_hit = true;
                    }
                }
                if (!not_hit)
                {
                    return;
                }
                disk_notshot.Remove(hit.collider.gameObject);
                //记分员记录分数
                judge.Record(hit.collider.gameObject);
                //等0.1秒后执行回收飞碟
                StartCoroutine(WaitingParticle(0.08f, hit, disk_factory, hit.collider.gameObject));
                //break;
            }
        }
    }

    public void StartGame()
    {
        game_start = true;
    }

    public void ReStart()
    {
        game_over = false;
        playing_game = false;
        judge.Reset();
        round = 1;
        speed = 2f;
    }

    public void GameOver()
    {
        game_over = true;
    }

    //暂停几秒后回收飞碟
    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, UFOFactory disk_factory, GameObject obj)
    {
        yield return new WaitForSeconds(wait_time);
        //等待之后执行的动作  
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.FreeDisk(obj);
    }
}