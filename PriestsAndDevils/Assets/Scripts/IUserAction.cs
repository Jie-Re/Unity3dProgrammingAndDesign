using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void MoveBoat();                                    //移动船
    void Restart();                                    //重新开始
    void MoveRole(RoleModel role);                     //移动角色
    int Check();                                       //检测游戏结束

}
