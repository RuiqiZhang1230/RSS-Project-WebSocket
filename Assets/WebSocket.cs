using System.Text;
using UnityEngine;
using NativeWebSocket;
using System;

public class ArduinoWebSocket : MonoBehaviour
{
    public string serverUri = "ws://192.168.4.1:81"; // Arduino WebSocket��ַ
    private WebSocket websocket;

    private void Start()
    {
        websocket = new WebSocket(serverUri);

        websocket.OnOpen += () =>
        {
            Debug.Log("WebSocket���ӳɹ���");
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError($"WebSocket����{e}");
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("WebSocket���ӹر�");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log($"���յ�����Ϣ��{message}");
            ProcessSensorData(message); // ������յ��Ĵ���������
        };

        ConnectWebSocket();
    }

    private async void ConnectWebSocket()
    {
        await websocket.Connect();
    }

    private void Update()
    {
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }

    private void ProcessSensorData(string json)
    {
        SensorData data = JsonUtility.FromJson<SensorData>(json);

        SensorDataManager.Instance.UpdateMotionDetected(data.motion_detected);
        SensorDataManager.Instance.UpdateTemperature(data.temperature);
        SensorDataManager.Instance.UpdateEncoderCount(data.encoder_count);
        SensorDataManager.Instance.UpdateButtonPressed(data.button_pressed);
        SensorDataManager.Instance.UpdateAcceleration(
            new Vector3(data.acceleration.x, data.acceleration.y, data.acceleration.z)
        );
        SensorDataManager.Instance.UpdateGyro(
            new Vector3(data.gyro.x, data.gyro.y, data.gyro.z)
        );

        // ������Ϣ
        Debug.Log($"Acceleration: X={data.acceleration.x}, Y={data.acceleration.y}, Z={data.acceleration.z} | " +
                  $"Gyro: X={data.gyro.x}, Y={data.gyro.y}, Z={data.gyro.z} | " +
                  $"Button Pressed: {data.button_pressed}|" + $"Encoder Count: {data.encoder_count}");


    }

    [Serializable]
    private class SensorData
    {
        public int encoder_count;
        public bool button_pressed;
        public Acceleration acceleration;
        public Gyro gyro;
        public float temperature;
        public bool motion_detected;

        [Serializable]
        public class Acceleration
        {
            public float x, y, z;
        }

        [Serializable]
        public class Gyro
        {
            public float x, y, z;
        }
    }
}
