using UnityEngine;

public class Door : MonoBehaviour
{
    public float interactionDistance = 3.0f; // 门交互的最大距离
    public GameObject intText; // 显示交互文本的 GameObject
    public string doorOpenAnimName = "DoorOpen"; // 开门动画名称
    public string doorCloseAnimName = "DoorClose"; // 关门动画名称
    public Transform player; // 玩家 Transform，用于检测距离

    private Animator doorAnim; // 动画组件
    private bool isOpen = false; // 门的开关状态

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        if (intText != null)
        {
            intText.SetActive(false); // 确保交互文本一开始隐藏
        }

        // 订阅 SensorDataManager 的 OnMotionDetectedUpdated 事件
        if (SensorDataManager.Instance != null)
        {
            SensorDataManager.Instance.OnMotionDetectedUpdated += HandleMotionDetected;
        }
        else
        {
            Debug.LogError("SensorDataManager is not initialized!");
        }
    }

    void OnDestroy()
    {
        // 取消订阅事件以避免内存泄漏
        if (SensorDataManager.Instance != null)
        {
            SensorDataManager.Instance.OnMotionDetectedUpdated -= HandleMotionDetected;
        }
    }

    // 处理红外传感器触发事件
    private void HandleMotionDetected(bool motionDetected)
    {
        if (motionDetected && IsPlayerWithinInteractionDistance())
        {
            ToggleDoor(); // 当检测到运动并且玩家在范围内时，触发开关门
        }
    }

    // 判断玩家是否在交互范围内
    private bool IsPlayerWithinInteractionDistance()
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned!");
            return false;
        }

        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= interactionDistance;
    }

    // 开关门的逻辑
    private void ToggleDoor()
    {
        if (isOpen)
        {
            doorAnim.ResetTrigger("open");
            doorAnim.SetTrigger("close");
        }
        else
        {
            doorAnim.ResetTrigger("close");
            doorAnim.SetTrigger("open");
        }

        isOpen = !isOpen; // 切换门的状态
    }
}
