using BeardedManStudios.Forge.Logging;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 9:19:35 AM
*  @Description:    
* ==============================================================================
*/
using BeardedManStudios.Threading;
using System;
using System.Threading;

namespace ihaiu
{
    public class HTcpClient : HTCPClientBase
    {

        protected override void Initialize(string host, ushort port)
        {
            base.Initialize(host, port);
            InitializeTCPClient(host, port);
        }

        protected void InitializeTCPClient(string host, ushort port)
        {
            //Set the port
            SetPort(port);

            // Startup the read thread and have it listening for any data from the server
            Task.Queue(ReadAsync);
        }

        private void ReadAsync()
        {
            try
            {
                while (IsBound)
                {
                    switch (Read())
                    {
                        case ReadState.Void:
                            break;
                        case ReadState.Continue:
                            Thread.Sleep(10);
                            break;
                        case ReadState.Disconnect:
                            return;
                    }
                }
            }
            catch (Exception e)
            {
                // There was in issue reading or executing from the network so disconnect
                // TODO:  Add more logging here and an exception with an inner exception being
                // the exception that was thrown
                BMSLog.LogException(e);
                //Console.WriteLine("CRASH: " + e.Message);
                Disconnect(true);
            }
        }
    }
}