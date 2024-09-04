using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TTSToggler : MonoBehaviour
{
    [SerializeField] GameObject _receiverObject;
    [SerializeField] GameObject _senderObject;
    [SerializeField] GameObject parent;

    [SerializeField] TMP_InputField _chatField;
    [SerializeField] GameObject _scrollViewObj;
    [SerializeField] AudioSource _audioSource;

    //[SerializeField] TextMeshProUGUI _deviceStatus;
    [SerializeField] Image _deviceStatus;
    private string _deviceName = "dottalk";
    private bool _isBTConnected = false;
    private string _receivedData = "";
    private string _sendedData = "";

    [SerializeField] GameObject _settingWindow;
    [SerializeField] TMP_InputField _deviceNameField;
    [SerializeField] Toggle _toggleTTS;
    [SerializeField] TMP_InputField _ttsIterationField;
    private int _ttsIteration = 1;

    void Start()
    {
        CreateBTConnection();
    }

    void Update()
    {
        ReadBTData();
    }

    private bool _toggleSettingWindow = true;
    public void ShowSettingWindow()
    {
        if (!_toggleSettingWindow)
        {
            _settingWindow.SetActive(true);
        }
        else
        {
            _settingWindow.SetActive(false);
        }

        _toggleSettingWindow = !_toggleSettingWindow;
    }

    private void CreateBTConnection()
    {
        BluetoothService.CreateBluetoothObject();
    }

    private void StartBTConnection()
    {
        _isBTConnected = BluetoothService.StartBluetoothConnection(_deviceName);

        if (_isBTConnected)
        {
            SetDeviceName(_deviceName);
            SetDeviceStatus(true);
        }
    }

    private void StopBTConnection()
    {
        _isBTConnected = false;
        BluetoothService.StopBluetoothConnection();
        SetDeviceName(_deviceName);
        SetDeviceStatus(false);
    }

    private void ReadBTData()
    {
        if (_isBTConnected)
        {
            _receivedData += BluetoothService.ReadFromBluetooth();
            if (_receivedData.Contains("/"))
            {
                if (_receivedData.Length == 1 && _sendedData.Length > 0)
                {
                    SendBTData(_sendedData);
                    _receivedData = "";
                }
                else
                {
                    ReceiveData(_receivedData);
                    _receivedData = "";
                    _sendedData = "";
                }
            }
        }
    }

    private void SendBTData(string sendData)
    {
        if (_isBTConnected)
        {
            BluetoothService.WritetoBluetooth(sendData);
            _sendedData = sendData;
        }
    }

    public void SendData()
    {
        if (_chatField.text.Length > 0)
        {
            Text[] msg = _senderObject.GetComponentsInChildren<Text>();
            string sendData = _chatField.text + "/";
            SendBTData(sendData);

            msg[0].text = _chatField.text;
            msg[1].text = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
            Instantiate(_senderObject, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);

            ClearInputText();
        }
    }

    private void ReceiveData(string received)
    {
        Text[] msg = _receiverObject.GetComponentsInChildren<Text>();

        msg[0].text = received.Replace('/', ' ');
        msg[1].text = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
        Instantiate(_receiverObject, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
        AutoScrollDown();
    }

    private void ClearInputText()
    {
        _chatField.text = String.Empty;
        AutoScrollDown();
    }

    private void AutoScrollDown()
    {
        RectTransform tr = parent.GetComponent<RectTransform>();
        Vector2 newScale = tr.sizeDelta;
        newScale.y += 140;
        tr.sizeDelta = newScale;
        _scrollViewObj.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }

    public void OnSetTTSIterationNumber()
    {
        try
        {
            _ttsIteration = Math.Min(Math.Max(1, Convert.ToInt32(_ttsIterationField.text)), 3);
            _ttsIterationField.text = _ttsIteration.ToString();
        }
        catch (Exception e)
        {
            _ttsIteration = 1;
            _ttsIterationField.text = "1";
        }
    }

    public void OnSetDeviceName()
    {
        SetDeviceName(_deviceNameField.text);
        StopBTConnection();
        StartBTConnection();
    }

    private void SetDeviceName(string devName)
    {
        _deviceName = devName;
    }

    private void SetDeviceStatus(bool isConnected)
    {
        if (isConnected)
            _deviceStatus.color = Color.green;
        else
            _deviceStatus.color = Color.red;
    }
}
