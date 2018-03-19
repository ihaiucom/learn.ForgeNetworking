-- 服务器与客户端直接的协议号必须大于等于10000
local _mt =
{
	OUTER_B2BM_Ping						= 1;
	OUTER_B2BM_RegNewBattleServer_Req	= 2;
	OUTER_B2BM_RegNewBattleServer_Resp	= 3;
	OUTER_BM2B_EnterGame_Req			= 4;
	OUTER_BM2B_EnterGame_Resp			= 5;
	OUTER_BM2B_GameOver_Req				= 6;
	OUTER_BM2B_GameOver_Resp			= 7;
}

-- 注册并检测消息
lx.cluster_opcode.registerMsgTypeTable(_mt)

return _mt