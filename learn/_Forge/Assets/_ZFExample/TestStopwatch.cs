using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestStopwatch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	

	void Update ()
    {
		if(isUpdatePrint)
        {
            UnityEngine.Debug.Log("ElapsedTicks=" + sw.ElapsedTicks + "   ElapsedMilliseconds=" + sw.ElapsedMilliseconds + "   frameCount=" + Time.frameCount + "   unscaledTime=" + Time.unscaledTime);
        }
	}

    public Stopwatch sw = new Stopwatch();
    public bool isUpdatePrint = false;
    private void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 80, 50), "Start"))
        {
            sw.Start();
        }


        if (GUI.Button(new Rect(10, 100, 80, 50), "Stop"))
        {
            sw.Stop();
        }


        if (GUI.Button(new Rect(10, 200, 80, 50), "Reset"))
        {
            sw.Reset() ;
        }


        if (GUI.Button(new Rect(10, 300, 80, 50), "Print"))
        {
            UnityEngine.Debug.Log("ElapsedTicks=" + sw.ElapsedTicks + "   ElapsedMilliseconds=" + sw.ElapsedMilliseconds + "   frameCount=" + Time.frameCount + "   unscaledTime=" + Time.unscaledTime);
        }

        isUpdatePrint = GUI.Toggle(new Rect(100, 10, 200, 50), isUpdatePrint, "Update Print");
    }

    [ContextMenu("Info")]
    public void Info()
    {
        UnityEngine.Debug.Log("Stopwatch.IsHighResolution=" + Stopwatch.IsHighResolution);
        UnityEngine.Debug.Log("Stopwatch.Frequency=" + Stopwatch.Frequency);
    }


}
