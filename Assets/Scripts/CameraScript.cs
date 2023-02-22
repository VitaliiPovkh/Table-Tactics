using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float minMovementSpeed;
    [SerializeField] private float maxMovementSpeed;

    [SerializeField] private float borderWidth = 20;


    private Vector2 startMousePosition;

    private void LateUpdate()
    {
        WheelMovement();
        KeysMovement();
        BorderMovement();
    }

    private void WheelMovement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            startMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            float xDist = Mathf.Min(Screen.width - Input.mousePosition.x, Input.mousePosition.x);
            float yDist = Mathf.Min(Screen.height - Input.mousePosition.y, Input.mousePosition.y);

            float speedX = Mathf.Lerp(minMovementSpeed, maxMovementSpeed, Mathf.InverseLerp(Screen.width / 2, 0f, xDist));
            float speedY = Mathf.Lerp(minMovementSpeed, maxMovementSpeed, Mathf.InverseLerp(Screen.height / 2, 0f, yDist));
            float speed = Mathf.Clamp(Mathf.Max(speedX, speedY), minMovementSpeed, maxMovementSpeed);

            Vector3 moveDir = ((Vector2)Input.mousePosition - startMousePosition).normalized;
            transform.position += speed * Time.deltaTime * moveDir;
        }
    }

    private void KeysMovement()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized;
        transform.position += maxMovementSpeed * Time.deltaTime * direction;
    }

    private void BorderMovement()
    {
        
    }

}
