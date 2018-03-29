using Ihaius;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/27/2017 9:27:39 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class AbstractUnitMonoBehaviour : MonoBehaviour, IRoomObject, IUnitComponent, IGamePause
    {
        virtual protected void OnDestroy()
        {
            CRelease();
        }

        #region PrefabPool
        /** PrefabPool--设置为使用状态消息 */
        public void OnSpawned(PrefabPool pool)
        {
            CReset();
        }


        /** PrefabPool--设置为闲置状态消息 */
        public void OnDespawned(PrefabPool pool)
        {
            CRelease();
        }
        #endregion

        #region IScript
        private bool isRest = false;
        private bool isRelease = true;
        /** 重置 */
        public void CReset()
        {
            if (!isRest)
            {
                isRest = true;
                isRelease = false;
                OnReset();
            }
        }

        /** 释放 */
        public void CRelease()
        {
            if (!isRelease)
            {
                isRest = false;
                isRelease = true;
                OnRelease();
            }
        }

        /** 销毁 */
        public void CDealloc()
        {
            CRelease();
        }


        /** 重置 */
        virtual protected void OnReset()
        {
        }

        /** 释放 */
        virtual protected void OnRelease()
        {
            this.room = null;
            this._unitData = null;
            this._unitAgent = null;
        }


        #endregion


        // =====================================
        // IRoomObject
        // -------------------------------------
        /** 房间--门面 */
        public WarRoom room { get; set; }

        /** 房间--场景数据 */
        public WarSceneData sceneData
        {
            get
            {
                return room.sceneData;
            }
        }



        /** 房间--视图代理 */
        public WarUnityViewAgent clientViewAgent
        {
            get
            {
                return room.clientViewAgent;
            }
        }

        /** 房间--场景视图 */
        public WarUnitySceneView clientSceneView
        {
            get
            {
                return room.clientSceneView;
            }
        }

        /** 房间--场景视图 */
        public WarUnityRes clientRes
        {
            get
            {
                return room.clientRes;
            }
        }


        /** 房间--时间 */
        public WarTime Time
        {
            get
            {
                return room.Time;
            }
        }



        /** 房间--逻辑时间 */
        public WarLTime LTime
        {
            get
            {
                return room.LTime;
            }
        }


        // =====================================
        // IGamePause
        // -------------------------------------
        /** 游戏--暂停 */
        virtual public void OnGamePause()
        {

        }

        /** 游戏--继续 */
        virtual public void OnGameUnPause()
        {

        }

        /** 游戏--游戏结束 */
        virtual public void OnGameOver()
        {

        }


        // =====================================
        // ISyncedUpdate
        // -------------------------------------

        /** 游戏--同步更新 */
        //virtual public void OnSyncedUpdate()
        //{

        //}


        // =====================================
        // IUnitComponent
        // -------------------------------------

        private GameObject _gameObject;
        private Transform _transform;
        private UnitAgent _unitAgent;
        private UnitData _unitData;
        private AnimatorManager _animatorManager;
        private Transform   _aniTform;
        private Transform   _shotTform;

        private Material unitAgentMaterial = null;
        private Shader normalMaterial = null;
        private Shader attackShader = null;

        public GameObject AnchorRootGameObject
        {
            get
            {
                if (_gameObject == null)
                {
                    _gameObject = gameObject;
                }
                return _gameObject;
            }
        }
        public Transform AnchorRoot
        {
            get
            {
                if (_transform == null)
                {
                    _transform = transform;
                }
                return _transform;
            }
        }

        public  void getShaderMaterial()
        {
            if (attackShader == null)
            {
                attackShader = Shader.Find("Mobhero/RimLight");
            }
            if (normalMaterial == null)
            {
                //normalMaterial = Shader.Find("Custom/PlayerDiffuse");
                normalMaterial = Shader.Find("Unlit/Texture");
            }
            if (unitAgentMaterial == null)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = unitAgent.transform.GetComponentInChildren<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    unitAgentMaterial = unitAgent.transform.GetComponentInChildren<SkinnedMeshRenderer>().material;
                }
            }
        }
        public void getByAttackShader()
        {
            getShaderMaterial();
            if (unitAgentMaterial != null)
            {
                unitAgentMaterial.shader = attackShader;
            }
        }
        public void getNormalShader()
        {
            getShaderMaterial();
            if (unitAgentMaterial != null)
            {
                unitAgentMaterial.shader = normalMaterial;
            }
        }

        /** 单位数据 */
        public UnitAgent unitAgent
        {
            get
            {
                if (_unitAgent == null)
                {
                    _unitAgent = GetComponent<UnitAgent>();
                }
                return _unitAgent;
            }

            set
            {
                _unitAgent = value;
            }
        }


        /** 单位数据 */
        public UnitData unitData
        {
            get
            {
                if (_unitData == null)
                {
                    _unitAgent = GetComponent<UnitAgent>();
                    if (_unitAgent == null) return null;
                    _unitData = _unitAgent._unitData;
                }
                return _unitData;
            }

            set
            {
                _unitData = value;
            }
        }

        /** 单位属性 */
        public PropUnit prop
        {
            get
            {
                return unitData.prop;
            }
        }

        public AnimatorManager aniManager
        {
            get
            {
                if (_animatorManager == null)
                {
                    _animatorManager = unitAgent.animatorManager;
                }
                return _animatorManager;
            }
        }

        public  Transform   modelTform
        {
            get
            {
                if (_aniTform == null && aniManager != null)
                {
                    _aniTform = aniManager.transform;
                    if (aniManager.rotationNode != null)
                    {
                        _aniTform = aniManager.rotationNode;
                    }
                }
                return _aniTform;
            }
        }

        /// <summary>
        /// 攻击点
        /// </summary>
        public Transform shotTform
        {
            get
            {
                if (_shotTform == null)
                {
                    _shotTform = modelTform;
                    if (aniManager != null && aniManager.ShotPos != null)
                    {
                        _shotTform = aniManager.ShotPos;
                    }
                }
                return _shotTform;
            }
        }

        /** 势力数据 */
        public LegionData legionData
        {
            get
            {
                return sceneData.GetLegion(unitData.legionId);
            }
        }



        /** 单位--安装 */
        virtual public void OnUnitInstall()
        {

        }

        /** 单位--卸载 */
        virtual public void OnUnitUninstall()
        {
        }


        /** 初始化 */
        virtual public void Init(UnitData unitData)
        {
            this.unitData = unitData;
            this.room = unitData.room;
        }


        #region Unit Component

        /** 单位UID */
        public int uid
        {
            get
            {
                return unitData.uid;
            }
        }

        /** 单位类型 */
        public UnitType unitType
        {
            get
            {
                return unitData.unitType;
            }
        }
        /// <summary>
        /// 单位空间类型
        /// </summary>
        public UnitSpaceType unitSpaceType
        {
            get
            {
                return unitData.spaceType;
            }
        }

        /** 建筑类型 */
        public UnitBuildType buildType
        {
            get
            {
                return unitData.buildType;
            }
        }


        public Vector3 position
        {
            get
            {
                return transform.position;
            }
        }

        public Vector3 rotation
        {
            get
            {
                return unitAgent.rotation;
            }
        }


        #endregion
    }
}
