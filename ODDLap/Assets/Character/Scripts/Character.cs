using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public MovingCount movingCount;
    public RestartScene restartScene;

    public float moveSmoothTime = 0.1f;
    public float gridSize = 1f;

    private bool isMoving = false;
    private Vector3 velocity = Vector3.zero;

    private Queue<Vector2> inputQueue = new Queue<Vector2>();


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
            Vector3 targetPos = transform.position + (Vector3)(nextDir * gridSize);
            StartCoroutine(MoveToPosition(targetPos));
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;
        movingCount.MoveCounting();

        while ((target - transform.position).sqrMagnitude > 0.001f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, moveSmoothTime);
            yield return null;
        }

        transform.position = target;
        velocity = Vector3.zero;
        isMoving = false;

        TryMoveNext(); // 다음 입력 처리
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
}
