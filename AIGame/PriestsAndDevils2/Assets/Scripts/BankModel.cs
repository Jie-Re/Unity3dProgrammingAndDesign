using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankModel
{
    private const int RoleAmount = 3;

    GameObject bank;//河岸对象
    int bank_sign;//左河岸标志为-1，右河岸标志为1
    RoleModel[] roles = new RoleModel[RoleAmount * 2];//河岸上有的角色
    Vector3[] positions;//保存每个角色在河岸上的位置

    //坐标设置相关参数
    private int priest_xdistance = 9;
    private int devil_xdistance = 7;
    private Vector3 p_bank_position1 = new Vector3(28, 6, 70 );
    private Vector3 d_bank_position1 = new Vector3(28, 6, 45);
    private Vector3 left_bank_position = new Vector3(-45, 0, 70);
    private Vector3 right_bank_position = new Vector3(45, 0, 70);


    public BankModel(string bank_mark)
    {
        positions = new Vector3[2 * RoleAmount];
        for (int i = 0; i < RoleAmount; i++)
        {
            positions[i] = new Vector3(p_bank_position1.x + i * priest_xdistance, p_bank_position1.y, p_bank_position1.z);
            positions[i + RoleAmount] = new Vector3(d_bank_position1.x + i * devil_xdistance, d_bank_position1.y, d_bank_position1.z);
            Debug.Log(positions[i]);
            Debug.Log(positions[i + RoleAmount]);
        }

        if (bank_mark == "left")
        {
            Debug.Log("Load left bank");
            bank = Object.Instantiate(Resources.Load("Bank", typeof(GameObject)), left_bank_position, Quaternion.identity) as GameObject;
            bank_sign = -1;
        }
        else
        {
            Debug.Log("Load right bank");
            bank = Object.Instantiate(Resources.Load("Bank", typeof(GameObject)), right_bank_position, Quaternion.identity) as GameObject;
            bank_sign = 1;
        }
    }

    public int GetBankSign() { return bank_sign; }

    public int GetEmptyNumber()//得到陆地上哪一个位置是空的
    {
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == null)
                return i;
        }
        return -1;
    }

    public Vector3 GetEmptyPosition()//得到陆地上的空位置
    {
        Vector3 pos = positions[GetEmptyNumber()];
        pos.x = bank_sign * pos.x;//因为两个陆地关于x坐标对称
        return pos;
    }

    public void AddRole(RoleModel role)//登上河岸
    {
        roles[GetEmptyNumber()] = role;
    }

    public RoleModel DeleteRoleByName(string role_name)//离开河岸
    {
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] != null && roles[i].GetName() == role_name)
            {
                RoleModel role = roles[i];
                roles[i] = null;
                return role;
            }
        }
        return null;
    }

    public int[] GetRoleNum()
    {
        int[] count = { 0, 0 };//count[0]是牧师数，count[1]是魔鬼数
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] != null)
            {
                if (roles[i].GetSign() == 0)
                    count[0]++;
                else
                    count[1]++;
            }
        }
        return count;
    }

    public void Reset()
    {
        roles = new RoleModel[6];
    }
}
