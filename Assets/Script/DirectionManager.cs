using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;

public class DirectionManager : MonoBehaviour
{
    // GPS ������ �޾� Ÿ�� ��ǥ���� �� ��ġ �� ������ ����� ����� ��, ��������̷� ������ �ڵ�
    
    public Transform currentPosition; // ���� ������ Transform (�� ��ġ�� ��Ÿ���� ��ü)

    // ��ǥ ������ GPS ��ǥ (����, �浵)
    public double targetLatitude;
    public double targetLongitude;

    public string raspberryPiIP = ""; // ����� ������ IP �ּ�
    public int port = 5000; // ����� ���̿��� ������ ��Ʈ ��ȣ

    private TcpClient client;
    private NetworkStream stream;

    void Start()
    {
        client = new TcpClient(raspberryPiIP, port);
        stream = client.GetStream();
    }

    void Update()
    {
        // ���� ��ġ�� GPS ��ǥ�� ���� (�� ���������� ���Ƿ� ����, �����δ� GPS �����Ϳ��� �޾ƿ;� ��)
        double currentLatitude = 37.7749; // ���� ��ġ�� ����
        double currentLongitude = -122.4194; // ���� ��ġ�� �浵

        // ��ǥ ������ GPS ��ǥ�� ���� ��ġ�� GPS ��ǥ�� ����Ͽ� ���� ���� ���
        float angle = CalculateAngle(currentLatitude, currentLongitude, targetLatitude, targetLongitude);

        // ���� �� ����
        string message = angle.ToString();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        Debug.Log("Direction angle: " + angle + " degrees");
    }

    // �� GPS ��ǥ(����, �浵) ������ ���� ������ ����ϴ� �Լ�
    float CalculateAngle(double lat1, double lon1, double lat2, double lon2)
    {
        double dLon = lon2 - lon1;
        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
        double brng = Math.Atan2(y, x);
        brng = (brng * 180.0 / Math.PI + 360) % 360; // ���� ���� ������ ��ȯ

        return (float)brng;
    }

    void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
    }
}
