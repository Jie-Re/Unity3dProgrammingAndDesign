[博客传送门](https://blog.csdn.net/xxiangyusb/article/details/103730268)
# 环境说明
Unity2019.2.3f1  
Win10 x64
# Unity3D 导航与寻路
寻路是游戏中经常使用到的一项技术，自动寻路就是AI中的一个十分重要的分支，其算法异常复杂。Unity中提供的一套非常成熟的组件来为我们解决这一难题：Unity 导航系统允许创建给游戏角色导航的游戏世界。它将游戏场景中复杂的结构组织关系简化为带有一定信息的网格 ,在这些网格的基础上通过一系列相应的计算来实现自动寻路。

- **NavMesh** (Navigation Mesh) ：一种数据结构，它描述了游戏对象可行走的表面。通过三角网格，计算其中任意两点之间的最短路径，用于游戏对象的导航。它是根据场景几何结构自动创建或烘焙构建的
	更多有关NavMesh的介绍更参考[Unity 基础寻路-NavMesh](https://blog.csdn.net/Czhenya/article/details/77603388)
- **NavMesh Agent**：寻路导航组件，它创建具有寻路能力的角色
	有关NavMeshAgent组件面板属性的说明可参考[NavMeshAgent 寻路导航组件](https://blog.csdn.net/u013628121/article/details/53105022)，[NavMeshAgent](https://blog.csdn.net/czlilove/article/details/21455575)以及[Unity官方文档说明](https://docs.unity3d.com/Manual/class-NavMeshAgent.html)
- **Off-Mesh Link**：这一组件允许将不连接的块之间建立“传送门”。例如，跳过沟渠或围栏，或在穿过它之前打开门，都可以被描述为 Off-Mesh Link
- **Nav Mesh Obstacle**：这一组件允许您描述 agent 在移动时应避免的移动障碍。例如，由物理系统控制的桶或箱子就是很典型的障碍。在障碍物移动的过程中，Agent 尽力避开它，但一旦障碍物变得静止，它将在导航网格上开一个洞，以便 Agent 可以改变他们的路径以绕过它，或者静止的障碍物阻塞路径，使得 Agent 找到其他路线
	有关这一组件的更多详细说明可参考[NavMesh Obstacle组件](https://www.jianshu.com/p/eae6c84793ac)及[Unity官方文档说明](https://docs.unity3d.com/Manual/class-NavMeshObstacle.html)
## Agent 和 Navmesh 练习
- 创建一个新Unity3D项目`NavPractive`
- 创建一个`Plane`对象：`GameObject`->`3D Object`->`Plane`
- 分别创建蓝色`Material`和粉色`Material`，命名为`blue`和`pink`
- 创建一个`Cube`对象，赋上`blue`，制作成`wall`预制
	![WallPrefab.PNG](https://img-blog.csdnimg.cn/20191227132059675.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 创建一个`Sphere`对象，赋上`pink`，制作成`target`预制
	![TargetPrefab.PNG](https://img-blog.csdnimg.cn/20191227132014364.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 从`Assets Store`中下载并导入标准资源`Standard Assets`
- 添加AI角色：将资源包`Standard Assets/Characters/ThirdPersonCharacter/Prefabs/`下的`AIThirdPersonController`拖入场景中
- 布局：

|对象|position|rotation|scale|
|--|--|--|--|
|Main Camera|(0,5,-10)|(30,0,0)|(1,1,1)|
|AIThirdPersonController|(0,0,-4)|(0,0,0)|(1,1,1)|
|target|(0,0,-4)|(0,0,0)|(0.5,0.1,0.5)|
|wall0|(0,0,0)|(0,0,0)|(3,1,1)|
|wall1|(4,0,0)|(0,0,0)|(2,1,1)|
|wall2|(-4,0,0)|(0,0,0)|(2,1,1)|
![scene.PNG](https://img-blog.csdnimg.cn/20191227192223314.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 完成布局后，将`target`拖入`AIThirdPersonController`对象的`AICharacterControl`脚本组件的`Target`插槽（**这里注意是将`target`对象赋值过去，而非预制**）
![AICharacterControllerTarget.png](https://img-blog.csdnimg.cn/20191227192524782.png)
- 打开`Navigation`编辑面板窗口：`Window`->`AI`->`Navigation`
- 配置`Plane`和`target`对象的`Navigation`->`Object`：
	- 勾选`Navigation Static`
	- 设置`Navigation Area`为`Walkable`
- 配置`wall0`，`wall1`和`wall2`的`Navigation`->`Object`：
	- 勾选`Navigation Static`
	- 设置`Navigation Area`为`Not Walkable`
- 点击`Navigation`/`Bake`面板中的`Bake`，可得到如图所示的结果：
![Bake.png](https://img-blog.csdnimg.cn/20191227202344612.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
图中，水蓝色凸多边形构成的网格就是寻路算法的数据结构，agent 将用它来导航。
- 创建脚本`PlaceTarget`，代码内容如下：

	```csharp
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public class PlaceTarget : MonoBehaviour {
		
		// Update is called once per frame
		void Update () {
			if (Input.GetMouseButtonDown (0)) {    
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);  
				RaycastHit hit;  
				if (Physics.Raycast (ray, out hit)) {  
					Debug.Log (hit.collider.name);  
					if (hit.collider.name == "Plane") {
						this.transform.position = hit.point;
					}
				}
			}
		}
	}
	```
	将这一脚本挂载到`target`对象下
- 运行：
![Navigation1.gif](https://img-blog.csdnimg.cn/20191227203552228.gif)
## Obstacle和Off-Mesh-Link练习
- 将当前场景另存为`scene2`
- 创建一个`Cylinder`对象，`Scale=(0.5,0.01,0.5)`，黄色，制作成`mark`预制
- 将`mark`预制拖入场景，创建两个对象，分别命名为`mark0`(`position=(0,0,-1)`)和`mark1`(`position=(0,0,1)`)，两者均勾选`Navigation Static`，并设置`Navigation Area`为`Walkable`
- `Creat Empty`创建一个empty对象，命名为`meshlink`，并添加组件`Off Mesh Link`
- 将`mark0`和`mark1`分别拖入`meshlink`的`Off Mesh Link`组件的`start`和`end`插槽，并设置`Cost Override`为`-1`
![OffMeshLinkConfig.PNG](https://img-blog.csdnimg.cn/2019122723334499.PNG)
- 创建一个`Cube`对象，命名为`Obstacle`，并为其添加组件`Nav Mesh Obstacle`，在该组件中勾选`Carve`，它将自动切割导航网格
![ObstacleConfig.PNG](https://img-blog.csdnimg.cn/20191227233613214.PNG)
- 重新布置场景如下：
![Scene2.PNG](https://img-blog.csdnimg.cn/20191227234228209.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
- 运行：
![Navigation2.gif](https://img-blog.csdnimg.cn/20191227234941785.gif)
本部分完整项目代码见[NavPractice](https://github.com/Jie-Re/Unity3dProgrammingAndDesign/tree/master/AIGame/NavPractice)
# P&D 过河游戏智能帮助实现
程序具体要求：
- 实现状态图的自动生成
- 讲解图数据在程序中的表示方法
- 利用算法实现下一步的计算
## 状态图
参考[Unity学习之P&D 过河游戏智能帮助实现](https://blog.csdn.net/kiloveyousmile/article/details/71727667)，以游戏过程中的开始陆地的情况为例，其状态图如下：
![Status.jpg](https://img-blog.csdnimg.cn/20191227235835405.jpg?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3h4aWFuZ3l1c2I=,size_16,color_FFFFFF,t_70)
其中，P代表牧师，D代表魔鬼，B代表船，数字代表角色个数，例如：3P3DB表示开始陆地有3个牧师和3个魔鬼，同时船也停在开始陆地。箭头指向即为状态的转化方向。

从而我们可以很容易想到这样设计算法：点击`Tip`之后，分析当前场景的状态，读取可行路径上的下一个状态（可通过随机选择函数进行选择/亦可设计算法选择具有最短路径者/或直接自行指定，我这里采用的是直接指定），将这一可行的状态返回给用户。

从而我们可以设计如下数据结构：
```csharp
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
```
## AI算法
- 首先获取当前状态

	```csharp
	next.boat_sign = boat.GetBoatSign();
	```

- 根据状态图，计算下一状态：

	```csharp
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
	```

## 其他设计修改
- `IUserAction`：根据分析我们可以知道，用户动作列表应多一个“获取帮助”的操作

	```csharp
	//IUserAction.cs
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public interface IUserAction //用户动作“列表”接口
	{
	    void MoveBoat();//移动船
	    void MoveRole(RoleModel role);//移动角色
	    void Restart();//重新开始
	    string getTips();//获取帮助
	}
	
	```
	此外，在`Controller.cs`中，`getTips()`的实现如下：
	```csharp
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
	```

- `UserGUI`：添加`Tips`的按钮及其响应处理代码

	```csharp
	if (GUI.Button(new Rect(80,10,60,30), "Tips", button_style))
	{
	    helping_text = action.getTips();
	    if (isTip)
	        isTip = false;
	    else
	        isTip = true;
	}
	if (isTip)
	{
	    GUI.Label(new Rect(10, 50, 200, 50), helping_text);
	}
	```
本部分完整项目代码见[PriestsAndDevils2](https://github.com/Jie-Re/Unity3dProgrammingAndDesign/tree/master/AIGame/PriestsAndDevils2)
## 演示视频
[前往b站观看演示视频](https://www.bilibili.com/video/av80928095/)

# 其他参考资料
[潘老师的课程网站 第十章 游戏智能](https://pmlpml.github.io/unity3d-learning/10-intelligent.html)
