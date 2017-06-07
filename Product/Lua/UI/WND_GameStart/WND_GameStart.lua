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

local  WND_GameStart = {}
extends(WND_GameStart , UIBase)

function WND_GameStart.New(controller)
    local newUI_Introduce = new(WND_GameStart)
    newUI_Introduce.Controller = controller
    return newUI_Introduce
end

function WND_GameStart:OnInit(controller)
	Log.Info("================================ WND_GameStart:OnInit ============================")
	-- self.Controller=controller
	-- -- self.playBtn=self:GetUIText('Title')
	-- -- this button click to load new UI
 --    local playBtn = self.PlayBtn
    
 --    if UnityEngine and  UnityEngine.Vector3 then -- static code binded!
 --        playBtn.onClick:RemoveAllListeners()
 --        playBtn.onClick:AddListener(function()
	-- 		--UIModule.Instance:CloseWindow("Login")
	-- 		--UIModule.Instance:OpenWindow("Main","user1")
 --            print('Click the button!!!')
 --            local audioName = AppSettings.AudioConfigSettings.Get('zh').FileName
 --            print(audioName)
 --            local audioSource = self.MyAudioSource
 --            --audioSource.Play()
 --            local  audio = self:GetControl("UnityEngine.AudioSource","Audio Source")
 --            print(audio)
 --            print(audioSource)
 --            local image=self.image
 --            print(image)
 --        end)
 --        print('Success bind button OnClick!')
 --    else
 --        Long.Warning('MainButton need Slua static code.')
 --    end
    
end

function WND_GameStart:OnOpen()
    -- self.playBtn.text = "This is a title"
end

return WND_GameStart