using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SROptions
{
    public AppWeb web = new AppWeb();

    [Sort(2001)]
    [Category("Web")]
    [DisplayName("Start Web")]
    public void Web_Start()
    {
        web.Start();
    }

    [Sort(2002)]
    [Category("Web")]
    [DisplayName("Stop Web")]
    public void Web_Stop()
    {
        web.Stop();
    }


}