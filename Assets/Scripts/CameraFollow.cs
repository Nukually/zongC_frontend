using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // 当前相机跟随的目标物体
    public float smoothSpeed = 0.125f;  // 相机平滑跟随的速度
    public Vector3 offset;  // 相机与目标物体之间的偏移量
    public GameObject[] objectsWithTag;  // 存储所有带有"Player"标签的物体
    public Transform[] transforms;  // 存储所有带有"Player"标签物体的Transform
    private int currentIndex = 0;  // 当前目标的索引

    // 自由模式控制
    public bool isFreeMode = false;  // 判断是否处于自由模式
    public float moveSpeed = 10f;  // 相机平移速度
    public float zoomSpeed = 1f;  // 相机缩放速度
    private float currentZoom = 5f;  // 当前缩放级别（相机的正交大小）

    // 用来存储初始相机位置
    private Vector3 initialPosition;
    
    void Start()
    {
        // 获取初始位置
        initialPosition = transform.position;

        // 找到所有带有"Player"标签的物体
        objectsWithTag = GameObject.FindGameObjectsWithTag("Player");

        // 初始化transforms数组，存储目标物体的Transform组件
        transforms = new Transform[objectsWithTag.Length];
        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            transforms[i] = objectsWithTag[i].transform;
        }

        // 如果找到了目标物体，则默认选择第一个作为目标
        if (transforms.Length > 0)
        {
            target = transforms[0];
        }
    }

    public void SwitchTarget()
    {
        // 切换目标物体，切换到下一个目标
        currentIndex++;
        SwitchToFollowMode();
        if (currentIndex >= transforms.Length)
        {
            SwitchToFreeMode();
            currentIndex = 0;  // 如果遍历完所有的玩家，回到第一个
        }
        target = transforms[currentIndex];
    }

    void LateUpdate()
    {
        if (isFreeMode)
        {
            // 自由模式下，控制相机的平移和缩放
            HandleFreeModeMovement();
        }
        else
        {
            // 跟随模式，平滑地跟随目标
            FollowTarget();
        }
    }

    // 跟随目标的平滑移动
    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }

    // 自由模式下，控制相机的平移和缩放
    void HandleFreeModeMovement()
    {
        // 获取输入的WASD来控制相机的平移
        float horizontal = Input.GetAxis("Horizontal");  // A/D
        float vertical = Input.GetAxis("Vertical");      // W/S

        // 控制相机的平移
        Vector3 moveDirection = new Vector3(horizontal, vertical, 0f).normalized;

        // 平移相机
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // 使用鼠标滚轮来调整相机的缩放（前进或后退）
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, 2f, 15f);  // 限制相机缩放范围
            Camera.main.orthographicSize = currentZoom;  // 调整相机的正交大小
        }
        // 更新相机的视口区域（可选）
        UpdateViewportRect();
        // 确保相机保持正确的Z轴位置，不影响2D表现
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    // 更新相机的视口区域（Viewport Rect）
    void UpdateViewportRect()
    {
        // 示例：改变相机视口的大小，可以通过以下方式调整
        float aspectRatio = (float)Screen.width / Screen.height;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);  // 修改视口的范围，可以根据需要调整
    }

    // 切换到自由模式
    public void SwitchToFreeMode()
    {
        isFreeMode = true;
    }

    // 切换回跟随模式
    public void SwitchToFollowMode()
    {
        isFreeMode = false;
    }
}

