using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    private Coroutine idleDelayCoroutine;

    [SerializeField] private float moveDuration = 0.1f; // 한 칸당 움직이는 시간
    [SerializeField] private float gridSize = 1f; // 그리드 사이즈
    private float checkRadius = 0.1f;

    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;

    private Queue<Vector2> inputQueue = new Queue<Vector2>(); // 움직임을 담는 큐

    private MovingCount movingCount; // 행위를 할 시 카운트 감소
    private Escape escape;
    private RestartScene restartScene; // 씬 재시작

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private LayerMask obstacleLayer; // 벽이나 못 가는 장애물
    private LayerMask boxLayer; // 상자
    private LayerMask exitDoorLayer; // 탈출문 레이어

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        movingCount = FindAnyObjectByType<MovingCount>();
        escape = FindAnyObjectByType<Escape>();
        restartScene = FindAnyObjectByType<RestartScene>();

        obstacleLayer = LayerMask.GetMask("Obstacles");
        boxLayer = LayerMask.GetMask("Boxes");
        exitDoorLayer = LayerMask.GetMask("EXIT");
    }

    void Update()
    {
        Vector2 inputDir = GetInputDirection();

        if (inputDir != Vector2.zero)
        {
            inputQueue.Enqueue(inputDir);
            TryMoveNext();
        }

    }

    private void TryMoveNext()
    {
        if (!isMoving && inputQueue.Count > 0)
        {
            Vector2 nextDir = inputQueue.Dequeue();

            if (nextDir.x < 0)
                spriteRenderer.flipX = true;
            else if (nextDir.x > 0)
                spriteRenderer.flipX = false;

            Vector3 targetPos = transform.position + (Vector3)(nextDir * gridSize);

            // 벽 체크
            Collider2D checkWall = Physics2D.OverlapCircle(targetPos, checkRadius, obstacleLayer);
            if (checkWall != null)
            {
                Debug.Log("벽");
                TryMoveNext();
                return;
            }

            // 탈출문 체크
            Collider2D checkDoor = Physics2D.OverlapCircle(targetPos, checkRadius, exitDoorLayer);
            if (checkDoor != null)
            {
                if (escape.HasKey())
                {
                    Debug.Log("탈출");
                    checkDoor.gameObject.SetActive(false); // 탈출구 비활성화
                    StartCoroutine(MoveToPosition(targetPos));
                    return;
                }
                else
                {
                    Debug.Log("열쇠 없음");
                    TryMoveNext();
                    return;
                }
            }

            // 박스 체크
            Collider2D checkBox = Physics2D.OverlapCircle(targetPos, checkRadius, boxLayer);
            if (checkBox != null)
            {
                // 상자 앞으로 장애물 있는지 확인
                Vector3 boxNextPos = targetPos + (Vector3)(nextDir * gridSize);
                Collider2D obstacle = Physics2D.OverlapCircle(boxNextPos, checkRadius, boxLayer | obstacleLayer | exitDoorLayer);

                if (obstacle == null)
                {
                    StartCoroutine(MoveBoxThenPlayer(checkBox.gameObject, boxNextPos, targetPos));
                }
                else
                {
                    StartCoroutine(CantMoveBox());
                    Debug.Log("상자 뒤에 벽");
                    movingCount.MoveCounting();
                    return;
                }
            }

            else
            {
                // 그냥 이동
                StartCoroutine(MoveToPosition(targetPos));
            }
        }
    }
    private IEnumerator CantMoveBox()
    {
        isMoving = true;
        anim.SetTrigger("Push");
        yield return new WaitForSeconds(0.5f);
        isMoving = false;
        TryMoveNext();
    }

    private IEnumerator MoveBoxThenPlayer(GameObject box, Vector3 boxTarget, Vector3 playerTarget)
    {
        isMoving = true;
        anim.SetTrigger("Push");
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveBox(box, boxTarget));      // 상자 먼저 이동
        yield return StartCoroutine(MoveToPosition(playerTarget)); // 그다음 플레이어 이동
    }

    private IEnumerator MoveBox(GameObject box, Vector3 targetPos)
    {
        float elapsed = 0f;
        Vector3 start = box.transform.position;

        while (elapsed < moveDuration)
        {
            box.transform.position = Vector3.Lerp(start, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        box.transform.position = targetPos;
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;
        anim.SetTrigger("Move");

        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }transform.position = target;

        yield return new WaitForSeconds(0.15f); //딜레이

        isMoving = false;
        movingCount.MoveCounting(); // 움직임 모션 이후 카운트 1 감소
        TryMoveNext();
    }

    private Vector2 GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) return Vector2.right;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) return Vector2.down;

        if (Input.GetKeyDown(KeyCode.R)){
            inputQueue.Clear();
            movingCount.initCount();
            restartScene.Restart();
        }

        return Vector2.zero;
    }

    private int countForDebug = 0; // 가시용
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            Debug.Log("가시닿음 " + ++countForDebug);
            movingCount.MoveCounting(); // 닿을 시 바로 감소
        }
        if (other.CompareTag("Key"))
        {
            Debug.Log("키획득");
            escape.GetKey();
            other.gameObject.SetActive(false);
        }
    }
}
