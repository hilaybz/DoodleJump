using UnityEngine;
using UnityEngine.UI;
public class CameraFollowScript : MonoBehaviour
{
    public Transform target;
    public Text scoreText;
    private void LateUpdate()
    {
        if (target.position.y > transform.position.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, target.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = ((int)(transform.position.y * 10)).ToString();
    }
}
