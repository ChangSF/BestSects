
public class MSG_DEFINE
{
    public const string MSG_LOST_CONNECT_TO_SERVER = "MSG_LOST_CONNECT_TO_SERVER";  // 断线

    public const string MSG_SCENE_LOAD_PRE = "MSG_SCENE_LOAD_PRE";                 // 旧场景准备关闭（新场景准备加载）
    public const string MSG_SCENE_LOAD_COMPLETE = "MSG_SCENE_LOAD_COMPLETE";       // 场景加载完成, 参数1：str 场景名
    public const string MSG_SCENE_INIT_COMPLETE = "MSG_SCENE_INIT_COMPLETE";       // 场景初始化完成
    public const string MSG_LOADING_TO_END = "MSG_LOADING_TO_END";                  //将进度条加载到100%

    public const string MSG_PLAYER_REALIVE = "MSG_PLAYER_REALIVE";              // 主角复活  参数1：Player
    public const string MSG_PLAYER_DAMAGE = "MSG_PLAYER_DAMAGE";                // 主角受伤  参数1：Player  参数2：血量  //这个不对的呵呵
    public const string MSG_UPDATE_ROLE_INFO_PANEL = "MSG_UPDATE_ROLE_INFO_PANEL";//更新主角信息界面，血量、真气 参数可为空，参数：Player
    public const string MSG_MONSTER_DAMAGE = "MSG_MONSTER_DAMAGE";              // 怪物受伤  参数1：Monster 参数2：血量
    public const string MSG_MONSTER_BATICAST = "MSG_MONSTER_BATICAST";          // 怪物霸体值  参数1：Monster 参数2：当前霸体值

    public const string MSG_MONSTER_EXIT_VIEW = "MSG_MONSTER_EXIT_VIEW";        // 怪物离开视野（可能是死亡）  参数1：Monster
    public const string MSG_MONSTER_ENTER_VIEW = "MSG_MONSTER_ENTER_VIEW";      // 怪物进入视野（可能是创建）  参数1：Monster
    public const string MSG_MONSTER_DEATH = "MSG_MONSTER_DEATH";                // 怪物死亡  参数1：Monster
    public const string MSG_MONSTER_BORN = "MSG_MONSTER_BORN";                  // 怪物创建  参数1：Monster
    public const string MSG_MONSTER_RELEASE_SKILL = "MSG_MONSTER_RELEASE_SKILL";   //怪物开始释放某个技能   参数1：怪物actor 参数2：技能ID
    public const string MSG_MONSTER_RES_OUT = "MSG_MONSTER_RES_OUT";             // 怪物死亡后资源接管  参数1：Monster


    public const string MSG_PLAYER_DEATH = "MSG_PLAYER_DEATH";                  // 角色死亡  参数1：Player

    public const string MSG_SKILL_ENTER_CD = "MSG_SKILL_ENTER_CD";               // 技能冷却  参数1：冷却cd , 
    public const string MSG_SKILL_DAMAGED = "MSG_SKILL_DAMAGED";      // 攻击到怪物  参数1：攻击到到怪物数
    public const string MSG_SKILL_UPDATEICON = "MSG_SKILL_UPDATEICON";      // 切换图标  参数1：按钮id  参数2：技能id
    public const string MSG_SKILL_FIGHTUI_STANDBY = "MSG_SKILL_FIGHTUI_STANDBY";      // 主UI准备好

    public const string MSG_ITEM_FLY_PICK = "MSG_ITEM_FLY_PICK";                  // 物品主动飞向角色进行拾取  参数1：item
    public const string MSG_ITEM_FLY_PICK_ARRIVE = "MSG_ITEM_FLY_PICK_ARRIVE";    // 物品主动飞向角色到达  参数1：item
    public const string MSG_HERO_INIT_ATTR = "MSG_HERO_INIT_ATTR";                // 主角属性的初始化

