using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TcpClientScript : MonoBehaviour
{
    public static string ServerIP = "127.0.0.1";
    public static int ServerPort = 12345;
    public GameObject highlightPrefab;
    private GameObject instantiatedImage;
    private TcpClient client;
    private NetworkStream stream;
    private CancellationTokenSource cancellationTokenSource;
    private bool isConnected = false;
    private byte[] buffer = new byte[1024];
    private int[] randomNumbers = new int[11];
    private bool isInit;
    private void HighlightTile(int x, int y)
    {
        if (isInit)
        {
            Destroy(instantiatedImage);
            isInit = false;
        }
        // 计算该瓦片的世界坐标
        Vector3 tileWorldPosition = new Vector3(x , y , 0f);
        // 创建一个高亮物体并放置在该位置
        instantiatedImage = Instantiate(highlightPrefab, tileWorldPosition, Quaternion.identity);
        isInit = true;
    }

    private void Awake()
    {
        cancellationTokenSource = new CancellationTokenSource();
        ConnectToServerAsync(cancellationTokenSource.Token);
        Debug.Log("TCP Awake");
    }

    public void BackToMenu()
    {
        if (SceneLoadManager.CallerType == 2)
        {
            SendMessageToServer("no");
        }
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        Debug.Log("Disconnect 返回菜单");
    }
    
    private async void ConnectToServerAsync(CancellationToken token)
    {
        try
        {
            client = new TcpClient();
            await client.ConnectAsync(ServerIP, ServerPort);
            if (client.Connected)
            {
                stream = client.GetStream();
                isConnected = true;
                Debug.LogWarning("连接成功");
                SendMessageToServer(SceneLoadManager.CallerType.ToString());
                _ = ReceiveDataAsync(token);
                
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"连接服务器失败: {e.Message}");
        }
    }

    public void MySendMessage(string message)
    {
        SendMessageToServer(message);//这一步给服务器的是一个坐标
        string[] parts = message.Split(' '); // 使用空格分割字符串
        int number1 = 0;
        int number2 = 0;
        // 解析成两个整数
        if (parts.Length == 2)
        {
            number1 = int.Parse(parts[0]); // 将第一个部分转为int
            number2 = int.Parse(parts[1]); // 将第二个部分转为int

            Debug.Log($"Number 1: {number1}, Number 2: {number2}");
        }
        else
        {
            Debug.LogError("输入的字符串格式不正确！");
        }
        //高亮 number1 number2
        HighlightTile(number1,number2);
    }
    public async void SendMessageToServer(string message)
    {
        if (isConnected && stream != null)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Debug.LogError("发送数据失败: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("尚未连接到服务器，无法发送消息");
        }
    }

    private async Task ReceiveDataAsync(CancellationToken token)
    {
        try
        {
            while (isConnected && !token.IsCancellationRequested)
            {
                int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                if (byteCount == 0)
                {
                    Debug.LogWarning("服务器关闭了连接");
                    Disconnect();
                    break;
                }

                string receivedData = Encoding.UTF8.GetString(buffer, 0, byteCount);
                Debug.Log("收到来自服务器的数据: " + receivedData);
                
                // 处理随机数消息
                if (receivedData.StartsWith("position:"))
                {
                    string[] parts = receivedData.Split(':')[1].Split(',');
                    for (int i = 0; i < 11; i++)
                    {
                        randomNumbers[i] = int.Parse(parts[i]);
                    }
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    PlayerController playerAndroid = players[0].GetComponent<PlayerController>();
                    playerAndroid.UpdateRandomNumbersWithPose(randomNumbers[0],randomNumbers[1],randomNumbers[2]);
                    int u = 3;
                    for (int j = 1; j < players.Length; j++)
                    {
                        PlayerController playerController = players[j].GetComponent<PlayerController>();
                        playerController.UpdateRandomNumbers(randomNumbers[u],randomNumbers[u+1]);
                        u += 2;
                    }
                }
            }
        }
        catch (OperationCanceledException) { }
    }
    
    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }

}

