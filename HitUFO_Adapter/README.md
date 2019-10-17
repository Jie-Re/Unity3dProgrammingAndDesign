# 改进飞碟（Hit UFO）游戏
## 游戏内容要求
1. 按adapter模式设计图修改飞碟游戏
2. 使它同时支持物理运动与运动学（变换）运动
## 适配器模式
> 适配器模式（Adapter Pattern）是作为两个不兼容的接口之间的桥梁。这种类型的设计模式属于结构型模式，它结合了两个独立接口的功能。
这种模式涉及到一个单一的类，该类负责加入独立的或不兼容的接口功能。

> 关键代码：适配器继承或依赖已有的对象，实现想要的目标接口。

## 程序设计
基于作业05修改代码，首先可以明确的是MVC架构中的`游戏对象模型`、`控制器Controller.cs`、`界面UserGUI.cs`的代码是不需要做修改的。

现在添加飞碟物理运动框架：
- `PhysisFlyAction.cs`
- `PhysisFlyActionManager`
这两个程序的整体框架与作业04中的`FlyAction.cs`和`FlyActionManager.cs`的整体框架基本相差无异，不同在于内部运动的实现逻辑：

```csharp
//FlyAction
public override void Update()
{
    //计算物体的向下的速度,v=at
    time += Time.fixedDeltaTime;
    gravity_vector.y = gravity * time;

    //位移模拟
    transform.position += (start_vector + gravity_vector) * Time.fixedDeltaTime;
    current_angle.z = Mathf.Atan((start_vector.y + gravity_vector.y) / start_vector.x) * Mathf.Rad2Deg;
    transform.eulerAngles = current_angle;

    //如果物体y坐标小于-10，动作就做完了
    if (this.transform.position.y < -10)
    {
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}

public override void Start()
{
    //飞行动作建立时候不做任何事情
}
public override void FixedUpdate()
{
    //不做任何事情，但必须重载，否则SSAction中抛出异常
}
```
```csharp
//PhysisFlyAction
public override void FixedUpdate()
{
    //判断是否超出范围
    if (this.transform.position.y < -10)
    {
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}
public override void Update()
{
    //不做任何事情，但必须重载，否则SSAction中抛出异常
}
public override void Start()
{
    //使用重力以及给一个初速度
    gameobject.GetComponent<Rigidbody>().velocity = power / 35 * start_vector;
    gameobject.GetComponent<Rigidbody>().useGravity = true;
}
```
这里值得一提的是，当`MonoBehaviour`启动时，`Update()`和`FixedUpdate()`都会在每一帧被调用，但是由于物理运动中需要处理`Rigidbody`，所以需要使用`FixedUpdate()`

> **Update和FixedUpdate的区别：**
    update跟当前平台的帧数有关，而FixedUpdate是真实时间，所以处理物理逻辑的时候要把代码放在FixedUpdate而不是Update.
 	Update是在每次渲染新的一帧的时候才会调用，也就是说，这个函数的更新频率和设备的性能有关以及被渲染的物体（可以认为是三角形的数量）。在性能好的机器上可能fps 30，差的可能小些。这会导致同一个游戏在不同的机器上效果不一致，有的快有的慢。因为Update的执行间隔不一样了。
 	而FixedUpdate，是在固定的时间间隔执行，不受游戏帧率的影响。有点想Tick。所以处理Rigidbody的时候最好用FixedUpdate。
	PS：FixedUpdate的时间间隔可以在项目设置中更改，Edit->ProjectSetting->time  找到Fixedtimestep。就可以修改了。


接下来，为了使程序兼容两种运动模式，需要添加适配器代码`ActionManagerAdapter.cs`：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionManager
{
    void playDisk(GameObject disk, float angle, float power, bool isPhy);
}

public class ActionManagerAdapter : MonoBehaviour,IActionManager
{
    public FlyActionManager action_manager;
    public PhysisFlyActionManager phy_action_manager;

    public void playDisk(GameObject disk, float angle, float power, bool isPhy)
    {
        if (isPhy)
        {
            phy_action_manager.UFOFly(disk, angle, power);
        }
        else
        {
            action_manager.UFOFly(disk, angle, power);
        }
    }

    //Use this for initialization
    void Start()
    {
        action_manager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        phy_action_manager = gameObject.AddComponent<PhysisFlyActionManager>() as PhysisFlyActionManager;
    }
}

```

此外，由于增加了`FixedUpdate()`方法的调用，故而还需要对父类`SSAction`和`SSActionManager`做修改——`SSAction`中添加虚函数`FixedUpdate()`，`SSActionManager`中添加可供子类调用的函数`FixedUpdate()`（方法内部实现代码与`Update()`完全相同）

至此，作业04的代码便成功修改成了适配器模式。
## 详细代码
[代码传送门](https://github.com/Jie-Re/Unity3dProgrammingAndDesign/tree/master/HitUFO_Adapter)
## 游戏演示视频
同作业04的演示视频：[前往B站观看游戏demo演示视频](https://www.bilibili.com/video/av71451466/)
# 参考资料
[适配器模式](https://www.runoob.com/design-pattern/adapter-pattern.html)
[前辈的博客](https://blog.csdn.net/kasama1953/article/details/52606419)
[前辈的博客](https://blog.csdn.net/C486C/article/details/80052862)
