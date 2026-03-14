using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHover : MonoBehaviour
{
    [Header("参照")]
    public PlayerMove playerMove;
    public CharacterController controller;

    public GameObject playerGuard;

    [Header("ホバリング設定")]
    public float gravityLoss = 0.2f;
    public float initialUpForce = 1.0f; // ホバリング開始時の初期上昇力
    public float maxUpForce = 5.0f;     // 上昇力の最大値
    public float upForceIncreaseRate = 1.0f; // 1秒あたりの上昇力増加量
    public float hoverTime = 1.5f;

    private bool isHovering = false;
    private float currentHoverTime = 0f;
    private bool jumpButtonHeld = false; // ジャンプボタンが押され続けているか

    void Awake()
    {
        if (playerMove == null)
        {
            playerMove = GetComponent<PlayerMove>();
        }
    }

    void OnJump(InputValue value)
    {
        jumpButtonHeld = value.isPressed;
    }
    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            if (controller.isGrounded)
            {
                SphereCollider[] cols = GetComponents<SphereCollider>();
                foreach(SphereCollider sc in cols)
                {
                    sc.enabled = false;
                }
                playerGuard.SetActive(true);
            }

        }
        else
        {
            SphereCollider[] cols = GetComponents<SphereCollider>();
            foreach (SphereCollider sc in cols)
            {
                sc.enabled = true;
            }
            playerGuard.SetActive(false);

        }
    }

    void Update()
    {
        if (playerMove == null || playerMove.MoveDirection == null) return;


        //Debug.Log("isPressed:" + jumpButtonHeld);
        // Spaceキーの押下状態を直接取得
        //if (Keyboard.current != null)
        //{
        //    bool isSpacePressed = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
        //    bool isGamepadPressed = Gamepad.current != null && Gamepad.current.buttonSouth.isPressed;

        //    jumpButtonHeld = isSpacePressed || isGamepadPressed;
        //}
        //}

        // Debug.Log(jumpButtonHeld); // デバッグ用

        // 地面にいる場合はホバリング状態をリセット
        if (playerMove.GetComponent<CharacterController>().isGrounded)
        {
            isHovering = false;
            currentHoverTime = 0f;
        }

        if (jumpButtonHeld && !playerMove.GetComponent<CharacterController>().isGrounded && currentHoverTime < hoverTime /* && playerMove.MoveDirection.y < 0 */)
        {
            isHovering = true;
            currentHoverTime += Time.deltaTime;

            // 経過時間に応じてupForceを計算
            float currentUpForce = initialUpForce + (upForceIncreaseRate * currentHoverTime);
            currentUpForce = Mathf.Min(currentUpForce, maxUpForce); // 最大値でクランプ

            // ホバリング中は落下速度を軽減し、わずかに上昇力を加える
            Vector3 currentMoveDirection = playerMove.MoveDirection;

            float effectiveGravity = playerMove.gravity * (1f - gravityLoss);
            currentMoveDirection.y -= effectiveGravity * Time.deltaTime; // 重力軽減

            currentMoveDirection.y += currentUpForce * Time.deltaTime; // 計算した上昇力を適用

            playerMove.SetMoveDirectionY(currentMoveDirection.y); // PlayerMoveのmoveDirectionを上書き
        }
        else
        {
            isHovering = false;
        }
    }

    public bool IsHovering
    {
        get { return isHovering; }
    }
}