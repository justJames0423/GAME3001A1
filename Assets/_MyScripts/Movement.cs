using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform target;
    public GameObject obstacle;
    float speed = 5.0f;

    private bool isSeeking = false;
    private bool isFleeing = false;
    private bool isArriving = false;
    private bool isAvoiding = false;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetToInitialPosition();
            obstacle.SetActive(false);
            isSeeking = true;
            isFleeing = false;
            isArriving = false;
            isAvoiding = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetToInitialPosition();
            obstacle.SetActive(false);
            isSeeking = false;
            isFleeing = true;
            isArriving = false;
            isAvoiding = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetToInitialPosition();
            obstacle.SetActive(false);
            isSeeking = false;
            isFleeing = false;
            isArriving = true;
            isAvoiding = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetToInitialPosition();
            obstacle.SetActive(true);
            isSeeking = false;
            isFleeing = false;
            isArriving = false;
            isAvoiding = true;
        }

        if (target != null)
        {
            if (isSeeking)
            {
                SeekTarget();
            }
            else if (isFleeing)
            {
                FleeTarget();
            }
            else if (isArriving)
            {
                ArriveAtTarget();
            }
            else if (isAvoiding)
            {
                AvoidObstacle();
            }

        }
    }

    void SeekTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        DrawLineDirection(direction);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.position += direction * speed * Time.deltaTime; 
    }

    void FleeTarget()
    {
        Vector3 direction = transform.position - target.position;
        direction.Normalize();

        DrawLineDirection(direction);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.position += direction * speed * Time.deltaTime;

        float distanceFromTarget = Vector3.Distance(transform.position, target.position);

        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        screenPos.x = Mathf.Clamp(screenPos.x, 0.05f, 0.95f);
        screenPos.y = Mathf.Clamp(screenPos.y, 0.05f, 0.95f);

        transform.position = Camera.main.ViewportToWorldPoint(screenPos);
    }

    void ArriveAtTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float stopDistance = 1f;

        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        DrawLineDirection(direction);

        if (distanceToTarget > stopDistance)
        {
            float slowDown = Mathf.Clamp01(distanceToTarget / 5f);

            transform.position += direction * speed * slowDown * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void AvoidObstacle()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        float distanceToObstacle = Vector3.Distance(transform.position, obstacle.transform.position);

        float avoidanceRadius = 2.0f;

        if (distanceToObstacle < avoidanceRadius)
        {
            Vector3 directionAwayFromObstacle = (transform.position - obstacle.transform.position).normalized;

            transform.position += directionAwayFromObstacle * speed * Time.deltaTime;

            float angle = Mathf.Atan2(directionAwayFromObstacle.y, directionAwayFromObstacle.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            ArriveAtTarget();
        }
    }

    void ResetToInitialPosition()
    {
        transform.position = initialPosition;
    }

    void DrawLineDirection(Vector3 direction)
    {
        Debug.DrawLine(transform.position, transform.position + direction * 2f, Color.red);
    }
}
