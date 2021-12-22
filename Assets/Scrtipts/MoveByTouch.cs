using UnityEngine;

public class MoveByTouch : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;

    public GameObject[] players;

    public Rigidbody rb;

    Touch touch;
    Vector3 touchPosition,
        whereToMove;
    bool isMoving = false;

    float previousDistanceToTouchPos,
        currentDistanceToTouchPos;

    float distance;

    void Start()
    {
        rb = players[0].GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isMoving)
            currentDistanceToTouchPos = (touchPosition - transform.position).magnitude;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                touch = Input.touches[i];
                if (
                    UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(
                        touch.fingerId
                    )
                )
                    return;

                if (touch.phase == TouchPhase.Began)
                {
                    previousDistanceToTouchPos = 0;
                    currentDistanceToTouchPos = 0;
                    isMoving = true;

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        touchPosition = hit.point;
                        whereToMove = (hit.point - transform.position).normalized;
                        rb.velocity = new Vector3(whereToMove.x * speed, 0, whereToMove.z * speed);
                    }
                }
            }
        }

        if (currentDistanceToTouchPos > previousDistanceToTouchPos)
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }

        if (isMoving)
            previousDistanceToTouchPos = (touchPosition - transform.position).magnitude;
    }
}
