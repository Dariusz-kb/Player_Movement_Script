using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationAmount = 15f;
    [SerializeField] private float rotationSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isDragging = false;

    private Camera mainCam;
    private Shooter shooter;

    private void Start()
    {
        mainCam = Camera.main;
        shooter = GetComponent<Shooter>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandleInput();

        if (isDragging)
        {
            DragMoveToTarget();
        }
        else
        {
            MoveToTarget();
        }

        HandleRotation();
    }

    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCam.nearClipPlane));
        worldPos.z = transform.position.z;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverPlayer(worldPos))
            {
                isDragging = true;
                shooter.isFiring = true;
            }
            else
            {
                SetTargetPosition(worldPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            shooter.isFiring = false;
        }

        if (isDragging)
        {
            targetPosition = worldPos;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector3 touchPos = mainCam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCam.nearClipPlane));
        touchPos.z = transform.position.z;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (IsPointerOverPlayer(touchPos))
                {
                    isDragging = true;
                    shooter.isFiring = true;
                }
                else
                {
                    SetTargetPosition(touchPos);
                }
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (isDragging)
                {
                    targetPosition = touchPos;
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isDragging = false;
                shooter.isFiring = false;
                break;
        }
    }

    private void SetTargetPosition(Vector3 worldPos)
    {
        targetPosition = worldPos;
        isMoving = true;
    }

    private void MoveToTarget()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    private void DragMoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        float directionX = targetPosition.x - transform.position.x;
        float targetYRotation = 0f;

        if (Mathf.Abs(directionX) > 0.01f)
        {
            targetYRotation = directionX < 0 ? rotationAmount : -rotationAmount;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private bool IsPointerOverPlayer(Vector3 point)
    {
        Collider2D hit = Physics2D.OverlapPoint(point);
        return hit != null && hit.transform == transform;
    }
}