    //技能
    public const string MSG_SKILL_CONFIGDATA_CHANGE     = "MSG_SKILL_CONFIGDATA_CHANGE";     //　技能配置数据更新
    public const string MSG_SKILL_USE_COMMONSKILL       = "MSG_SKILL_USE_COMMONSKILL";                // 主角使用通用技能
    public const string MSG_SKILL_USE_LAST_COMMONSKILL  = "MSG_SKILL_USE_LAST_COMMONSKILL";                // 主角使用通用技能的最后一个技能(只有怒气技能)
    public const string MSG_SKILL_USE_NORMALSKILL       = "MSG_SKILL_USE_NORMALSKILL";                // 主角使用普通技能    
    public const string MSG_SKILL_USE_FROMUI            = "MSG_SKILL_USE_FROMUI";                          // 通过界面使用技能    
    public const string MSG_SKILL_ACTOR_CONLLISION_OBJ  = "MSG_SKILL_ACTOR_CONLLISION_OBJ";      // 使用特殊技能时，角色碰撞到其它角色
    public const string MSG_SKILL_HIT_ACTOR             = "MSG_SKILL_HIT_ACTOR";        //技能击中某角色
    public const string MSG_PLAYER_ATTR_UPDATE          = "MSG_PLAYER_ATTR_UPDATE";      // 主角数据更新
    public const string MSG_EQUIP_PERF_CTRL_INIT        = "MSG_EQUIP_PERF_CTRL_INIT";   //装备数据转技能效果
    public const string MSG_SKILL_STATE_CHANGE          = "MSG_SKILL_STATE_CHANGE";     //状态发生改变 参数1：状态index，参数2：bool值



    //buff
    public const string MSG_BUFF_PLAYER_DISABLE_MOVE    = "MSG_BUFF_PLAYER_DISABLE_MOVE";        // 主角buff禁止移动
    public const string MSG_BUFF_PLAYER_ENABLE_MOVE     = "MSG_BUFF_PLAYER_ENABLE_MOVE";        // 主角buff解除禁止移动
    public const string MSG_BUFF_PLAYER_SORT            = "MSG_BUFF_PLAYER_SORT";              // 主角buff解除禁止移动
    public const string MSG_BUFF_CHANGE                 = "MSG_BUFF_CHANGE";                    // buff变化通知

    // 关卡事件
    public const string MSG_LEVEL_TRIGGER_READY = "MSG_LEVEL_TRIGGER_READY";    // 怪群触发器等待激活  参数1：怪群ID
    public const string MSG_LEVEL_TRIGGER_DONE = "MSG_LEVEL_TRIGGER_DONE";      // 怪群触发器已激活  参数1：怪群组ID, 参数2：怪群ID, 参数3:第几次触发
    public const string MSG_LEVEL_TRIGGER_DONE_SCRIPT = "MSG_LEVEL_TRIGGER_DONE_SCRIPT"; // 怪群触发器已激活  参数1：怪群组ID, 参数2：怪群ID, 参数3:第几次触发(传递给脚本）
    public const string MSG_LEVEL_CREATE_NEXT_TRIGGER = "MSG_LEVEL_CREATE_NEXT_TRIGGER"; // 怪物触发器已创建
    public const string MSG_LEVEL_SINGLE_ALL_MONSTER_KNOWED = "MSG_LEVEL_SINGLE_ALL_MONSTER_KNOWED";    //单人关卡里所有怪数据都知道 参数1：
    public const string MSG_LEVEL_CREATE_BATCH = "MSG_LEVEL_CREATE_BATCH";      // 创建了一波怪

    // 触屏点击事件
    public const string MSG_CLICK_HAPPEN = "MSG_CLICK_HAPPEN";
    public const string MSG_CLICK_PLAYER           = "MSG_CLICK_PLAYER";             // 角色被点了  参数1：IActor
    public const string MSG_CLICK_NPC = "MSG_CLICK_NPC";                             // Npc被点中了 参数1：Transform
    public const string MSG_CLICK_FUNCTION_OBJECT = "MSG_CLICK_FUNCTION_OBJECT";     // 点中了功能节点  参数1：Transform
    public const string MSG_CLICK_OBJECT = "MSG_CLICK_OBJECT";                       // 点中了其它节点  参数1：Transform
    public const string MSG_CLICK_PICK_OBJ = "MSG_CLICK_PICK_OBJ";                      //点中了采集怪 参数1：Transform
    public const string MSG_CLICK_NOTHING = "MSG_CLICK_NOTHING";                      // 什么都没点中

