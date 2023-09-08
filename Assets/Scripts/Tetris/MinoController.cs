using UnityEngine;

public class MinoController : MonoBehaviour
{
    public TetrisManager tetrisManager;

    [SerializeField] float threshold = 0;
    [SerializeField] float horizontalDeltaTime = 0;
    [SerializeField] float delayUntilTurbo = 0.5f;
    [SerializeField] float softDropDeltaTime = 0.0375f;
    [SerializeField] float softDropInterval = 0.0375f;
    private TickGenerator tickGenerator;

    private bool movementInputSpillLock = false;
    private bool turboMovementInputLock = true;
    private bool hardDropInputSpillLock = false;
    private bool rotationInputSpillLock = false;
    private bool holdConseqLock = false;

    private bool isSoftDropping = false;
    private bool isHorizontallyMoving = false;

    private void Start()
    {
        tickGenerator = TickGenerator.Instance;
        tetrisManager.onLiveMinoStop += delegate () { holdConseqLock = false; };
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //연속 하드 드롭 방지 해제
        if (Input.GetButtonUp("Jump"))
            hardDropInputSpillLock = false;

        //연속 소프트 드롭 해제
        if (threshold == 0 ? y == 0 : (y < threshold && y > -threshold))
        {
            tickGenerator.onTickEvent -= ConseqSoftDrop;
            isSoftDropping = false;
            softDropDeltaTime = softDropInterval;
        }

        //연속 회전 방지 해제
        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.L))
            rotationInputSpillLock = false;

        //연속 이동 방지 해제
        if (threshold == 0 ? x == 0 : (x < threshold && x > -threshold))
        {
            tickGenerator.onTickEvent -= ConseqHorizontalMovement;
            movementInputSpillLock = false;
            isHorizontallyMoving = false;
            turboMovementInputLock = true;
            horizontalDeltaTime = 0;
        }

        //소프트 드롭
        if (y < -threshold && !isSoftDropping)
        {
            isSoftDropping = true;
            tickGenerator.onTickEvent += ConseqSoftDrop;
            return;
        }

        //하드 드롭
        if (Input.GetButton("Jump") && !hardDropInputSpillLock)
        {
            hardDropInputSpillLock = true;
            tetrisManager.HardDrop();
            tetrisManager.ResetTimer();
            return;
        }

        //회전
        if (Input.GetKey(KeyCode.K) && !rotationInputSpillLock) //왼쪽
        {
            rotationInputSpillLock = true;
            tetrisManager.RotateLeft();
            return;
        }
        if (Input.GetKey(KeyCode.L) && !rotationInputSpillLock) //오른쪽
        {
            rotationInputSpillLock = true;
            tetrisManager.RotateRight();
            return;
        }

        //좌우 이동
        if (x != 0 && !(!movementInputSpillLock ^ turboMovementInputLock))
        {
            movementInputSpillLock = true;
            if (!isHorizontallyMoving)
            {
                isHorizontallyMoving = true;
                tickGenerator.onTickEvent += ConseqHorizontalMovement;
            }
            if (x < -threshold)
                tetrisManager.MoveLeft();
            else if (x > threshold)
                tetrisManager.MoveRight();
            return;
        }

        //홀드
        if (y > threshold && !holdConseqLock)
        {
            holdConseqLock = true;
            tetrisManager.Hold();
            tetrisManager.ResetTimer();
            return;
        }
    }

    void ConseqHorizontalMovement(float delta)
    {
        horizontalDeltaTime += delta;
        if (horizontalDeltaTime > delayUntilTurbo)
        {
            horizontalDeltaTime -= delayUntilTurbo;
            turboMovementInputLock = false;
        }
    }

    void ConseqSoftDrop(float delta)
    {
        softDropDeltaTime += delta;
        if (softDropDeltaTime > softDropInterval)
        {
            softDropDeltaTime -= softDropInterval;
            tetrisManager.SoftDrop();
            tetrisManager.ResetTimer();
        }
    }
}
