using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction //用户动作“列表”接口
{
    void MoveBoat();//移动船
    void MoveRole(RoleModel role);//移动角色
    void Restart();//重新开始
}
