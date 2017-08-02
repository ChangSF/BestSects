--WND_Login
using "UnityEngine"
using "LitJson"
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
local serverlist={};


function WND_Login.New(controller)
    local newWND_Login = new(WND_Login)
    newWND_Login.Controller = controller
    return newWND_Login
end

function WND_Login:OnInit(controller)
	Log.Info("================================ WND_Login:OnInit ============================")

    WND_Login.serverListUrl="http://47.94.220.1/serverlist.html";
	if UnityEngine and  UnityEngine.Vector3 then 
        self.btnLogin.onClick:RemoveAllListeners()
        self.btnLogin.onClick:AddListener(function()
            self.wndAccount.gameObject:SetActive(false);
            self.wndRegister.gameObject:SetActive(false);
            self.wndServerSelect.gameObject:SetActive(true);
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
            UIModule.Instance:CloseWindow("WND_Login")
            UIModule.Instance:OpenWindow("WND_Introduce","user1")
        end)
    end
    -- error(Slua:version());
    local c=coroutine.create(function()
        print "coroutine start"
        local wait =WaitForSeconds(2);
        Yield(wait);
        print "coroutine WaitForSeconds 2"
        local www = WWW("http://47.94.220.1/serverlist.html")
        Yield(www)
        
        local  reader = JsonReader(www.text);
        local data = JsonMapper.ToObject(reader);
        local x = tonumber( data.Count);
        for i=1,x do
            local item = data:getItem(i-1);
            serverlist[i]={};
            serverlist[i]["server"]=item:getItem("server"):ToString();
            serverlist[i]["ip"]=item:getItem("ip"):ToString();
            serverlist[i]["port"]=item:getItem("port"):ToString();
            Log.Error(item:getItem("ip"):ToString());
        end
    end)
    coroutine.resume(c);
end

function WND_Login:OnOpen()
    
end





return WND_Login