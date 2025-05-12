using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    private Coroutine idleDelayCoroutine;

    [SerializeField] private float moveDuration = 0.1f; // �� ĭ�� �����̴� �ð�
    [SerializeField] private float gridSize = 1f; // �׸��� ������
    private float checkRadius = 0.1f;

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

            // �� üũ
            Collider2D checkWall = Physics2D.OverlapCircle(targetPos, checkRadius, obstacleLayer);
            if (checkWall != null)
            {
                Debug.Log("��");
                TryMoveNext();
                return;
            }

            // Ż�⹮ üũ
            Collider2D checkDoor = Physics2D.OverlapCircle(targetPos, checkRadius, exitDoorLayer);
            if (checkDoor != null)
            {
                if (escape.HasKey())
                {
                    Debug.Log("Ż��");
                    checkDoor.gameObject.SetActive(false); // Ż�ⱸ ��Ȱ��ȭ
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

            // �ڽ� üũ
            Collider2D checkBox = Physics2D.OverlapCircle(targetPos, checkRadius, boxLayer);
            if (checkBox != null)
            {
                // ���� ������ ��ֹ� �ִ��� Ȯ��
                Vector3 boxNextPos = targetPos + (Vector3)(nextDir * gridSize);
                Collider2D obstacle = Physics2D.OverlapCircle(boxNextPos, checkRadius, boxLayer | obstacleLayer | exitDoorLayer);

                if (obstacle == null)
                {
                    StartCoroutine(MoveBoxThenPlayer(checkBox.gameObject, boxNextPos, targetPos));
                }
                else
                {
                    StartCoroutine(CantMoveBox());
                    Debug.Log("���� �ڿ� ��");
                    movingCount.MoveCounting();
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
        yield return StartCoroutine(MoveBox(box, boxTarget));      // ���� ���� �̵�
        yield return StartCoroutine(MoveToPosition(playerTarget)); // �״��� �÷��̾� �̵�
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

        yield return new WaitForSeconds(0.15f); //������

        isMoving = false;
        movingCount.MoveCounting(); // ������ ��� ���� ī��Ʈ 1 ����
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
