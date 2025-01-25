using UnityEngine;

public class Door : MonoBehaviour
{
    public float interactionDistance = 3.0f; // �Ž�����������
    public GameObject intText; // ��ʾ�����ı��� GameObject
    public string doorOpenAnimName = "DoorOpen"; // ���Ŷ�������
    public string doorCloseAnimName = "DoorClose"; // ���Ŷ�������
    public Transform player; // ��� Transform�����ڼ�����

    private Animator doorAnim; // �������
    private bool isOpen = false; // �ŵĿ���״̬

    void Start()
    {
        doorAnim = GetComponent<Animator>();
        if (intText != null)
        {
            intText.SetActive(false); // ȷ�������ı�һ��ʼ����
        }

        // ���� SensorDataManager �� OnMotionDetectedUpdated �¼�
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
        // ȡ�������¼��Ա����ڴ�й©
        if (SensorDataManager.Instance != null)
        {
            SensorDataManager.Instance.OnMotionDetectedUpdated -= HandleMotionDetected;
        }
    }

    // ������⴫���������¼�
    private void HandleMotionDetected(bool motionDetected)
    {
        if (motionDetected && IsPlayerWithinInteractionDistance())
        {
            ToggleDoor(); // ����⵽�˶���������ڷ�Χ��ʱ������������
        }
    }

    // �ж�����Ƿ��ڽ�����Χ��
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

    // �����ŵ��߼�
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

        isOpen = !isOpen; // �л��ŵ�״̬
    }
}
