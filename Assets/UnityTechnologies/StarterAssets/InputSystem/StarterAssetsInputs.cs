using UnityEngine;
using System; // Ϊ�¼��������
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private void Start()
        {
            // ���� SensorDataManager ���¼�
            if (SensorDataManager.Instance != null)
            {
                SensorDataManager.Instance.OnAccelerationUpdated += UpdateMove;
                SensorDataManager.Instance.OnGyroUpdated += UpdateLook;
                SensorDataManager.Instance.OnButtonPressedUpdated += UpdateJump;
                SensorDataManager.Instance.OnEncoderCountUpdated += UpdateSprint;
            }
            else
            {
                Debug.LogError("SensorDataManager is not initialized!");
            }
        }

        private void OnDestroy()
        {
            // ȡ�������¼�����ֹ�ڴ�й©
            if (SensorDataManager.Instance != null)
            {
                SensorDataManager.Instance.OnAccelerationUpdated -= UpdateMove;
                SensorDataManager.Instance.OnGyroUpdated -= UpdateLook;
                SensorDataManager.Instance.OnButtonPressedUpdated -= UpdateJump;
                SensorDataManager.Instance.OnEncoderCountUpdated -= UpdateSprint;
            }
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        // ���ݼ��ٶ����ݸ����ƶ�����
        private void UpdateMove(Vector3 acceleration)
        {
            move = new Vector2(acceleration.x, acceleration.z); // ʹ�ü��ٶ� X �� Z ������Ϊ�ƶ�����
        }

        // �������������ݸ����ӽǷ���

        private float gyroSensitivity = 0.5f;

        private void UpdateLook(Vector3 gyro)
        {
            look.x = gyro.x * gyroSensitivity;
            look.y = gyro.y * gyroSensitivity; // ʹ�������� X �� Y ������Ϊ�ӽǱ仯
        }

        // ������ת��������ť������Ծ״̬
        private void UpdateJump(bool buttonPressed)
        {
            jump = buttonPressed; // ����ʱ��Ծ
        }

        // ������ת������ֵ���³��״̬
        private void UpdateSprint(int encoder)
        {
            if (encoder == 0)
            {
                sprint = false; // ��ֹ
            }
            else if (Mathf.Abs(encoder) > 5)
            {
                sprint = true; // ���
            }
            else
            {
                sprint = false; // �����ٶ�
            }
        }
    }
}
