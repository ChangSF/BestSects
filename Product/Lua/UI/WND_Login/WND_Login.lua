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
            self.wndAccount.gameObject:SetActive(false);
            self.wndRegister.gameObject:SetActive(false);
            self.wndServerSelect.gameObject:SetActive(true);
            self:InitServerList();
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
            self.wndAccount.gameObject:SetActive(false);
            self.wndRegister.gameObject:SetActive(false);
            self.wndServerSelect.gameObject:SetActive(true);
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
	print()
    Lua2csMessenger.Instance:AddListener1("ResUserLogin", function(data)
        -- ResUserLoginMessage response = data as ResUserLoginMessage;
        Log.Error("登录结果=> ".. data.Msg);
        UIModule.Instance:CloseWindow("WND_Login")
        UIModule.Instance:OpenWindow("WND_Introduce","user1")
    end);
    local c=coroutine.create(function()
        Yield(WaitForSeconds(2));
        local www = WWW("http://47.94.220.1/serverlist.html")
        Yield(www)
        local  reader = JsonReader(www.text);
        local data = JsonMapper.ToObject(reader);
        local x = tonumber( data.Count);
        for i=1,x do
            local item = data:getItem(i-1);
            serverlist[i]={};
            serverlist[i].server=item:getItem("server"):ToString();
            serverlist[i].ip=item:getItem("ip"):ToString();
            serverlist[i].port=item:getItem("port"):ToString();
            Log.Error(item:getItem("ip"):ToString());
            bGetServerList=true;
        end
        -------------
        for i=2,10 do
            local item = data:getItem(0);
            serverlist[i]={};
            serverlist[i].server=tostring(i);
            serverlist[i].ip=item:getItem("ip"):ToString();
            serverlist[i].port=item:getItem("port"):ToString();
            bGetServerList=true;
        end
        ----------------------
    end)
    coroutine.resume(c);
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
    serverName.text=itemData.ip;
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
	NetworkModule.Instance:Connect(serverlist[selectedIndex].ip, 1520);
end

function WND_Login:LoginServer()
    local msg = ReqUserLoginMessage();
    msg.Username = "abc";
    msg.Password = "123";
    NetworkModule.Instance:Send(MessageID.ReqUserLogin,msg);
end

return WND_Login