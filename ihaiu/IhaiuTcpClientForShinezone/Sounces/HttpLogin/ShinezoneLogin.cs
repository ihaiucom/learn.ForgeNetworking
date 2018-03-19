using BeardedManStudios.SimpleJSON;
using System;

public class ShinezoneLogin
{
	public static int PANEL_LOGIN 		= 1;
	public static int PANEL_REGISTER 	= 2;
	public static int PANEL_SERVER 		= 3;

	public enum StateID
	{
		Logining,
		Registering,
		LoadServerList
	}

	public string[] stateTxts = new string[]
	{
		"Logining ...",
		"Registering ...",
		"Load Server List ...",
	};
	
	/** 错误提示 */
	public event Action<int, string> 		errorEvent; 
	/** 显示状态提示 */
	public event Action<int, string> 		stateShowEvent; 
	/** 关闭状态提示 */
	public event Action 					stateHideEvent; 
	/** 打开面板 */
	public event Action<int> 				openPanelEvent; 
	/** 需要重新登录 */
	public event Action 					reloginEvent; 
	/** 登录成功 */
	public event Action<string> 			loginSuccessEvent; 
	/** 登录失败 */
	public event Action<string> 			loginFailEvent;
    /** 进入游戏成功 */
    public event Action<ulong, string, string>             loginEnterEvent;

    public ShinezoneAccount account;

    const int SUCCEED = 0;
	public ShinezoneLogin (ShinezoneAccount account)
	{
        this.account = account;
		account.requestEvent.OnLogin 		+= OnLogin;
		account.requestEvent.OnRegister 	+= OnRegister;
		account.requestEvent.OnServerList 	+= OnServerList;
		account.requestEvent.OnEnterGame 	+= OnEnterGame;
	}

	public void Error(int id, string err)
	{
        if(errorEvent != null)
		    errorEvent (id, err);
	}

	public void StateShow(StateID id, string txt)
    {
        if (stateShowEvent != null)
            stateShowEvent ((int)id, txt);
	}

	
	public void StateHide()
    {
        if (stateHideEvent != null)
            stateHideEvent ();
	}

	public void OpenPanel(int panelId)
    {
        if (openPanelEvent != null)
            openPanelEvent (panelId);
	}


    public void OnEnter(ulong accountId, string accountName, string session)
    {
        if (loginEnterEvent != null)
            loginEnterEvent(accountId, accountName, session);
    }


    /** 登录 */
    public void Login(string username, string password)
    {
        StateShow (StateID.Logining,  stateTxts[ (int) StateID.Logining]);
		account.Login (username, password, true);
	}

	/** 注册 */
	public void Register(string username, string password)
	{
		StateShow (StateID.Registering,  stateTxts[ (int) StateID.Registering]);
		account.Register (username, password);
	}

	/** 进入游戏 */
	public void EnterGame(int id, string ip, int port)
	{
        Loger.LogTagFormat("ShinezoneNet", "ShinezoneLogin EnterGame: id={0}, ip={1}, port={2}", id, ip, port);

        account.EnterGame (id.ToString(), ip, port.ToString() );
	}







	private void OnLogin(JSONNode result, int error_code, string error_msg)
	{
		if (error_code == SUCCEED) 
		{
			StateShow(StateID.LoadServerList, stateTxts[ (int) StateID.LoadServerList]);
            //account.ServerList();

            loginSuccessEvent(result.ToString());
        } 
		else
		{
			StateHide ();
			Error(error_code, error_msg);
			loginFailEvent(error_msg);
			Loger.LogError("error_code=" + error_code + " " + error_msg);
		}
	}

	
	private void OnRegister(JSONNode result, int error_code, string error_msg)
	{
		if (error_code == SUCCEED) 
		{
			StateShow(StateID.LoadServerList, stateTxts[ (int) StateID.LoadServerList]);
            //account.ServerList();
            loginSuccessEvent(result.ToString());
        } 
		else
		{
			StateHide ();
			Error(error_code, error_msg);
			loginFailEvent(error_msg);
			Loger.LogError("error_code=" + error_code + " " + error_msg);
		}
	}

	private void OnServerList(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result);

		StateHide ();
		if (error_code == SUCCEED) 
		{
			loginSuccessEvent(result.ToString());
			openPanelEvent(PANEL_SERVER);
		} 
		else
		{
			Error(error_code, error_msg);
			loginFailEvent(error_msg);
			Loger.LogError("error_code=" + error_code + " " + error_msg);
		}
	}

	
	
	private void OnEnterGame(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result);


		if (error_code == SUCCEED) 
		{
            //			UnityEngine.Application.LoadLevel ("Main");
            //			return;
            account.SetLastServerID(account.GetServerID());

			ulong accountId = ulong.Parse(result["account_id"]);
			string account_name = account.GetUserName();
			string session 	= result["M"];
            OnEnter(accountId, account_name, session);
		} 
		else
		{
			Error(error_code, error_msg);
			Loger.LogError("error_code=" + error_code + " " + error_msg);
		}
	}

	private void onNeedRestartDoWebAuth(object arg)
	{
		reloginEvent ();
		openPanelEvent (PANEL_LOGIN);
	}
}