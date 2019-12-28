using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//表示下一状态要用船运走的角色
public enum BoatAction { P, D, PP, DD, PD }
/* P：船运载一个牧师
 * D：船运载一个恶魔
 * PP：船运载两个牧师
 * DD：船运载两个恶魔
 * PD：船运载一个牧师一个恶魔
 */

//船将要承载的船员的情况及船的状态
public struct nextPassenger
{
    public int boat_sign;//船在左岸（end）为-1，右岸（start）为1
    public BoatAction boat_action;
}

public class Controller : MonoBehaviour, ISceneController, IUserAction
{
    private const int RoleAmount = 3;

    public BankModel start_land;//开始陆地
    public BankModel end_land;//结束陆地
    public BoatModel boat;//船
    private RoleModel[] roles;//角色
    UserGUI user_gui;
    Judge judge;
    nextPassenger next;

    public MySceneActionManager actionManager;//动作管理

    // Start is called before the first frame update
    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        LoadResources();
        judge = new Judge(boat);
        actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager;
    }

    /*void Update()
    {
        if (user_gui.isTip)
        {
            getTips();
        }
    }*/

    public void LoadResources()//创建陆地，角色，船
    {
        start_land = new BankModel("right");
        end_land = new BankModel("left");
        boat = new BoatModel();
        roles = new RoleModel[RoleAmount * 2];
        Debug.Log("LoadResources()");

        for (int i = 0; i < RoleAmount; i++)
        {
            RoleModel role = new RoleModel("priest");
            role.SetName("priest" + i);
            role.SetPosition(start_land.GetEmptyPosition());
            role.GoLand(start_land);
            start_land.AddRole(role);
            roles[i] = role;
        }
        
        for (int i = 0; i < RoleAmount; i++)
        {
            RoleModel role = new RoleModel("devil");
            role.SetName("devil" + i);
            role.SetPosition(start_land.GetEmptyPosition());
            role.GoLand(start_land);
            start_land.AddRole(role);
            roles[i + RoleAmount] = role;
        }
    }

    public void MoveBoat()//移动船
    {
        if (boat.IsEmpty() || user_gui.sign != 0) return;
        //boat.BoatMove();
        //动作分离版本改变
        actionManager.moveBoat(boat.getGameObject(), boat.BoatMoveToPosition(), boat.move_speed);
        user_gui.sign = judge.Check((start_land.GetRoleNum())[0], (start_land.GetRoleNum())[1], (end_land.GetRoleNum())[0], (end_land.GetRoleNum())[1]);
        if (user_gui.sign == 1)
        {
            for (int i = 0; i < RoleAmount; i++)
            {
                roles[i].PlayGameOver();
                roles[i + RoleAmount].PlayGameOver();
            }
        }
    }

    public void MoveRole(RoleModel role)//移动角色
    {
        if (user_gui.sign != 0) return;
        if (role.IsOnBoat())
        {
            BankModel land;
            if (boat.GetBoatSign() == -1)
                land = end_land;
            else
                land = start_land;
            boat.DeleteRoleByName(role.GetName());
            Debug.Log("DeleteRoleByName");
            Debug.Log(role.GetName());

            //role.Move(land.GetEmptyPosition());
            //动作分离版本改变
            Debug.Log(land.GetBankSign());
            Vector3 end_pos = land.GetEmptyPosition();
            Debug.Log(end_pos);
            Vector3 middle_pos = new Vector3(role.getGameObject().transform.position.x, end_pos.y, end_pos.z);
            Debug.Log(middle_pos);
            actionManager.moveRole(role.getGameObject(), middle_pos, end_pos, role.move_speed);


            role.GoLand(land);
            land.AddRole(role);
        }
        else
        {
            BankModel land = role.GetBankModel();
            ///船没有空位，也不是停靠的陆地，就不上船
            if (boat.GetEmptyNumber() == -1 || land.GetBankSign() != boat.GetBoatSign()) return;

            land.DeleteRoleByName(role.GetName());

            //role.Move(boat.GetEmptyPosition());
            //动作分离版本改变
            Vector3 end_pos = boat.GetEmptyPosition();
            Debug.Log("?????");
            Vector3 middle_pos = new Vector3(end_pos.x, role.getGameObject().transform.position.y, end_pos.z);
            actionManager.moveRole(role.getGameObject(), middle_pos, end_pos, role.move_speed);


            role.GoBoat(boat);
            boat.AddRole(role);
        }
        user_gui.sign = judge.Check((start_land.GetRoleNum())[0], (start_land.GetRoleNum())[1], (end_land.GetRoleNum())[0], (end_land.GetRoleNum())[1]);
        if (user_gui.sign == 1)
        {
            for (int i = 0; i < RoleAmount; i++)
            {
                roles[i].PlayGameOver();
                roles[i + RoleAmount].PlayGameOver();
            }
        }
    }

    public void Restart()
    {
        start_land.Reset();
        end_land.Reset();
        boat.Reset();
        for (int i = 0; i < roles.Length; i++)
        {
            roles[i].Reset();
        }
        if (user_gui.sign == 1)
        {
            for (int i = 0; i < RoleAmount; i++)
            {
                roles[i + RoleAmount].PlayIdle();
                roles[i].PlayIdle();
            }
        }
    }

    public string getTips()
    {
        if (user_gui.sign != 0) return "";
        getNextPassenger();//计算得到下一状态值

        //get helping contents
        string text = "";
        if (next.boat_action == BoatAction.D)
        {
            text = "运载一个魔鬼到对岸";
        }
        else if (next.boat_action == BoatAction.P)
        {
            text = "运载一个牧师到对岸";
        }
        else if (next.boat_action == BoatAction.DD)
        {
            text = "运载两个魔鬼到对岸";
        }
        else if (next.boat_action == BoatAction.PP)
        {
            text = "运载两个牧师到对岸";
        }
        else if (next.boat_action == BoatAction.PD)
        {
            text = "运载一个牧师、一个魔鬼到对岸";
        }
        return text;
    }

    //获取目前的牧师、魔鬼分布状态，返回值=船将要承载的船员的情况
    private void getNextPassenger()
    {
        int start_priest = start_land.GetRoleNum()[0];
        int start_devil = start_land.GetRoleNum()[1];

        //set current state to next
        next.boat_sign = boat.GetBoatSign();

        //get next state
        if (next.boat_sign == 1 && start_priest == 3 && start_devil == 3)
        {
            next.boat_action = BoatAction.DD;
        }
        else if (next.boat_sign == -1 && start_priest == 3 && start_devil == 1)
        {
            next.boat_action = BoatAction.D;
        }
        else if (next.boat_sign == -1 && start_priest == 3 && start_devil == 2)
        {
            next.boat_action = BoatAction.D;
        }
        else if (next.boat_sign == -1 && start_priest == 2 && start_devil == 2)
        {
            next.boat_action = BoatAction.P;
        }
        else if (next.boat_sign == 1 && start_priest == 3 && start_devil == 2)
        {
            next.boat_action = BoatAction.DD;
        }
        else if (next.boat_sign == -1 && start_priest == 3 && start_devil == 0)
        {
            next.boat_action = BoatAction.D;
        }
        else if (next.boat_sign == 1 && start_priest == 3 && start_devil == 1)
        {
            next.boat_action = BoatAction.PP;
        }
        else if (next.boat_sign == -1 && start_priest == 1 && start_devil == 1)
        {
            next.boat_action = BoatAction.PD;
        }
        else if (next.boat_sign == 1 && start_priest == 2 && start_devil == 2)
        {
            next.boat_action = BoatAction.PP;
        }
        else if (next.boat_sign == -1 && start_priest == 0 && start_devil == 2)
        {
            next.boat_action = BoatAction.D;
        }
        else if (next.boat_sign == 1 && start_priest == 0 && start_devil == 3)
        {
            next.boat_action = BoatAction.DD;
        }
        else if (next.boat_sign == -1 && start_priest == 0 && start_devil == 1)
        {
            next.boat_action = BoatAction.D;
        }
        else if (next.boat_sign == 1 && start_priest == 0 && start_devil == 2)
        {
            next.boat_action = BoatAction.DD;
        }
        else if (next.boat_sign == 1 && start_priest == 1 && start_devil == 1)
        {
            next.boat_action = BoatAction.PD;
        }
    }
}
