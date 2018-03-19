using BeardedManStudios.SimpleJSON;
using System;
using System.Collections.Generic;

public class ShinezoneAccountEventName
{
	/// 注册
	public static string register 				= "register";
	/// 游客
	public static string tourists 				= "tourists";
	/// 登陆
	public static string login 					= "login";
	/// 改密码
	public static string change_pwd 			= "change_pwd";
	/// 改用户名
	public static string change_user 			= "change_user";
	/// 改用户名和密码
	public static string change_user_pwd 		= "change_user_pwd";
	/// 服务器列表
	public static string server_list 			= "server_list";
	/// 进入指定服
	public static string enter_game 			= "enter_game";
	/// 领取礼包
	public static string use_gift 				= "use_gift";
	/// 充值
	public static string do_recharge 			= "do_recharge";
}

public class ShinezoneAccountEvent
{

	public delegate void ResFunc(JSONNode result, int error_code, string error_msg);
	
	private Dictionary<string, ResFunc> _func_tb = new Dictionary<string, ResFunc>();
	
	/// 请求成功
	public event Action<JSONNode> OnRegisterSuccess;
	/// 请求失败 url, httpstate
	public event Action<string, int> OnRequestFail;
	
	/// 注册
	public event ResFunc OnRegister;
	/// 游客
	public event ResFunc OnTourists;
	/// 登陆
	public event ResFunc OnLogin;
	/// 改密码
	public event ResFunc OnChangePwd;
	/// 改用户名
	public event ResFunc OnChangeUser;
	/// 改用户名和密码
	public event ResFunc OnChangeUserPwd;
	/// 服务器列表
	public event ResFunc OnServerList;
	/// 进入指定服
	public event ResFunc OnEnterGame;
	/// 领取礼包
	public event ResFunc OnUseGift;
	/// 充值
	public event ResFunc OnDoRecharge;
	
	public ShinezoneAccountEvent ()
	{
		Init ();
	}
	
	/**
	 * 初始化
	 * */
	public void Init()
	{
		_func_tb ["register"] 	= on_register_res;
		_func_tb ["tourists"] 	= on_tourists_res;
		_func_tb ["login"] 		= on_login_res;
		_func_tb ["change_pwd"] = on_change_pwd_res;
		_func_tb ["change_user"] = on_change_user_res;
		_func_tb ["change_user_pwd"] = on_change_user_pwd_res;
		_func_tb ["server_list"] = on_server_list_res;
		_func_tb ["enter_game"] = on_enter_game_res;
		_func_tb ["use_gift"] = on_use_gift_res;
		_func_tb ["do_recharge"] = on_do_recharge_res;
	}
	
	/**
	 * 当请求成功成功执行时
	 * */
	public void OnRequestResult (JSONNode result)
	{
		string opcode = result ["opcode"];
		if (opcode == null)
		{
			Loger.LogError(String.Format("error code:{0}, error msg:{1}", result["error_code"], result["error_msg"]));
			return;
		}
		
		if (!_func_tb.ContainsKey (opcode))
		{
			Loger.LogError ("not find response handler, opcode:" + opcode);
			return;
		}
		
		ResFunc func = _func_tb [opcode];
		if (func != null)
			func (result, result ["error_code"].AsInt, result ["error_msg"]);
		
		if(OnRegisterSuccess != null) OnRegisterSuccess (result);
	}
	
	
	/**
	 * 当请求遇到错误时
	 * */
	public void OnRequestError(string url, int httpcode)
	{
		Loger.LogError ("OnRequestError, url:" + url + ", http code:" + httpcode);
		if(OnRequestFail != null) OnRequestFail (url, httpcode);
	}
	
	
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// 注册
	private void on_register_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		OnRegister (result, error_code, error_msg);
	}
	
	/// 游客
	private void on_tourists_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		OnTourists (result, error_code, error_msg);
	}
	
	/// 登陆
	private void on_login_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		OnLogin (result, error_code, error_msg);
	}
	
	/// 改密码
	private void on_change_pwd_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		
		OnChangePwd (result, error_code, error_msg);
	}
	
	/// 改用户名
	private void on_change_user_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		OnChangeUser (result, error_code, error_msg);
	}
	
	/// 改用户名和密码
	private void on_change_user_pwd_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		OnChangeUserPwd (result, error_code, error_msg);
	}
	
	/// 服务器列表
	private void on_server_list_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		
		OnServerList (result, error_code, error_msg);
	}
	
	/// 进入指定服
	private void on_enter_game_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		if(OnEnterGame != null) OnEnterGame (result, error_code, error_msg);
	}
	
	/// 领取礼包
	private void on_use_gift_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		
		OnUseGift (result, error_code, error_msg);
	}
	
	/// 充值
	private void on_do_recharge_res(JSONNode result, int error_code, string error_msg)
	{
		Loger.Log (result["opcode"] + error_msg);
		OnDoRecharge (result, error_code, error_msg);
	}
}
