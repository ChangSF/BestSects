--WND_Login
using "UnityEngine"
using "LitJson"
using "BestSects.Net"
using "BestSects.protocol"
local UIBase = import("UI/UIBase")
if not Cookie then
    Cookie = Slua.GetClass('KSFramework.Cookie')
end

if not I18N then
    I18N = Slua.GetClass('KSFramework.I18N') -- use slua reflection mode
end

if not UIModule then
    UIModule = Slua.GetClass('KEngine.UI.UIModule')
end

if not Log then
    Log = Slua.GetClass('KEngine.Log')
end


local  WND_Login = {}
extends(WND_Login , UIBase)
local this;
local serverlist={};
local serverlistGOs = {};
local bGetServerList = false;
local  selectedServer;
local selectedIndex = 1;

function WND_Login.New(controller)
    local newWND_Login = new(WND_Login)
    newWND_Login.Controller = controller
    return newWND_Login
end

function WND_Login:OnInit(controller)
	Log.Error("================================ WND_Login:OnInit ============================")
    --在NEW中无法获得self上面的数据的
    this=self;
    WND_Login.serverListUrl="http://47.94.220.1/serverlist.html";
	if UnityEngine and  UnityEngine.Vector3 then 
        self.btnLogin.onClick:RemoveAllListeners()
        self.btnLogin.onClick:AddListener(function()
            if bGetServerList == false then
                return;
            end
            self:Login_LoginServer();
            
        end)
    end
    if UnityEngine and  UnityEngine.Vector3 then 
        self.btnRegWnd.onClick:RemoveAllListeners()
        self.btnRegWnd.onClick:AddListener(function()
            self.wndAccount.gameObject:SetActive(false);
            self.wndRegister.gameObject:SetActive(true);
            self.wndServerSelect.gameObject:SetActive(false);
        end)
    end
    if UnityEngine and  UnityEngine.Vector3 then 
            self.btnReg.onClick:RemoveAllListeners()
            self.btnReg.onClick:AddListener(function()
            self:Reg_LoginServer();
        end)
    end
    if UnityEngine and  UnityEngine.Vector3 then 
        self.btnEnter.onClick:RemoveAllListeners()
        self.btnEnter.onClick:AddListener(function()
            -- UIModule.Instance:CloseWindow("WND_Login")
            -- UIModule.Instance:OpenWindow("WND_Introduce","user1")
            self:ConnectServer();
        end)
    end
    -- error(Slua:version());

end

