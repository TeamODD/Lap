using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{

    [SerializeField] private float moveSmoothTime = 0.05f; // �� ĭ�� �����̴� �ð�(�뷫��)
    [SerializeField] private float gridSize = 1f; // �׸��� ������

    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;

    private Queue<Vector2> inputQueue = new Queue<Vector2>(); // �������� ��� ť

    private MovingCount movingCount; // ������ �� �� ī��Ʈ ����
    private Escape escape;
    private RestartScene restartScene; // �� �����

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private LayerMask obstacleLayer; // ���̳� �� ���� ��ֹ�
    private LayerMask boxLayer; // ����
    private LayerMask exitDoorLayer; // Ż�⹮ ���̾�

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
                Debug.Log("��");
                TryMoveNext();
                return;
            }

            Collider2D door = Physics2D.OverlapCircle(targetPos, checkRadius, exitDoorLayer);
            if (door != null)
            {
                if (escape.HasKey())
                {
                    Debug.Log("Ż��");
                    door.gameObject.SetActive(false); // Ż�ⱸ ��Ȱ��ȭ
                    StartCoroutine(MoveToPosition(targetPos));
                    return;
                }
                else
                {
                    Debug.Log("���� ����");
                    TryMoveNext();
                    return;
                }
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, nextDir, gridSize, boxLayer);
            
            if (hit.collider != null)
            {
                // ���� ������ ��ֹ� �ִ��� Ȯ��
                Vector3 boxNextPos = hit.collider.transform.position + (Vector3)(nextDir * gridSize);
                            obstacle = Physics2D.OverlapCircle(boxNextPos, checkRadius, boxLayer | obstacleLayer);
                Collider2D obstacle2 = Physics2D.OverlapCircle(boxNextPos, checkRadius, boxLayer | exitDoorLayer);

                if (obstacle == null && obstacle2 == null)
                {
                    StartCoroutine(MoveBoxThenPlayer(hit.collider.gameObject, boxNextPos, targetPos));
                }
                else
                {
                    // ������ ��� �� ī��Ʈ ����
                    Debug.Log("���� �ڿ� ��");
                    movingCount.MoveCounting(); ;
                    TryMoveNext();
                    return;
                }
            }
            else
            {
                // �׳� �̵�
                StartCoroutine(MoveToPosition(targetPos));
            }
        }
    }
    private IEnumerator MoveBoxThenPlayer(GameObject box, Vector3 boxTarget, Vector3 playerTarget)
    {
        yield return StartCoroutine(MoveBox(box, boxTarget));      // ���� ���� �̵�
        yield return StartCoroutine(MoveToPosition(playerTarget)); // �״��� �÷��̾� �̵�
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
        anim.SetBool("isMove", false); // �ӽ÷� ���� ���� ���� �ø�
        yield return new WaitForSeconds(0.2f); // �� ����Ŀó�� ������ �� �ణ�� ��

        isMoving = false;
        movingCount.MoveCounting(); // ������ ��� ���� ī��Ʈ 1 ����

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

    private int countForDebug = 0; // ���ÿ�
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            Debug.Log("���ô��� " + ++countForDebug);
            movingCount.MoveCounting(); // ���� �� �ٷ� ����
        }
        if (other.CompareTag("Key"))
        {
            Debug.Log("Űȹ��");
            escape.GetKey();
            other.gameObject.SetActive(false);
        }
    }
}
