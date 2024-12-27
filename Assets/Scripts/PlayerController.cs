using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    private LineRenderer lineRenderer;  
    private Vector3[] positions;  
    private int maxPoints = 100;  
    private int currentPointIndex = 0;  

    void Start()
    {
        lineRenderer = player.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = player.AddComponent<LineRenderer>();
        }
        positions = new Vector3[maxPoints];
        lineRenderer.positionCount = 0;
        // 创建并应用特效材质
        Material trailEffectMaterial = new Material(Shader.Find("Custom/TrailEffectShader"));
        lineRenderer.material = trailEffectMaterial;
        // hello
    }

    public void UpdateRandomNumbers(int randomNumber1, int randomNumber2)
    {
        Vector3 newPosition = new Vector3(randomNumber1, randomNumber2, player.transform.position.z);
        positions[currentPointIndex] = newPosition;
        currentPointIndex = (currentPointIndex + 1) % maxPoints;
        lineRenderer.positionCount = Mathf.Min(currentPointIndex, maxPoints);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }

        player.transform.position = newPosition;
    }

    public void UpdateRandomNumbersWithPose(int randomNumber1, int randomNumber2, int randomNumber3)
    {
        //第三个数代表朝向：北东南西分别是0123
        Vector3 newPosition = new Vector3(randomNumber1, randomNumber2, player.transform.position.z);
        positions[currentPointIndex] = newPosition;
        currentPointIndex = (currentPointIndex + 1) % maxPoints;
        lineRenderer.positionCount = Mathf.Min(currentPointIndex, maxPoints);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
        player.transform.position = newPosition;
        //3-0 2-90 1-180 0-270
        player.transform.rotation = Quaternion.Euler(0.0f,0.0f,90 * (3 - randomNumber3));
        Vector3 newScale = new Vector3(player.transform.localScale.x, -player.transform.localScale.y,
            player.transform.localScale.z);
        //TODO
        //player.transform.localScale = 
    }
}