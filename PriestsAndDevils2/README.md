# 编程实践-牧师与魔鬼小游戏进阶版（动作分离版）
## 程序要求
- 使用 C# 集合类型 有效组织对象
- 整个游戏仅主摄像机和一个Empty对象，其他对象必须代码动态生成！！！ 。 整个游戏不许出现Find游戏对象，SendMessage这类突破程序结构的通讯耦合语句。违背本条准则，不给分
- 请使用课件架构图编程，不接受非`MVC`结构程序
- 注意细节，例如：船未靠岸，牧师与魔鬼上下船运动中，均不能接受用户事件！
- 2019新要求：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束

- 代码改进：
	- 设计裁判类（提取出Check方法）
- 动作分离：
	- 把每个需要移动的游戏对象的移动方法提取出来，建立一个动作管理器来管理不同的移动方法。
	- 对于上一个版本，每一个可移动的游戏对象的组件都有一个Move脚本，当游戏对象需要移动时候，游戏对象自己调用Move脚本中的方法让自己移动。而动作分离版，则剥夺了游戏对象自己调用动作的能力，建立一个动作管理器，通过场景控制器(在我的代码设计中是Controller)把需要移动的游戏对象传递给动作管理器，让动作管理器去移动游戏对象。
	- 当动作很多或是需要做同样动作的游戏对象很多的时候，使用动作管理器可以让动作很容易管理，也提高了代码复用性。

## 游戏场景制作
资源下载：
- Devil animated character【恶魔】
- Toony Tiny RTS Demo【牧师】
- Lowpoly Paper Boat【船】
- Rounded Blocks【河岸】
- Wispy Skybox【天空（背景）】

