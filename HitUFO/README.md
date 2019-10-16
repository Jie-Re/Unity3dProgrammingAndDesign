# 鼠标打飞碟（Hit UFO）游戏
## 要求
### 游戏内容要求
- 游戏有 n 个 round，每个 round 都包括10 次 trial；
- 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
- 每个 trial 的飞碟有随机性，总体难度随 round 上升；
- 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。
### 程序设计要求
- 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
- 近可能使用前面 MVC 结构实现人机交互与游戏模型分离
## 程序设计
### MVC架构
首先搭建MVC架构，其中的MVC架构模型可以直接借用（copy）上一次牧师与魔鬼游戏的架构设计代码。
- 模型
	- 架构模型
		- `ISceneController`(interface)
		- `SSAction`(继承`ScriptableObject`)
		- `SSActionEventType`(enum类型）
		- `ISSActionCallback`(interface）
		- `SequenceAction`(继承`SSAction`和`ISSActionCallback`）
		- `SSDirector`(继承`System.Object`）
		- `SSActionManager`(继承`MonoBehaviou`和`ISSActionCallback`)
		- `Singleton<T>`
	- 游戏对象模型
		- `UFOData`(继承`MonoBehaviour`)
		- `UFOFactory`(继承`MonoBehaviour`)
		- `Judge`(继承`MonoBehaviour`)
		- `JoyStick`(继承`MonoBehaviour`)
		- `SpaceCraft`
	- 动作事件模型
		- `FlyAction`(继承`SSAction`)
		- `FlyActionManger`(继承`SSActionManager`)
		- `IUserAction`用户动作列表(interface)

	|动作|参数|结果|
	|--|--|--|
	|启动游戏|无|游戏初始界面|
	|重新开始|无|游戏初始界面|
	|用户点击游戏界面|点击位置pos|击中飞碟与否| 

- 控制器
	- `Controller`(继承`MonoBehaviour`，`ISceneController`，`IUserAction`)
- 界面
	- `UserGUI`
### 场景单实例的工厂
应本次程序设计的要求，还需要使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的。

> 工厂模式
> 意图：定义一个创建对象的接口，让其子类自己决定实例化哪一个工厂类，工厂模式使其创建过程延迟到子类进行。
> 关键代码：创建过程在其子类执行。

在本例中运用模板，可以为每个MonoBehaviour子类创建一个对象的实例。Singleton<T>代码如图所示：

```csharp
//Singleton.cs
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	protected static T instance;

	public static T Instance {  
		get {  
			if (instance == null) { 
				instance = (T)FindObjectOfType (typeof(T));  
				if (instance == null) {  
					Debug.LogError ("An instance of " + typeof(T) +
					" is needed in the scene, but there is none.");  
				}  
			}  
			return instance;  
		}  
	}
}
```
要使用场景单例，只需要将MonoBehavior子类对象挂载到任何一个游戏对象上即可，然后在任意位置可以通过`Singleton<YourMnoType>.Instance`获得该对象。
## 游戏规则说明
1. 每一轮游戏，生命值为6；每未击中一个飞碟则扣除1点生命值；当生命值降为0时，游戏结束
2. 鼠标点击飞碟可击中飞碟，每击中一个飞碟可获得5分
3. 达到10分可进入第二回合；达到25分可进入第三回合
4. 第一个回合飞碟颜色为绿色；第二个回合飞碟颜色为蓝色；第三个回合飞碟颜色为红色
5. 用`wasd`键控制飞机`上左下右`移动，保证飞机不要与飞碟发生碰撞，否则生命值将直接降为0，游戏结束
6. 随着回合数的增加，飞碟出现和移动速度将加快
## 详细代码
[代码传送门](https://github.com/Jie-Re/Unity3dProgrammingAndDesign/tree/master/HitUFO)
## 游戏演示视频
[前往B站观看游戏demo演示视频](https://www.bilibili.com/video/av71451466/)
