using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class DirectionManager : MonoBehaviour
{
    public Transform currentPosition; // 현재 지점
    public Transform targetPosition; // 목표 지점

    public string raspberryPiIP = ""; // 라즈베리 파이의 IP 주소
    public int port = 5000; // 라즈베리 파이에서 수신할 포트 번호

    private TcpClient client;
    private NetworkStream stream;

    void Start()
    {
        client = new TcpClient(raspberryPiIP, port);
        stream = client.GetStream();
    }

    void Update()
    {
        Vector3 direction = targetPosition.position - currentPosition.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;

        // 각도 값 전송
        string message = angle.ToString();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        Debug.Log("Direction angle: " + angle + " degrees");
    }

    void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
    }
}
