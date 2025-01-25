using UnityEngine;
using System; // 为事件添加引用
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
            // 订阅 SensorDataManager 的事件
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
            // 取消订阅事件，防止内存泄漏
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

        // 根据加速度数据更新移动方向
        private void UpdateMove(Vector3 acceleration)
        {
            move = new Vector2(acceleration.x, acceleration.z); // 使用加速度 X 和 Z 分量作为移动方向
        }

        // 根据陀螺仪数据更新视角方向

        private float gyroSensitivity = 0.5f;

        private void UpdateLook(Vector3 gyro)
        {
            look.x = gyro.x * gyroSensitivity;
            look.y = gyro.y * gyroSensitivity; // 使用陀螺仪 X 和 Y 分量作为视角变化
        }

        // 根据旋转编码器按钮更新跳跃状态
        private void UpdateJump(bool buttonPressed)
        {
            jump = buttonPressed; // 按下时跳跃
        }

        // 根据旋转编码器值更新冲刺状态
        private void UpdateSprint(int encoder)
        {
            if (encoder == 0)
            {
                sprint = false; // 静止
            }
            else if (Mathf.Abs(encoder) > 5)
            {
                sprint = true; // 冲刺
            }
            else
            {
                sprint = false; // 正常速度
            }
        }
    }
}