场景布置：
- Skybox：参见前面所述
- 静态游戏对象：主摄像机和空对象MAIN（挂载脚本`Click.cs`,`UseGUI.cs`,`Controller.cs`和`SSActionManger.cs`）
- 预制：`Boat`，`Bank`，`Priest`（添加组件`Animator`），`Devil`（添加组件`Animator`），如下图所示：
![AssetPrefabs](https://img-blog.csdnimg.cn/20190928183427527.PNG#pic_center)
	事实上，为了让代码动态创建对象，我们需要将这些预制放在`Resources`目录下。
	此外，为了使`Boat`，`Priest`，`Devil`正常响应点击事件，需要“外包”一个游戏对象，例如对于`Priest`，用一个`Capsule`作为其父亲，将资源商店中下载的预制作为`Capsule`的子对象；并且为了达到较好的显示效果，设置一定的位置偏移和大小比例，这里我采用的是：
	对于Priest：
	
|GameObject|Position|Scale|
|--|--|--|
|Capsule|(0,0,0)|(3,3,3)|
|从资源商店中下载的预制对象(TT_RTS_Demo_Character)|(0,-1.2,0)|(2,2,2)|
对于Devil：
	
|GameObject|Position|Scale|
|--|--|--|
|Capsule|(0,0,0)|(1.5,3,1.5)|
|从资源商店中下载的预制对象(devil@idle)|(0.2,-1.4,0)|(5,2.5,5)|
对于Boat：
	
|GameObject|Position|Scale|
|--|--|--|
|Cylinder|(0,0,0)|(15,1,8)|
|从资源商店中下载的预制对象(black_perl)|(0,-1,0)|(0.001,0.01,0.002)|
- 游戏预制对象坐标和大小设置

|GameObject|Position|Rotation|Scale|
|--|--|--|--|
|Main Camera|(0,1,0)|(0,0,0)|(1,1,1)|
|MAIN|(0,0,0)|(0,0,0)|(1,1,1)|
|Boat|(-10,0,40)|(0,10,0)|(0.01,0.01,0.01)|
|BoatSeat1|(-6,1,40)|/|/|
|BoatSeat2|(-14,1,40)|/|/|
|Bank|(-45,0,70)|(0,0,0)|(40,5,60)|
|Priest|/|(0,90,0)|(3,3,3)|
|Devil|/|(0,90,0)|(1.5,3,1.5)|
|PBankPosition1|(-28,3,70)|/|/|
|PBankPosition2|(-37,3,70)|/|/|
|PBankPosition3|(-46,3,70)|/|/|
|DBankPosition1|(-30,3,40)|/|/|
|DBankPosition2|(-26,3,40)|/|/|
|DBankPosition3|(-22,3,40)|/|/|
动态生成代码时，另一侧的坐标只要做x坐标的对称运算即可

游戏场景预制结果：
![游戏场景预制结果图](https://img-blog.csdnimg.cn/20190928181039776.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70#pic_center)
## 代码设计
MVC架构——界面人机交互程序设计的一种架构模式
将程序分为三部分：
- 模型
	- `RoleModel.cs`
	- `BoatModel.cs`
	- `BankModel.cs`
	- `PlayAnimation.cs`
- 控制器
	- `SSDirector.cs`
	- `ISceneController.cs`
	- `IUserAction.cs`
	- `SSAction.cs`
	- `SSMoveToAction.cs`
	- `SequenceAction.cs`
	- `ISSActionCallback.cs`
	- `SSActionManager.cs`
	- `Controller.cs`
	- `Click.cs`
	- `MySceneActionManger.cs`
	- `Judge.cs`
- 界面
	- `UserGUI.cs`

与上一版本的比较：
- 去除了`Move.cs`脚本
- 用户动作列表中去除了`Check()`，而把这一动作交给`Judge`实现
- 增加了`Judge.cs`脚本和相应的动作管理的脚本

### 模型
模型主要是描述数据对象及关系
- `RoleModel.cs`：牧师/魔鬼角色模型
- `BoatModel.cs`：船模型（记录了船的位置、船上的空位位置）
- `BankModel.cs`：河岸模型（记录了河岸位置、河岸上的空位位置）
-  `ISceneController`：场记（接口）
	其职责大致如下：
	- 管理本次场景所有的游戏对象
	- 协调游戏对象（预制件级别）之间的通讯
	- 响应外部输入事件
	- 管理本场次的规则（裁判）
	- 各种杂务
	这里，我们用一个`LoadResources()`方法实现这些职责：
	```csharp
	public interface ISceneController //场记接口
	{
	   void LoadResources();
	}
	```
-  `IUserAction`：用户动作“列表”（接口）
	- 移动船
	- 移动角色
	- 重新开始
	```csharp
	public interface IUserAction //用户动作“列表”接口
	{
	    void MoveBoat();//移动船
	    void MoveRole(RoleModel role);//移动角色
	    void Restart();//重新开始
	}
	```
- `SSDirector`：导演（单实例模式）
	创建`SSDirector`类，其职责大致如下：
		- 获取当前游戏的场景
		- 控制场景运行、切换、入栈与出栈
		- 暂停、恢复、退出
		- 管理游戏全局状态
		- 设定游戏的配置
		- 设定游戏全局视图
- `SSAction`：动作基类
	`SSAction`是所有动作的基类。它集成了`ScriptableObjects`，代表`SSAction`不需要绑定`GameObject`对象，且受Unity引擎场景管理
- `SSMoveToAction.cs`：移动动作实现（以speed的速度向target目的地移动）
- `SequenceAction.cs`：组合动作实现
	`SequenceAction`继承了`ISSActionCallback`，因为组合动作是每一个动作的顺序完成，它管理这一连串动作中的每一个小的动作，所以当小的动作完成的时候，也要发消息告诉它，然后它得到消息后去处理下一个动作。`SequenceAction`也继承了`SSAction`，因为成个组合动作也需要游戏对象，也需要标识是否摧毁，也会有一个组合动作的管理者的接口，组成动作也是动作的子类，只不过是让具体的动作组合起来做罢了。
- `ISSActionCallback.cs`：动作事件接口
	作为动作和动作管理者的接口（组合动作也可以是动作管理者），动作管理者继承这个接口，并且实现接口的方法。当动作完成时，动作会调用这个接口，发送消息通知动作管理者对象动作已完成，然后管理者会对下一个动作进行处理。
- `SSActionManager.cs`：动作管理基类
	管理`SequenceAction`和`SSAction`，可以给它们传递游戏对象，让游戏对象做动作或是一连串的动作，控制动作的切换。`SActionManager`继承了`ISSActionCallback`接口，通过这个接口，当动作做完或是连续的动作做完时候会告诉`SSActionManager`，然后`SSActionManager`去决定如何执行下一个动作。
- `Click.cs`：点击事件脚本（挂载在MAIN对象上即可）
- `MySceneActionManger.cs`：移动动作管理实现
	船的移动是一个`SSMoveToAction`动作就可以实现，而角色的移动需要两个`SSMoveToAction`动作组合（先垂直后水平移动或先水平后垂直移动）。然后设置当前场景控制器的动作管理者为`MySceneActionManger`，这样场景控制器就可以调用动作管理器的方法实现不同游戏对象的移动了。
	- `Judge.cs`：裁判类，当游戏达到结束条件时，通知场景控制器游戏结束
	设置当前场景控制器的裁判为`Judge`（传入当前场景控制器的`boat`构建`Judge`），将`IUserAction`中的`Check()`方法迁移至本类中，其中的`start_land`上的牧师与魔鬼个数，`end_land`上的牧师与魔鬼个数由场景控制器调用本类的`Check()`方法时传入即可。
### 控制器
- `Controller.cs`：接受用户事件，控制上述各模型的变化（挂载在`MAIN`对象下）
### 用户界面
- `UserGUI`：显示模型，将人机交互事件交给控制器处理
	- 处理鼠标点击事件（交给控制器`Controller`）
	- 渲染GUI（`OnGUI()`），接收事件 
## 游戏演示视频
[前往b站查看游戏演示视频](https://www.bilibili.com/video/av69887985/)