    // 网络相关事件
    public const string MSG_NETWORK_LOGOUT = "MSG_NETWORK_LOGOUT";            //　注销协议返回成功

    // 技能状态事件
    public const string MSG_CAST_SPELL_SUCCESS = "MSG_CAST_SPELL_SUCCESS";  // 成功释放技能 参数1：m_actor 参数2：技能ID
    public const string MSG_ACT_STATE_START = "MSG_ACT_STATE_START";        // 技能状态启动 参数1:CasterIActor, 参数1:IActor, 参数2:SkillProperty
    public const string MSG_SRV_CAST_NPC_SKILL = "MSG_SRV_CAST_NPC_SKILL";   // 服务器控制的技能 参数1：caster actor, 参数2： skill id

    public const string MSG_REFRESH_CHARACTER_ATTR = "MSG_REFRESH_CHARACTER_ATTR";  // 角色属性计算刷新

    public const string MSG_OPEN_LEVELSEL_PANEL = "MSG_OPEN_LEVELSEL_PANEL";  // 打开关卡选择

    public const string MSG_CREATE_SCENE_SWITCH = "MSG_CREATE_SCENE_SWITCH";  //创建场景跳转点，参数1：跳转物体，参数2：跳转ID
    
    public const string MSG_JIGGLE_BEGIN_MOVE = "MSG_JIGGLE_BEGIN_MOVE";      //摇杆移动事件开始触发

    public const string MSG_JIGGLE_STOP_MOVE = "MSG_JIGGLE_STOP_MOVE";      //摇杆停止移动事件

    public const string MSG_MONSTER_AI_GUARD = "MSG_MONSTER_AI_GUARD";      //怪物开始警戒，参数1：警戒目标  参数2：monster 

    //过场触发
    public const string MSG_SCENARIO_TRIGGER = "MSG_SCENARIO_TRIGGER";      //过场动画触发，参数： index 过场动画文件索引号 
    public const string MSG_SCENARIO_STOP = "MSG_SCENARIO_STOP";            //过场动画结束
    public const string MSG_SWICH_SHOW_HIDE_FIGHT_UI = "MSG_SWICH_SHOW_HIDE_FIGHT_UI";        //隐藏显示战斗ui 参数1: bool值，隐藏还是显示
    public const string MSG_SWICH_SHOW_HIDE_PRTHFIDING = "MSG_SWICH_SHOW_HIDE_PRTHFIDING"; //隐藏副本内导航 参数1: bool值，隐藏还是显示
    public const string MSG_SCENARIO_SEND_EVENT = "MSG_SCENARIO_SEND_EVENT";    //过场动画事件交互，参数：int 事件id,实时动画系统发送给外部系统
    public const string MSG_SEND_TO_SCENARIO_EVENT = "MSG_SEND_TO_SCENARIO_EVENT"; //外部系统向实时动画系统发送事件
    public const string MSG_RESUME_SCENARIO_ACTION = "MSG_RESUME_SCENARIO_ACTION";
    public const string MSG_SCENARIO_AI_TRIGGER = "MSG_SCENARIO_AI_TRIGGER"; //AI中触发实时动画的消息, 参数：monsterID,actionType,param,和表对应

    public const string MSG_USEQUENCE_REPLACE_RES = "MSG_USEQUENCE_REPLACE_RES"; //用实时资源替换动画里的资源, 参数1：id,参数2：之前的资源名
    public const string MSG_USEQUENCE_STOP_PAUSE = "MSG_USEQUENCE_STOP_PAUSE"; //实时动画暂停或停止, 参数：1表示停止，2表示暂停
    public const string MSG_USEQUENCE_UI_SHOW_HIDE = "MSG_USEQUENCE_UI_SHOW_HIDE";  //剧情动画，UI的显示与隐藏
    public const string MSG_USEQUENCE_HERO_SHOW_HIDE = "MSG_USEQUENCE_HERO_SHOW_HIDE";  //剧情动画，Hero的显示与隐藏
    public const string MSG_USEQUENCE_ACTOR_EFFECT_SHOW = "MSG_USEQUENCE_ACTOR_EFFECT_SHOW";  //剧情动画，ActorEffect挂载


