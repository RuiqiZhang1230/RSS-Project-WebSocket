using UnityEngine;
using TMPro;
using System;

public class TemperatureDisplay : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    void Start()
    {
        // 获取 TextMeshPro 组件
        textMeshPro = GetComponent<TextMeshPro>();

        // 初始化显示
        UpdateTemperatureDisplay(20.0f); // 默认显示 20.0°C

        // 订阅 SensorDataManager 的 OnTemperatureUpdated 事件
        SensorDataManager.Instance.OnTemperatureUpdated += HandleTemperatureUpdated;
    }

    void OnDestroy()
    {
        // 取消订阅事件，防止内存泄漏
        if (SensorDataManager.Instance != null)
        {
            SensorDataManager.Instance.OnTemperatureUpdated -= HandleTemperatureUpdated;
        }
    }

    // 事件处理方法
    private void HandleTemperatureUpdated(float temperature)
    {
        UpdateTemperatureDisplay(temperature);
    }

    // 更新温度显示
    private void UpdateTemperatureDisplay(float temperature)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = temperature.ToString("F1") + " °C";
        }
    }
}
