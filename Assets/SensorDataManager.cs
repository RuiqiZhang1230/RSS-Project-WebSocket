using UnityEngine;
using System;

public class SensorDataManager : MonoBehaviour
{
    public static SensorDataManager Instance;

    // 传感器数据
    private int encoderCount;
    private bool buttonPressed;
    private Vector3 acceleration;
    private Vector3 gyro;
    private float temperature;
    private bool motionDetected;

    // 单独的数据更新事件
    public event Action<bool> OnMotionDetectedUpdated; // 红外传感器事件
    public event Action<float> OnTemperatureUpdated;   // 温度传感器事件
    public event Action<int> OnEncoderCountUpdated;    // 编码器事件
    public event Action<bool> OnButtonPressedUpdated;  // 按钮事件
    public event Action<Vector3> OnAccelerationUpdated; // 加速度事件
    public event Action<Vector3> OnGyroUpdated;        // 陀螺仪事件

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

    // 更新红外传感器数据
    public void UpdateMotionDetected(bool motion)
    {
        motionDetected = motion;
        OnMotionDetectedUpdated?.Invoke(motion);
    }

    // 更新温度数据
    public void UpdateTemperature(float temp)
    {
        temperature = temp;
        OnTemperatureUpdated?.Invoke(temp);
    }

    // 更新编码器数据
    public void UpdateEncoderCount(int count)
    {
        encoderCount = count;
        OnEncoderCountUpdated?.Invoke(count);
    }

    // 更新按钮数据
    public void UpdateButtonPressed(bool pressed)
    {
        buttonPressed = pressed;
        OnButtonPressedUpdated?.Invoke(pressed);
    }

    // 更新加速度数据
    public void UpdateAcceleration(Vector3 accel)
    {
        acceleration = accel;
        OnAccelerationUpdated?.Invoke(accel);
    }

    // 更新陀螺仪数据
    public void UpdateGyro(Vector3 gyroData)
    {
        gyro = gyroData;
        OnGyroUpdated?.Invoke(gyroData);
    }
}
