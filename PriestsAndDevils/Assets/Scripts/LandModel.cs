using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandModel
{
    GameObject land;                        //陆地对象
    Vector3[] positions;                    //保存每个角色放在陆地上的位置
    int land_sign;                          //到达陆地标志为-1，开始陆地标志为1
    RoleModel[] roles = new RoleModel[6];   //陆地上有的角色

    //position
    Vector3 startLandPosition = new Vector3(40, 1, 0);
    Vector3 endLandPosition = new Vector3(-40, 1, 0);
    Vector3 startPriestPosition1 = new Vector3(16, 3, 1);
    Vector3 startPriestPosition2 = new Vector3(18, 3, 1);
    Vector3 startPriestPosition3 = new Vector3(20, 3, 1);
    Vector3 startDevilPosition1 = new Vector3(22, 3, 1);
    Vector3 startDevilPosition2 = new Vector3(24, 3, 1);
    Vector3 startDevilPosition3 = new Vector3(26, 3, 1);

    public LandModel(string land_mark)
    {
        positions = new Vector3[] { startPriestPosition1, startPriestPosition2, startPriestPosition3, startDevilPosition1, startDevilPosition2, startDevilPosition3 };
        if (land_mark == "start")
        {
            land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), startLandPosition, Quaternion.identity) as GameObject;
            land_sign = 1;
        }
        else if (land_mark == "end")
        {
            land = Object.Instantiate(Resources.Load("Land", typeof(GameObject)), endLandPosition, Quaternion.identity) as GameObject;
            land_sign = -1;
        }
    }

    //向当前河岸上添加牧师或魔鬼
    public void AddRole(RoleModel role)
    {
        roles[GetEmptyNumber()] = role;
    }

    //移除当前河岸上的牧师或魔鬼
    public RoleModel DeleteRoleByName(string role_name)
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
        return null;//河岸上为空，则返回null
    }

    //得到陆地上空位置
    public Vector3 GetEmptyPosition()
    {
        Vector3 pos = positions[GetEmptyNumber()];
        pos.x = land_sign * pos.x;//两个陆地关于x坐标对称
        return pos;
    }

    public int GetEmptyNumber()
    {
        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == null)
                return i;
        }
        return -1;
    }

    //获取河岸标识(start/end)
    public int GetLandSign()
    {
        return land_sign;
    }

    //重置
    public void Reset()
    {
        roles = new RoleModel[6];
    }

    //获取当前河岸上牧师数量和魔鬼数量
    public int[] GetRoleNum()
    {
        int[] count = { 0, 0 }; //count[0]是牧师数，count[1]是魔鬼数
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
}
