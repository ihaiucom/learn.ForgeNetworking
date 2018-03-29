using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      11/24/2017 4:43:38 PM
*  @Description:    单位数据--控制器
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class UnitData
    {
        // 释放
        public override void OnRelease()
        {
            uid = 0;
            avatarConfig = null;
            name = null;
            legionId = 0;
            unitType = UnitType.None;
            buildType = UnitBuildType.None;
            buildCellUid = -1;
            routeId = -1;
            prop.Clear();
            skillList.Clear();
            skillDict.Clear();
            isInScene = false;
            isLive = true;
            isFix = false;
            attackCD = 0;
            base.OnRelease();
        }



        // =====================================
        // IUnitInstall
        // -------------------------------------
        /** 单位--安装 */
        public void UnitInstall()
        {

        }

        /** 单位--卸载 */
        public void UnitUninstall()
        {

        }


        // =====================================
        // ISyncedUpdate
        // -------------------------------------
        public override void OnSyncedUpdate()
        {
            //haoleContainer.Update();
            buffContainer.Update();

            //if(room.proxieModel.PServer())
            {
                if (IsEmploying)
                {
                    if (employType == UnityEmployType.Time && employCD != -1)
                    {
                        employCD -= LTime.deltaTime;

                        if (employCD <= 0)
                        {
                            RemoveEmploy();
                        }
                    }
                }
            }

            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].Update();
            }
        }

		private float _preHP = 0;

        // 每秒更新
        public void OnSecond()
        {
            // 血量恢复
            if (prop.HpRecover !=  0)
            {
                prop.Hp += prop.HpRecover;
                if (prop.Hp > prop.HpMax)
                    prop.Hp = prop.HpMax;
                else if(prop.Hp <= 0)
				{
					if (unitAgent != null) 
						unitAgent.punUnit.Death(-1);
                }
            }

			if(_preHP != 0 && _preHP != prop.Hp)
				showBloodTime = LTime.time;
			
			_preHP = prop.Hp;


            // 能量恢复
            if (buildType != UnitBuildType.Mainbase)
            {
                if (prop.EnergyRecover > 0)
                {
                    legionData.AddEnergy(prop.EnergyRecover);
                }
            }
        }



    }
}
