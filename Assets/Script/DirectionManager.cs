using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;

public class DirectionManager : MonoBehaviour
{
    // GPS 정보를 받아 타겟 목표점과 현 위치 간 각도를 계산해 출력한 뒤, 라즈베리파이로 보내는 코드
    
    public Transform currentPosition; // 현재 지점의 Transform (현 위치를 나타내는 객체)

    // 목표 지점의 GPS 좌표 (위도, 경도)
    public double targetLatitude;
    public double targetLongitude;

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
        // 현재 위치의 GPS 좌표를 가정 (이 예제에서는 임의로 설정, 실제로는 GPS 데이터에서 받아와야 함)
        double currentLatitude = 37.7749; // 현재 위치의 위도
        double currentLongitude = -122.4194; // 현재 위치의 경도

        // 목표 지점의 GPS 좌표와 현재 위치의 GPS 좌표를 사용하여 방향 각도 계산
        float angle = CalculateAngle(currentLatitude, currentLongitude, targetLatitude, targetLongitude);

        // 각도 값 전송
        string message = angle.ToString();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        Debug.Log("Direction angle: " + angle + " degrees");
    }

    // 두 GPS 좌표(위도, 경도) 사이의 방향 각도를 계산하는 함수
    float CalculateAngle(double lat1, double lon1, double lat2, double lon2)
    {
        double dLon = lon2 - lon1;
        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
        double brng = Math.Atan2(y, x);
        brng = (brng * 180.0 / Math.PI + 360) % 360; // 라디안 값을 각도로 변환

        return (float)brng;
    }

    void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
    }
}
