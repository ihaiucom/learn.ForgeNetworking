using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/27/2017 11:24:24 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 建筑格子视图
    /// </summary>
    public class BuildCellAgent : AbstractRoomMonoBehaviour
    {
        /** 建筑格子 */
        public BuildCellData buildCellData;

        /** 建筑格子UID */
        public int uid
        {
            get
            {
                return buildCellData.uid;
            }
        }

        [HideInInspector]
        public Vector3  position;
        [HideInInspector]
        public bool     playerIsMove;


        public MeshRenderer meshRenderer;
        public BoxCollider  collider;
        //public  Rigidbody   rigidbody;

        public Material defaultMaterial;
        public Material hightlightMaterialSource;
        public Material selectModelMaterialSource;

        public static Material hightlightMaterial;
        public static Material selectModelMaterial;

        private Color defaultColor;
        public Color hightlightColor;

        public float hightlightTime = 3;
        public float hightlightHideTime = 1;
        public float recoveryTime = 2;

        public bool isHighlight = false;
        public bool isUsed = false;
        public bool isSelectModel = false;
        public bool isRecoverying = false;
        public bool isWaitRestLive = false;

        void Awake()
        {
            defaultColor = defaultMaterial.GetColor("_TintColor");
            if (hightlightMaterial == null)
            {
                hightlightMaterialSource.hideFlags = HideFlags.None;
                hightlightMaterial = new Material(hightlightMaterialSource);
                hightlightMaterial.hideFlags = HideFlags.DontSaveInEditor;


                selectModelMaterial = new Material(selectModelMaterialSource);
                selectModelMaterial.hideFlags = HideFlags.DontSaveInEditor;
            }

            //if (collider == null)
            //    collider = GetComponent<BoxCollider>();
            //if (rigidbody == null)
            //{
            //    collider.enabled = true;
            //    rigidbody = GetComponent<Rigidbody>();
            //    if (rigidbody == null)
            //    {
            //        rigidbody = gameObject.AddComponent<Rigidbody>();
            //    }
            //}

            if (meshRenderer == null)
                meshRenderer = GetComponentInChildren<MeshRenderer>();

        }

        void Start()
        {
            //if (rigidbody != null)
            //{
            //    Destroy(rigidbody);
            //}
            if (collider == null)
                collider = GetComponent<BoxCollider>();
            collider.enabled = true;
            if (transform.localPosition.y > 10)
            {
                collider.center = new Vector3(0, 1, 0);
            }
        }

        [ContextMenu("Test SetDefault")]
        public void SetDefault()
        {
            isHighlight = false;
            StopColorChange();
            if (isUsed) return;

            meshRenderer.sharedMaterial = defaultMaterial;
            //if (collider.enabled)
            //{
            //    collider.enabled = false;
            //}
            if (!meshRenderer.enabled)
            {
                Enable();
            }


        }


        [ContextMenu("Test SetHighlight")]
        public void SetHighlight()
        {
            if (room.stageType == StageType.PVPLadder) return;

            isHighlight = true;
            if (isUsed) return;

            hightlightMaterial.SetColor("_TintColor", hightlightColor);
            meshRenderer.material = hightlightMaterial;
            //if (collider.enabled)
            //{
            //    collider.enabled = false;
            //}
            if (!meshRenderer.enabled)
            {
                Enable();
            }



            StopColorChange();
            corutine = StartCoroutine(ColorHeightToDefault());

        }

        Coroutine corutine;
        void StopColorChange()
        {
            if (corutine != null)
            {
                StopCoroutine(corutine);
                corutine = null;
                meshRenderer.sharedMaterial = defaultMaterial;
            }
        }

        IEnumerator ColorHeightToDefault()
        {
            yield return new WaitForSeconds(hightlightTime);

            float t = 0;
            while (t < hightlightHideTime)
            {
                if (playerIsMove)
                {
                    t = 0;
                    hightlightMaterial.SetColor("_TintColor", hightlightColor);
                    yield return null;
                }

                t += UnityEngine.Time.deltaTime;

                hightlightMaterial.SetColor("_TintColor", Color.Lerp(hightlightColor, defaultColor, t / hightlightHideTime));
                meshRenderer.material = hightlightMaterial;
                yield return null;
            }

            SetDefault();

        }

        [ContextMenu("Test SetUsed")]
        public void SetUsed()
        {
            isUsed = true;
            StopColorChange();
            //if (isSelectModel && !collider.enabled)
            //{
            //    collider.enabled = true;
            //}
            Disable();
        }



        [ContextMenu("Test SetUnUsed")]
        public void SetUnUsed()
        {
            isUsed = false;
            //if (!isWaitRestLive && collider.enabled)
            //{
            //    collider.enabled = false;
            //}
            Enable();
        }



        [ContextMenu("Test SetSelectModel")]
        public void SetSelectModel()
        {
            if (isSelectModel) return;

            //if (!collider.enabled)
            //{
            //    collider.enabled = true;
            //}
            isSelectModel = true;
            if (isUsed)
            {
                StopColorChange();
                meshRenderer.sharedMaterial = selectModelMaterial;
                meshRenderer.enabled = true;
            }
        }


        [ContextMenu("Test SetUnSelectModel")]
        public void SetUnSelectModel()
        {
            if (!isSelectModel) return;

            isSelectModel = false;
            //if (!isWaitRestLive && collider.enabled)
            //{
            //    collider.enabled = false;
            //}
            if (isUsed)
            {
                Disable();
            }
            meshRenderer.sharedMaterial = defaultMaterial;

            //collider.enabled = false;
        }

        public void SetWaitReLife(bool _true = true)
        {
            isWaitRestLive = _true;
            //collider.enabled = _true;
        }

        public void SetTowerDestory()
        {
            StartCoroutine(DoTowerDestory());
        }

        IEnumerator DoTowerDestory()
        {
            float t = 0;
            isRecoverying = true;
            meshRenderer.enabled = false;


            GameObject prefab = (GameObject)Resources.Load("Prefabs/ES/CT3001", typeof(GameObject));

            GameObject effect = null;
            if (prefab != null)
            {
                effect = (GameObject)GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
            }

            if (effect != null)
            {
                effect.transform.SetParent(transform);
                effect.transform.localPosition = Vector3.zero;
                effect.SetActive(true);
            }

            yield return new WaitForSeconds(recoveryTime);

            //if (tower != null && !tower.IsDied())
            //{
            //    UIOverlay.TestMove(tower.transform.position, tower.GetRecoveryPower());
            //    tower.SetDie();
            //}

            if (effect != null)
            {
                effect.SetActive(false);
                GameObject.DestroyImmediate(effect);
                effect = null;
            }
            isUsed = false;
            isRecoverying = false;
            //BuildPoint.Instance.PecyclingPoint(index);

            SetDefault();
            Enable();
        }


        public void Disable()
        {
            meshRenderer.enabled = false;
            StopColorChange();
            StopAllCoroutines();
        }

        public void Enable()
        {
            meshRenderer.enabled = true;
            isHighlight = false;
            isUsed = false;
            SetDefault();
        }




    }
}
