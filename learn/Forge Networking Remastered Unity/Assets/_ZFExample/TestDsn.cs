using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TestDsn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string server = "ihaiu.com";

    [ContextMenu("Test")]
    public void Test()
    {

        // Get DNS host information.
        IPHostEntry hostInfo = Dns.GetHostEntry(server);
        // Get the DNS IP addresses associated with the host.
        IPAddress[] IPaddresses = hostInfo.AddressList;
        for(int i = 0; i < IPaddresses.Length; i ++)
        {
            Debug.LogFormat("{0}    {1} {2} {3}", i, IPaddresses[i].ToString(), IPaddresses[i].AddressFamily, IPaddresses[i].Address);
        }
    }


    [ContextMenu("TestDns")]
    public void TestDns()
    {
        Debug.Log("Dns.GetHostName()=" + Dns.GetHostName());
        IPHostEntry hostCheck = Dns.GetHostEntry(Dns.GetHostName());
        int i = 0;
        foreach (IPAddress ip in hostCheck.AddressList)
        {
            Debug.LogFormat("{0}    {1}     {2}", i, ip, ip.AddressFamily);
            i++;
        }
    }

    [ContextMenu("TestIP")]
    public void TestIP()
    {
        Debug.LogFormat("{0}    {1}    {2} ", "Any", IPAddress.Any, IPAddress.Any.AddressFamily);
        Debug.LogFormat("{0}    {1}    {2} ", "Broadcast", IPAddress.Broadcast, IPAddress.Broadcast.AddressFamily);
        Debug.LogFormat("{0}    {1}    {2} ", "IPv6Any", IPAddress.IPv6Any, IPAddress.IPv6Any.AddressFamily);
        Debug.LogFormat("{0}    {1}    {2} ", "IPv6Loopback", IPAddress.IPv6Loopback, IPAddress.IPv6Loopback.AddressFamily);
        Debug.LogFormat("{0}    {1}    {2} ", "IPv6None", IPAddress.IPv6None, IPAddress.IPv6None.AddressFamily);
        Debug.LogFormat("{0}    {1}    {2} ", "Loopback", IPAddress.Loopback, IPAddress.Loopback.AddressFamily);
        Debug.LogFormat("{0}    {1}    {2} ", "None", IPAddress.None, IPAddress.None.AddressFamily);
    }


    [ContextMenu("TestGUID")]
    public void TestGuid()
    {
        System.Guid guid = new System.Guid();
        Debug.Log(guid);
        guid = System.Guid.NewGuid();
        Debug.Log(guid);
        guid = System.Guid.NewGuid();
        Debug.Log(guid);
    }
}