function WND_Login:OnOpen()
    Log.Error("================================ WND_Login:OnOpen ============================")
    print(self.serverItemTemplate.gameObject);
    Lua2csMessenger.Instance:AddListener("NetworkConnect", function()
        -- ResUserLoginMessage response = data as ResUserLoginMessage;
        Log.Error("连接成功");
        self:LoginServer();

    end);
	print(MessageID.NetworkConnect);
    Lua2csMessenger.Instance:AddListener1("ResLogin", function(data)
        -- ResUserLoginMessage response = data as ResUserLoginMessage;
        Log.Error("登录结果=> ".. data.Code);
        if data.Code == 0 then
            Cookie.Set("CurrentPlayerInfo",data.PlayerInfo);
            UIModule.Instance:CloseWindow("WND_Login");
            UIModule.Instance:OpenWindow("WND_Introduce","user1");
        end
    end);
    --local c=coroutine.create(function()
    --    Yield(WaitForSeconds(2));
    --    local www = WWW(WND_Login.serverListUrl);
    --    Yield(www)
    --    local  reader = JsonReader(www.text);
    --    local data = JsonMapper.ToObject(reader);
    --    --{"login":{"ip":"47.94.220.1","port":1588},"game":[{"server":1,"ip":"47.94.220.1","port":9001}]}
    --    self.loginServer = {};
    --    local LoginServerData = data:getItem("login");
    --    self.loginServer.ip=LoginServerData:getItem("ip"):ToString();
    --    self.loginServer.port=LoginServerData:getItem("port"):ToString();
    --    local gameServerList = {};
    --    local serverListData= data:getItem("game");
    --    local x = tonumber( serverListData.Count);
    --    for i=1,x do
    --        local item = serverListData:getItem(i-1);
    --        serverlist[i]={};
    --        serverlist[i].server=item:getItem("server"):ToString();
    --        serverlist[i].ip=item:getItem("ip"):ToString();
    --        serverlist[i].port=item:getItem("port"):ToString();
    --        serverlist[i].name=item:getItem("name"):ToString();
    --        Log.Error(item:getItem("ip"):ToString());
    --        bGetServerList=true;
    --    end
    --    -------------
    --    -- for i=2,10 do
    --    --     local item = serverListData:getItem(0);
    --    --     serverlist[i]={};
    --    --     serverlist[i].server=tostring(i);
    --    --     serverlist[i].ip=item:getItem("ip"):ToString();
    --    --     serverlist[i].port=item:getItem("port"):ToString();
    --    --     bGetServerList=true;
    --    -- end
    --    ----------------------
    --end);
    --coroutine.resume(c);
    local serverListJson = Cookie.Get("serverListJson",function()
        Log.Error("无法获取到服务器列表!");
    end);
    Debug.LogError(tostring(serverListJson));
    local  reader = JsonReader(serverListJson);
    local data = JsonMapper.ToObject(reader);
    --{"login":{"ip":"47.94.220.1","port":1588},"game":[{"server":1,"ip":"47.94.220.1","port":9001}]}
    self.loginServer = {};
    local LoginServerData = data:getItem("login");
    self.loginServer.ip=LoginServerData:getItem("ip"):ToString();
    self.loginServer.port=LoginServerData:getItem("port"):ToString();
    local gameServerList = {};
    local serverListData= data:getItem("game");
    local x = tonumber( serverListData.Count);
    for i=1,x do
        local item = serverListData:getItem(i-1);
        serverlist[i]={};
        serverlist[i].server=item:getItem("server"):ToString();
        serverlist[i].ip=item:getItem("ip"):ToString();
        serverlist[i].port=item:getItem("port"):ToString();
        serverlist[i].name=item:getItem("name"):ToString();
        Log.Error(item:getItem("ip"):ToString());
        bGetServerList=true;
    end
end

function WND_Login:Login_LoginServer()
    if self.inputAccount.text =="" and self.inputPassword.text=="" then
        return;
    end
    local d=coroutine.create(function()
    --去登录服登录
        local url = "http://"..self.loginServer.ip..":"..self.loginServer.port.."/loGIN?UsERnaME="..self.inputAccount.text.."&pASsWOrD="..self.inputPassword.text;
        Debug.LogError(url);
        local www = WWW(url);
        Yield(www);
        ------------------
        --{"code":0,"roles":[{"uid":"111","level":1,"sex":1,"nickname":"小明","serverID":1}],"token":"54658079ec552aa802d6037d28bf57e3"}
        Debug.LogError(www.text);
        if www.error==nil or www.error == "" then
            self.username=self.inputAccount.text;
            self.password=self.inputPassword.text;
            self:ProcessLoginServerData(www.text);
        else
            Log.error("网络出错,无法访问登录服务器!");
        end
    end);
    coroutine.resume(d);
end
function WND_Login:Reg_LoginServer()
    local account = self.regAccount.text;
    local password = self.regPassword.text;
    local password2 = self.regPassword2.text;
    if password ~= password2 then
        Log.Error("两次输入的密码不正确!");
        return;
    end
    local e=coroutine.create(function()
        local url="http://"..self.loginServer.ip..":"..self.loginServer.port.."/rEgIsTER?USerNAmE="..self.regAccount.text.."&PAsSWoRD="..self.regPassword.text;
        Debug.LogError(url);
        local www = WWW(url);
        Yield(www);
        if www.error== nil or www.error == "" then
            self.username=self.regAccount.text;
            self.password=self.regPassword.text;
            self:ProcessLoginServerData(www.text);
        else
            Log.Error(www.error);
        end
    end);
    coroutine.resume(e);
