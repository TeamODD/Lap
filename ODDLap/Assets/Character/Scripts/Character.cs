using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{

    [SerializeField] private float moveSmoothTime = 0.05f; // 한 칸당 움직이는 시간(대략적)
    [SerializeField] private float gridSize = 1f; // 그리드 사이즈

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

    [SerializeField] private float checkRadius = 0.1f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

            Collider2D obstacle = Physics2D.OverlapCircle(targetPos, checkRadius, obstacleLayer);

            if (obstacle != null)
            {
                Debug.Log("벽");
                TryMoveNext();
                return;
            }

            Collider2D door = Physics2D.OverlapCircle(targetPos, checkRadius, exitDoorLayer);
            if (door != null)
            {
                if (escape.HasKey())
                {
                    Debug.Log("탈출");
                    door.gameObject.SetActive(false); // 탈출구 비활성화
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

            RaycastHit2D hit = Physics2D.Raycast(transform.position, nextDir, gridSize, boxLayer);
            
            if (hit.collider != null)
            {
                // 상자 앞으로 장애물 있는지 확인
                Vector3 boxNextPos = hit.collider.transform.position + (Vector3)(nextDir * gridSize);
                            obstacle = Physics2D.OverlapCircle(boxNextPos, checkRadius, boxLayer | obstacleLayer);
                Collider2D obstacle2 = Physics2D.OverlapCircle(boxNextPos, checkRadius, boxLayer | exitDoorLayer);

                if (obstacle == null && obstacle2 == null)
                {
                    StartCoroutine(MoveBoxThenPlayer(hit.collider.gameObject, boxNextPos, targetPos));
                }
                else
                {
                    // 발차기 모션 후 카운트 감소
                    Debug.Log("상자 뒤에 벽");
                    movingCount.MoveCounting(); ;
                    TryMoveNext();
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
    private IEnumerator MoveBoxThenPlayer(GameObject box, Vector3 boxTarget, Vector3 playerTarget)
    {
        yield return StartCoroutine(MoveBox(box, boxTarget));      // 상자 먼저 이동
        yield return StartCoroutine(MoveToPosition(playerTarget)); // 그다음 플레이어 이동
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true; anim.SetBool("isMove", true);

        while ((target - transform.position).sqrMagnitude > 0.001f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, moveSmoothTime);
            yield return null;
        }

        transform.position = target;
        velocity = Vector3.zero;
        anim.SetBool("isMove", false); // 임시로 보기 좋게 위로 올림
        yield return new WaitForSeconds(0.2f); // 헬 테이커처럼 움직임 간 약간의 텀

        isMoving = false;
        movingCount.MoveCounting(); // 움직임 모션 이후 카운트 1 감소

        TryMoveNext();
    }

    private IEnumerator MoveBox(GameObject box, Vector3 targetPos)
    {
        isMoving = true; anim.SetBool("isKick", true);
        float elapsed = 0f;
        float duration = moveSmoothTime;
        Vector3 start = box.transform.position;

        while (elapsed < duration)
        {
            box.transform.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        box.transform.position = targetPos;
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isKick", false);
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
