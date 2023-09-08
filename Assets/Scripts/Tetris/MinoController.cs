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

        //���� �ϵ� ��� ���� ����
        if (Input.GetButtonUp("Jump"))
            hardDropInputSpillLock = false;

        //���� ����Ʈ ��� ����
        if (threshold == 0 ? y == 0 : (y < threshold && y > -threshold))
        {
            tickGenerator.onTickEvent -= ConseqSoftDrop;
            isSoftDropping = false;
            softDropDeltaTime = softDropInterval;
        }

        //���� ȸ�� ���� ����
        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.L))
            rotationInputSpillLock = false;

        //���� �̵� ���� ����
        if (threshold == 0 ? x == 0 : (x < threshold && x > -threshold))
        {
            tickGenerator.onTickEvent -= ConseqHorizontalMovement;
            movementInputSpillLock = false;
            isHorizontallyMoving = false;
            turboMovementInputLock = true;
            horizontalDeltaTime = 0;
        }

        //����Ʈ ���
        if (y < -threshold && !isSoftDropping)
        {
            isSoftDropping = true;
            tickGenerator.onTickEvent += ConseqSoftDrop;
            return;
        }

        //�ϵ� ���
        if (Input.GetButton("Jump") && !hardDropInputSpillLock)
        {
            hardDropInputSpillLock = true;
            tetrisManager.HardDrop();
            tetrisManager.ResetTimer();
            return;
        }

        //ȸ��
        if (Input.GetKey(KeyCode.K) && !rotationInputSpillLock) //����
        {
            rotationInputSpillLock = true;
            tetrisManager.RotateLeft();
            return;
        }
        if (Input.GetKey(KeyCode.L) && !rotationInputSpillLock) //������
        {
            rotationInputSpillLock = true;
            tetrisManager.RotateRight();
            return;
        }

        //�¿� �̵�
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

        //Ȧ��
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
