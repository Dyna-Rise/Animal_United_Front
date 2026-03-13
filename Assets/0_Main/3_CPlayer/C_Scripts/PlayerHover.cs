using UnityEngine;
using UnityEngine.InputSystem; // InputSystemを使用するために追加

public class PlayerHover : MonoBehaviour
{
    [Header("参照")]
    public PlayerMove playerMove; // PlayerMoveスクリプトへの参照

    [Header("ホバリング設定")]
    public float gravityLoss = 0.2f; // ホバリング中の重力の影響を軽減する倍率 (0.5なら半減)
    public float upForce = 2.0f; // ホバリング中に発生させるわずかな上昇力
    public float hoverTime = 1.5f; // ホバリングできる最大時間

    private bool isHovering = false;
    private float currentHoverTime = 0f;
    private bool jumpButtonPressed = false; // ジャンプボタンが押されているか

    void Awake()
    {
        // PlayerMoveスクリプトが同じGameObjectにあるか、Inspectorで設定されているかを確認
        if (playerMove == null)
        {
            playerMove = GetComponent<PlayerMove>();
        }
    }

    // ジャンプボタンの入力処理 (PlayerMoveとは独立して監視)
    void OnJump(InputValue value)
    {
        jumpButtonPressed = value.isPressed;
        // Debug.Log("Jump button pressed: " + jumpButtonPressed);
    }

    void Update()
    {
        // PlayerMoveのcontrollerがnullでないことを確認
        if (playerMove == null || playerMove.MoveDirection == null) return;

        // 地面にいる場合はホバリング状態をリセット
        if (playerMove.MoveDirection.y == 0 && playerMove.GetComponent<CharacterController>().isGrounded)
        {
            isHovering = false;
            currentHoverTime = 0f;
        }

        // ジャンプ中で、ボタンが押されており、かつホバリング時間が残っている場合
        if (jumpButtonPressed && playerMove.MoveDirection.y < 0 && !playerMove.GetComponent<CharacterController>().isGrounded && currentHoverTime < hoverTime)
        {
            isHovering = true;
            currentHoverTime += Time.deltaTime;

            // ホバリング中は落下速度を軽減し、わずかに上昇力を加える
            Vector3 currentMoveDirection = playerMove.MoveDirection;
            currentMoveDirection.y += (playerMove.gravity * (1 - gravityLoss) * Time.deltaTime); // 重力軽減
            currentMoveDirection.y += upForce * Time.deltaTime; // 上昇力

            // 下方向への速度が0以下にならないようにする（無限上昇を防ぐため）
            if (currentMoveDirection.y > 0 && currentMoveDirection.y < upForce * Time.deltaTime)
            {
                currentMoveDirection.y = upForce * Time.deltaTime;
            }

            playerMove.SetMoveDirectionY(currentMoveDirection.y); // PlayerMoveのmoveDirectionを上書き
        }
        else
        {
            isHovering = false;
        }
    }

    // ホバリング中かどうかを外部から取得するためのプロパティ
    public bool IsHovering
    {
        get { return isHovering; }
    }
}