    public const string MSG_STOP_JIGGLE_CONTROL_MOVE_EVENT = "MSG_STOP_JIGGLE_CONTROL_MOVE_EVENT";        //停止移动摇动产生的移动

    //身上的属性发生变化
    public const string MSG_ATTR_CHANGE = "MSG_ATTR_CHANGE";      //身上的属性发生了变化 参数1：uikey，参数2：uiFrom，参数3：uiTo,参数4：int64 actorid

    //情景对话
    public const string MSG_NPC_INTER_DIALOGUE_CLOSE = "MSG_NPC_INTER_DIALOGUE_CLOSE";

    // 加载完level，并且隐藏了loading界面
    public const string MSG_HIDE_LEVEL_LOADING_COMPLETE = "MSG_HIDE_LEVEL_LOADING_COMPLETE";

    //销毁版本更新UI
    public const string MSG_DESTROY_VERSION_UPDATE_UI = "MSG_DESTROY_VERSION_UPDATE_UI";

    public const string MSG_SPLASH_BLOOD = "MSG_SPLASH_BLOOD"; //--屏幕溅血效果
    public const string MSG_BROKEN_SCREEN = "MSG_BROKEN_SCREEN"; //--屏幕破碎效果
    public const string MSG_PLAYER_OPERATE = "MSG_PLAYER_OPERATE";
    public const string MSG_SHOW_FLASH_SCREEN = "MSG_SHOW_FLASH_SCREEN"; //--显示闪屏效果
    public const string MSG_CLOSE_FLASH_SCREEN = "MSG_CLOSE_FLASH_SCREEN"; //--关闭闪屏效果

    //刷新网络状态UI
    public const string MSG_Refresh_NET_STATE_UI = "MSG_Refresh_NET_STATE_UI";

    //特殊挑战相关
    public const string MSG_NOTIFY_SKILL_USE = " MSG_NOTIFY_SKILL_USE";          //释放了技能 
    public const string MSG_MONSTER_DEATH_INBATI = "MSG_MONSTER_DEATH_INBATI";      //怪物霸体下死亡
    public const string MSG_KILLED_MONTERS_INFRAME = "MSG_KILLED_MONTERS_INFRAME";      //一个判定帧杀死了规定数量的怪物
    public const string MSG_HIT_BY_SKILL = "MSG_HIT_BY_SKILL";      //主角被技能击中了
    public const string MSG_MONSTER_DEATH_INBUFF = "MSG_MONSTER_DEATH_INBUFF";      //怪物buff下死亡

    //怒气相关
    public const string MSG_ADD_ANGER_VALUE = "MSG_ADD_ANGER_VALUE";     //增加怒气了 参数一：技能id，参数二：是否触发会心一击,参数三：是否是被攻击
    public const string MSG_USE_ANGER_SKILL = "MSG_USE_ANGER_SKILL";     //怒气技能开始和借宿 参数:bool型，怒气技能开始为true，结束为false
    public const string MSG_REFRESH_ANGER_SKILL_DATA = "MSG_REFRESH_ANGER_SKILL_DATA";     //技能UI，怒气装备发生改变时执行

    //血条相关
    public const string MSG_REFRESSH_PLAYER_HUD_UI = "MSG_REFRESSH_PLAYER_HUD_UI";     //使用了怒气技能

    //野外
    public const string MSG_ENTER_SAFE_AREA = "MSG_ENTER_SAFE_AREA";     //安全区进出 参数1：actor
    public const string MSG_EXIT_SAFE_AREA = "MSG_EXIT_SAFE_AREA";     //安全区进出 参数1：actor

    //轻功
    public const string MSG_DODGE_STATE = "MSG_DODGE_STATE";     //轻功 参数1：actor 参数2:状态true\false
    //采集
    public const string MSG_GATHER_STATE_BEGIN = "MSG_GATHER_STATE_BEGIN";           //采集开始
    public const string MSG_GATHER_STATE_FAIL = "MSG_GATHER_STATE_FAIL";            //采集失败
    public const string MSG_GATHER_STATE_SUCCESS = "MSG_GATHER_STATE_SUCCESS";      //采集成功


