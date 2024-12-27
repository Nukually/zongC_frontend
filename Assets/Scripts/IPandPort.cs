using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPandPort : MonoBehaviour
{
    // Start is called before the first frame update
    public void UpdateIP(string ip)
    {
        TcpClientScript.ServerIP = ip;
    }

    public void UpdatePort(string port)
    {
        TcpClientScript.ServerPort = int.Parse(port);
    }
}
