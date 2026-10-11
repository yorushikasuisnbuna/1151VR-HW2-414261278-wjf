using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                                  // 要跟隨的人物
    public Vector3 offset = new Vector3(0f, 2f, -10f);        // 相機相對人物的位置（Z 一定要是 -10）
    public float smooth = 5f;                                 // 跟隨的平滑程度

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smooth * Time.deltaTime);
    }
}
