using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SROptions
{

    [Sort(1001)]
    [Category("场景")]
    [DisplayName("MultiplayerMenu")]
    public void Scene_MultiplayerMenu()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }


    [Sort(1002)]
    [Category("场景")]
    [DisplayName("ServerBrowser")]
    public void Scene_ServerBrowser()
    {
        SceneManager.LoadScene("ServerBrowser");
    }

}