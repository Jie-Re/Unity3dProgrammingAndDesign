using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, ISceneController, IUserAction
{
    private const int RoleAmount = 3;

    public BankModel start_land;//开始陆地
    public BankModel end_land;//结束陆地
    public BoatModel boat;//船
    private RoleModel[] roles;//角色
    UserGUI user_gui;
    Judge judge;

    public MySceneActionManager actionManager;//动作管理

    // Start is called before the first frame update
    void Start()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        user_gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        LoadResources();
        judge = new Judge(boat);
        actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager;
    }

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
}
