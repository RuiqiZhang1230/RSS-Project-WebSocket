using UnityEngine;
using System;

public class SensorDataManager : MonoBehaviour
{
    public static SensorDataManager Instance;

    // ����������
    private int encoderCount;
    private bool buttonPressed;
    private Vector3 acceleration;
    private Vector3 gyro;
    private float temperature;
    private bool motionDetected;

    // ���������ݸ����¼�
    public event Action<bool> OnMotionDetectedUpdated; // ���⴫�����¼�
    public event Action<float> OnTemperatureUpdated;   // �¶ȴ������¼�
    public event Action<int> OnEncoderCountUpdated;    // �������¼�
    public event Action<bool> OnButtonPressedUpdated;  // ��ť�¼�
    public event Action<Vector3> OnAccelerationUpdated; // ���ٶ��¼�
    public event Action<Vector3> OnGyroUpdated;        // �������¼�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���º��⴫��������
    public void UpdateMotionDetected(bool motion)
    {
        motionDetected = motion;
        OnMotionDetectedUpdated?.Invoke(motion);
    }

    // �����¶�����
    public void UpdateTemperature(float temp)
    {
        temperature = temp;
        OnTemperatureUpdated?.Invoke(temp);
    }

    // ���±���������
    public void UpdateEncoderCount(int count)
    {
        encoderCount = count;
        OnEncoderCountUpdated?.Invoke(count);
    }

    // ���°�ť����
    public void UpdateButtonPressed(bool pressed)
    {
        buttonPressed = pressed;
        OnButtonPressedUpdated?.Invoke(pressed);
    }

    // ���¼��ٶ�����
    public void UpdateAcceleration(Vector3 accel)
    {
        acceleration = accel;
        OnAccelerationUpdated?.Invoke(accel);
    }

    // ��������������
    public void UpdateGyro(Vector3 gyroData)
    {
        gyro = gyroData;
        OnGyroUpdated?.Invoke(gyroData);
    }
}
