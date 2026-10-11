using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // 走路速度
    public float jumpForce = 10f;       // 跳躍力道
    public Transform[] checkpoints;     // 陣列：檢查點，最後一個放 Goal

    private float reachDistance = 4f;   // 離檢查點多近算到達
    private Rigidbody2D rb;
    private Vector2 moveDir;            // vector：移動方向
    private bool isGrounded = false;
    private int nextIndex = 0;
    private bool finished = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (finished) return;
        // 往前走 / 返回
        float x = Input.GetAxisRaw("Horizontal");
        moveDir = new Vector2(x, 0f);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        // 身體翻轉（圖片預設朝右）
        if (x != 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (x > 0 ? 1f : -1f);
            transform.localScale = scale;
        }
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("按了空白鍵, grounded = " + isGrounded);
            // 跳躍
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        CheckCheckpoints();
    }


    void CheckCheckpoints()
    {
        if (checkpoints == null || nextIndex >= checkpoints.Length) return;

        float dist = Vector2.Distance(transform.position, checkpoints[nextIndex].position);
        if (dist < reachDistance)
        {
            if (nextIndex == checkpoints.Length - 1)
            {
                Debug.Log("到達終點！");
                finished = true;
                rb.linearVelocity = Vector2.zero;
            }
                
            else
                Debug.Log("通過檢查點 " + (nextIndex + 1));
            nextIndex++;
        }
    }

    void OnGUI()
    {
        if (!finished) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 48;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "到達終點！", style);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