end
function WND_Login:ProcessLoginServerData(jsonText)
    local  reader = JsonReader(jsonText);
    local data = JsonMapper.ToObject(reader);
    local code = data:getItem("code");
    --Debug.LogError("Code => "..tostring( tonumber(code:ToString())))
    if tonumber(code:ToString()) == 0 then
        self.roles={};
        local roles=data:getItem("roles");
        local x = tonumber(roles.Count);
        Debug.LogError("roles => "..tostring( tonumber(roles.Count)))
        for i=1,x do
            self.roles[i]={};
            local role = roles:getItem(i-1);
            self.roles[i].level = role:getItem("level");
            self.roles[i].nickname=role:getItem("nickname");
            self.roles[i].serverID = role:getItem("serverID");
            self.roles[i].sex = role:getItem("sex");
            self.roles[i].uid=role:getItem("uid");
        end
        self.token=data:getItem("token");
        Cookie.Set("token",self.token);
        Cookie.Set("roles",self.roles);
        self.wndAccount.gameObject:SetActive(false);
        self.wndRegister.gameObject:SetActive(false);
        self.wndServerSelect.gameObject:SetActive(true);
        self:InitServerList();
    else
        Log.Error("error code => ".. tostring( code));
    end
end
function  WND_Login:InitServerList()
    local x = 1;
    for k,v in pairs(serverlist) do
        local item = serverlistGOs[x];
        if item == nil then
            item = GameObject.Instantiate(self.serverItemTemplate.gameObject);
            serverlistGOs[x]=item;
        end
        x = x + 1;
        item:SetActive(true);
        item.name=v.server;
        local trans = item.transform;
        trans:SetParent(self.serverListContent);
        self:InitServerItem(trans,v);
    end
    if x - 1 < #serverlistGOs then
        for i=x,#serverlistGOs do
            if serverlistGOs[i] ~=nil then
                serverlistGOs[i]:SetActive(false);
            end
        end
    end
end

function WND_Login:InitServerItem(itemTrans,itemData)
    local selectState = itemTrans:Find("selectState"):GetComponent("Image");
    -- local serverNameBg= itemTrans:Find("serverNameBg");
    local serverNum = itemTrans:Find("serverNum/Text"):GetComponent("Text");
    local serverState = itemTrans:Find("serverState"):GetComponent("Text");
    local serverName = itemTrans:Find("serverName"):GetComponent("Text");
    if tonumber(itemData.server)==selectedIndex then
        selectState.color=Color32(255,255,255,255);
    else
        selectState.color=Color32(255,255,255,1);
    end
    serverNum.text=itemData.server;
    serverState.gameObject:SetActive(false);
    serverName.text=itemData.name;
    local btnState = selectState:GetComponent("Button");
    local index = tonumber(itemTrans.name);
    btnState.onClick:RemoveAllListeners()
    btnState.onClick:AddListener(function()
        if selectedIndex == index then
            return;
        end
        selectedIndex=index;
        print("选择了=> ".. tostring(index));
        self:InitServerList();
    end);
end

function WND_Login:ConnectServer()
--    NetworkModule.Instance:Connect(serverlist[selectedIndex].ip, tonumber(serverlist[selectedIndex].port));
	NetworkModule.Instance:Connect(serverlist[selectedIndex].ip, tonumber( serverlist[selectedIndex].port));
end

function WND_Login:LoginServer()
    local msg = ReqLoginMessage();
    Log.Error("z =>"..tostring(self.token));
    msg.Token =tostring(self.token);--登录令牌
    msg.Username = self.username;--帐号
    msg.Channel = 0;--玩家渠道
    msg.DeviceId = "abcdef";--设备号
    NetworkModule.Instance:Send(MessageID.ReqLogin,msg);
end

return WND_Login