    //云娃语音
    public const string YUNVA_LOGIN_RES = "YUNVA_LOGIN_RES";	//玩家登录云娃结果
    public const string YUNVA_LOGOUT_RES = "YUNVA_LOGOUT_RES";	//玩家登出云娃
    public const string YUNVA_CHANNEL_LOGIN_RES = "YUNVA_CHANNEL_LOGIN_RES";	//玩家登录频道结果
    public const string YUNVA_CHANNEL_LOGOUT_RES = "YUNVA_CHANNEL_LOGOUT_RES";	//玩家登出频道结果
    public const string YUNVA_CHANNEL_MESSAGE_NOTIFY = "YUNVA_CHANNEL_MESSAGE_NOTIFY";	//频道消息通知
    public const string YUNVA_CHAT_FRIEND_NOTIFY = "YUNVA_CHAT_FRIEND_NOTIFY";	//玩家私聊通知
    public const string YUNVA_RECORD_RESULT = "YUNVA_RECORD_RESULT";    //录音结果
    public const string YUNVA_SPEECH_RECOGNIZE_RES = "YUNVA_SPEECH_RECOGNIZE_RES";    //语音识别结果
    public const string YUNVA_SEND_CHANNEL_VOICE_RES = "YUNVA_SEND_CHANNEL_VOICE_RES";  //发送频道语音消息结果
    public const string YUNVA_SEND_CHANNEL_TEXT_RES = "YUNVA_SEND_CHANNEL_TEXT_RES";    //发送频道文本消息结果
    public const string YUNVA_SEND_P2P_VOICE_RES = "YUNVA_SEND_P2P_VOICE_RES";  //发送私聊语音消息结果
    public const string YUNVA_SEND_P2P_TEXT_RES = "YUNVA_SEND_P2P_TEXT_RES";    //发送私聊文本消息结果
    public const string YUNVA_RECORD_VOLUME_NOTIFY = "YUNVA_RECORD_VOLUME_NOTIFY";  //录音时音量变化
    //采集，钓鱼
    public const string MSG_ENTER_PICK_TRIGGER = "MSG_ENTER_PICK_TRIGGER";    //进入采集怪触发区域 参数1：bool 是否是私有怪，参数2： guid， 参数3：configID
    public const string MSG_EXIT_PICK_TRIGGER = "MSG_EXIT_PICK_TRIGGER";    //离开采集怪触发区域 参数1：bool 是否是私有怪，参数2： guid， 参数3：configID
    public const string MSG_ENTER_FISHING_TRIGGER = "MSG_ENTER_FISHING_TRIGGER";    //进入鱼塘范围 参数：鱼塘index
    public const string MSG_EXIT_FISHING_TRIGGER = "MSG_EXIT_FISHING_TRIGGER";    //离开鱼塘范围 参数：鱼塘index
    public const string MSG_PICK_MONSTER_DEATH = "MSG_PICK_MONSTER_DEATH";      //采集怪死亡

    //头顶气泡
    public const string MSG_MONSTER_SPEECH = "MSG_MONSTER_SPEECH";  //怪物头顶气泡

    //骑乘相关
    public const string MSG_GET_ON_RIDE = "MSG_GET_ON_RIDE";    // 成功上马  参数1：Player
    public const string MSG_GET_OFF_RIDE = "MSG_GET_OFF_RIDE";    // 成功下马  参数1：Player
    public const string MSG_GET_OFF_RIDE_NEED_SEND_SER = "MSG_GET_OFF_RIDE_NEED_SEND_SER";  //通知server主角下马
    public const string MSG_RIDE_BTN_ENABLE = "MSG_RIDE_BTN_ENABLE";  //骑马按钮变灰
    public const string MSG_RIDE_BTN_DISABLE = "MSG_RIDE_BTN_DISABLE";  //骑马按钮变灰

    public const string MSG_HERO_MOVE_EVENT = "MSG_HERO_MOVE_EVENT";   //主角移动状态 参数：bool 是否在移动
}