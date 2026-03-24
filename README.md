# 🧩 Game Overview | 游戏简介
This is a side-scrolling action-adventure demo developed with Unity. Players control a hero to navigate through levels, battle various monsters, and overcome environmental challenges.

这是一款基于 Unity 开发的横向卷轴动作冒险游戏 Demo。玩家将操控主角在关卡中与各类怪物战斗，突破重重关卡。

---

## 🕹 Gameplay | 操作说明
| Key | Action | Description / 描述 |
|:---:|:---:|:---|
| **A / D** | **Move** | Horizontal movement / 左右移动 |
| **Shift** | **Run** | Increase movement speed / 奔跑加速 |
| **Space** | **Jump** | Jump & Double jump / 跳跃与二段跳 |
| **L** | **Slide** | Dash forward with lower hitbox / 滑铲位移与低位躲避 |
| **F** | **Attack** | Perform a melee strike / 挥砍攻击 |
| **E** | **Interact** | Open chests or use portals / 与宝箱或传送点交互 |

---

## ⚙️ Game Scene | 游戏场景划分
| Module | Description | 功能说明 |
|:---:|:---|:---|
| **Persistent** | Controls long-term modules: Character, Camera, BGM, UI Manager, Scene Loader, and Data Manager. | 控制全局持久化模块：角色、相机、音频管理、场景加载及数据管理中心。 |
| **Menu** | Handles player choices at start: Start, Continue, and Quit. | 负责初始菜单交互：开始游戏、继续存档及退出游戏。 |
| **Map1** | Primary test map featuring: Boar AI, Spikes, Portals, Chests, and Water hazards. | 核心功能测试地图：涵盖野猪 AI、荆棘陷阱、传送机制、宝箱系统及水域判定。 |
| **Map2** | Auxiliary map for testing inter-scene transitions and save/load systems. | 辅助测试地图：专门用于验证跨场景传送逻辑及存档/读档功能。 |

---

## 💡 Core Gameplay Logic | 核心逻辑

### 1. Level & Environment | 环境与数据加载
* **Tilemap Construction**: High-performance terrain and background rendering using Unity Tilemap. / 利用 Unity Tilemap 绘制高性能地形与多层背景。
* **Dynamic Objects**:
    * **Waterfall**: Animated background elements. / 动态水流背景元素。
    * **Obstacles**: Spikes and vine traps with damage triggers. / 带有伤害触发器的荆棘与陷阱。

### 2. Character Controller & FSM | 角色控制与状态机
* **Locomotion**:
    * **Movement**: Smooth physics-based movement via `Rigidbody2D.velocity`. / 基于刚体速度的平滑平移。
    * **Jump**: Multi-stage jumping with `isGrounded` detection. / 带有接地检测的多段跳跃系统。
    * **Slide**: Hitbox resizing for low-profile dashing. / 滑铲时动态缩小碰撞体以通过低矮区域。
    * **Wall Slide**: Friction-based wall clinging and slow descent. / 贴墙摩擦逻辑，支持在墙面缓慢下滑。
* **Combat & Interaction**:
    * **Attack**: Animation-driven attack with frame-perfect Hurtbox activation. / 动画驱动攻击，并在特定帧激活伤害判定区。
    * **Interact**: Trigger-based UI prompts ("E") for chests and portals. / 基于触发器的交互系统，靠近物件时弹出“E”键提示。

### 3. Enemy AI & Judgement | 敌人 AI 与判定系统
* **Boar Logic**:
    * **Patrol**: Automatic movement between set waypoints or raycast detection. / 基于射线检测或预设点的自动巡逻。
    * **Flip**: 180° rotation upon hitting walls or ledge edges. / 撞墙或到达平台边缘时自动翻转转向。
    * **Chase/Run**: Accelerates towards the player upon detection. / 发现玩家后进入追击/冲刺模式。
* **Collision System**:
    * **Damage Detection**: Player HP reduction triggered by `BoxCollider2D` overlaps with hazards. / 角色碰撞体与危险源重叠时触发扣血逻辑。
    * **Death Zones**: Water or abyss triggers that lead to an immediate Game Over. / 深水或深渊触发器，接触即判定死亡。

### 4. UI & Scene Management | UI 与场景管理
* **Real-time Monitoring**:
    * **Health Bar**: Dynamic fill amount updates synchronized with `PlayerStats`. / 血条 UI 与玩家属性实时数据同步。
    * **World-Space UI**: Interaction prompts that track object positions. / 动态追踪物体位置的屏幕空间交互提示。
* **Flow Control**:
    * **Game Over**: Freezes game via `Time.timeScale = 0` and displays the retry menu. / 死亡时冻结时间并弹出结算菜单。
    * **Restart**: Hot-reloading levels via `SceneManager`. / 通过场景管理器实现关卡快速重载。

---

## 📂 Project Structure | 项目结构
```text
Assets/
├── AddressableAssetsData/    # Addressable system configurations / 可寻址资源配置
├── Animations/               # Controllers & Clips / 动画控制器与剪辑
│   ├── Enemy/                # Enemy behavior animations / 敌人动画
│   ├── Player/               # Character action clips / 玩家动作动画
│   └── UI/                   # Interface transitions / UI 交互动画
├── Art Asset/                # Raw sprites & textures / 原始美术素材
│   ├── generic_char_v0.2/    # Character sprite sheets / 角色序列帧
│   ├── KeyBoard_Anime/       # Input prompt assets / 按键提示素材
│   └── Legacy-Fantasy/       # Environment & props / 环境与场景素材
├── Audio/                    # Sound effects & BGM / 音频资源
├── DataSQ/                   # Game sequence & scene data / 游戏序列与场景配置
├── Fonts/                    # Typography (Smiley Sans) / 字体资源
├── Plugins/                  # Third-party plugins (DOTween) / 外部插件
├── Prefab/                   # Reusable game objects / 游戏物体预制体
├── Resources/                # Dynamic loading assets / 动态加载资源
├── Scenes/                   # Level & menu scenes / 游戏关卡与菜单
├── Scripts/                  # Core C# logic / 核心逻辑代码
│   ├── Enemy/                # AI & Combat logic / 敌人 AI 与战斗
│   ├── Player/               # Control & Stats / 角色控制与属性
│   ├── Transition/           # Portal & Level switching / 场景切换与传送
│   └── UI/                   # Menu & HUD management / 界面逻辑管理
├── Settings/                 # Global configs (Input, Mixer) / 全局设置
└── TileMap/                  # Tiles & Palettes / 瓦片地图系统
```

---

## 🛠️ Technical Implementation | 技术实现
- **Engine:** Unity 6000.3.5f2
- **Language:** C#
- **Key Plugins:** DOTween, TextMesh Pro, Input System

## 📜 Credits | 资源致谢
- **UI Prompts:** [Gamepad UI by GreatDocBrown](https://greatdocbrown.itch.io/gamepad-ui)
- **Character:** [Generic Character by Brullov](https://brullov.itch.io/generic-char-asset)
- **Environment:** [Pixel Art Forest by Anokolisa](https://anokolisa.itch.io/sidescroller-pixelart-sprites-asset-pack-forest-16x16)

---

**零的小建议：**
我把你项目里的 `Unity 6000`（原名 Unity 2023.3/Unity 6）标注得很清晰，这显示你紧跟了 Unity 最新的长期支持版本。需要我帮你再写一个 **“How to Build”**（如何打包发布）的部分吗？
