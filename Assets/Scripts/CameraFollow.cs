using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // プレイヤーの Transform を Inspector で設定
    public float smoothSpeed = 0.125f; // 追従のなめらかさ
    public Vector3 offset; // カメラとプレイヤーの距離（必要なら）

    void LateUpdate()
    {
        if (player != null)
        {
            // プレイヤーの位置にオフセットを足した位置へ補間して移動
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}

