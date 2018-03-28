using Rooms.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour {

    public IRoleInfo roleInfo;
    public Text idText;
    public Text nameText;



    public void SetData(IRoleInfo roleInfo)
    {
        this.roleInfo = roleInfo;
        idText.text = roleInfo.uid + "";
        nameText.text = roleInfo.name;
    }

    public float Y
    {
        get
        {
            return ((RectTransform)transform).anchoredPosition.y;
        }

        set
        {
            ((RectTransform)transform).anchoredPosition = new Vector2(0, value);
        }
    }

}
