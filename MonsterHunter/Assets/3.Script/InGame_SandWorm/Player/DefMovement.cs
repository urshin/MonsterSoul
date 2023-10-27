using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DefMovement : MonoBehaviour
{
    public static DefMovement Instance; // �� Ŭ������ �ν��Ͻ��� �ٸ� ������ ������ �� �ֵ��� ���� ������ ����
    GameManager GM; // ���� �Ŵ��� Ŭ������ �ν��Ͻ��� �����ϴ� ����

    // Awake �޼ҵ�� ���� ������Ʈ�� Ȱ��ȭ�� �� ȣ��˴ϴ�.
    public void Awake()
    {
        if (Instance == null) // �������� �ڽ��� üũ��, null�� ��쿡�� ����
        {
            Instance = this; // ���� �ڱ� �ڽ��� ������.
        }
    }

    public CharacterController controller; // ĳ���� ��Ʈ�ѷ� ������Ʈ
    public Animator anim; // �ִϸ����� ������Ʈ
    public Transform cam; // ���� ī�޶��� Transform

    public float speedSmoothVelocity; // �ӵ� �ε巴�� ��ȭ��Ű�� �� ���Ǵ� ����
    public float speedSmoothTime; // �ӵ� �ε巴�� ��ȭ��Ű�� �� ���Ǵ� �ð�
    public float currentSpeed; // ���� �̵� �ӵ�
    public float velocityY; // ���� �ӵ� (�߷� ����)
    public Vector3 moveInput; // �̵� �Է� ����
    public Vector3 dir; // �̵� ���� ����
    public float Nowspeed; // ĳ���� ���� ������ ���ǵ�.

    [Header("Settings")] // ���߿� �÷��̾ ���� �ٲ� // start���� �����ؿ���
    public float gravity = 25f; // �߷�
    public float moveSpeed = 4f; // �̵� �ӵ�
    public float RunningSpeed = 7f; // ĳ���� �ٱ� ���ǵ�
    public float rotateSpeed = 3f; // ȸ�� �ӵ�
    public float JumpSpeed = 5f; // ����

    public bool lockMovement; // �̵� ��� ����

    void Start()
    {
        GM = GameManager.Instance; // ���� �Ŵ����� �ν��Ͻ� ��������
        Invoke("LateStart", 0.1f); // LateStart �޼ҵ带 0.1�� �ڿ� ����
        controller = GetComponent<CharacterController>(); // ĳ���� ��Ʈ�ѷ� ������Ʈ ��������
        cam = Camera.main.transform; // ���� ī�޶��� Transform ��������
    }

    void LateStart()
    {
        anim = Player.Instance.PlayerAvatar.GetComponent<Animator>(); // �÷��̾��� �ƹ�Ÿ�� ����� �ִϸ����� ������Ʈ ��������
        InitializedPlayerInfo(); // �÷��̾� ���� �ʱ�ȭ �޼ҵ� ȣ��
    }

    void InitializedPlayerInfo()
    {
        moveSpeed = GM.PlayerSpeed; // ���� �Ŵ������� ������ �÷��̾� �̵� �ӵ� ��������
        Nowspeed = moveSpeed; // �̵� �ӵ� �ʱ�ȭ
        JumpSpeed = GM.PlayerJumpPower; // ���� �Ŵ������� ������ �÷��̾� ���� �Ŀ� ��������
        RunningSpeed = moveSpeed * 1.4f; // �̵� �ӵ��� ����� �ٱ� �ӵ� ����
    }

    public void Update()
    {
        if (!GM.IsLoading) // ������ �ε� ���� �ƴ� ��쿡�� ������Ʈ �޼ҵ� ����
        {
            GetInput(); // �Է� �ޱ�

            if (Player.Instance.isPlayerBulling) // ���ݹ޴� ����
            {
                if (Player.Instance.IsDown)
                {
                    anim.SetTrigger("Down"); // �ִϸ����� Ʈ���� "Down" ����
                    Player.Instance.IsDown = false; // �÷��̾� �ٿ� ���� ����
                }
            }
            else
            {
                PlayerMovement(); // �÷��̾� �̵�
            }

            if (!lockMovement) PlayerRotation(); // �̵��� ��� ���°� �ƴϸ� �÷��̾� ȸ��
        }
    }

    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // ����� ���� �Է� �ޱ�

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize(); // ����ȭ�Ͽ� ���� ���� ����
        right.Normalize(); // ����ȭ�Ͽ� ���� ���� ����

        // ����, ������ �Ǵ� ���� ���� �ƴ� ��쿡�� �̵� �Է��� ó��
        if (anim.GetBool("IsAttack") || anim.GetBool("IsRoll") || anim.GetBool("IsJump") || anim.GetBool("IsBuff"))
        {
            // �̵� �Է��� ����
        }
        else
        {
            Nowspeed = moveSpeed; // ���� �̵� �ӵ� ����
            dir = (forward * moveInput.y + right * moveInput.x).normalized; // �Է��� ������� �̵� ���� ���� ����
        }
    }

    private void PlayerMovement()
    {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, Nowspeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime); // �ε巯�� �ӵ� ��ȭ ���
        if (velocityY > -10) velocityY -= Time.deltaTime * gravity; // �߷� ����

        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY; // �̵� �ӵ� ���� ���

        controller.Move(velocity * Time.deltaTime); // �̵� �ӵ��� ĳ���� �̵�

        anim.SetBool("IsMoving", anim.GetFloat("Movement") >= 0.1f); // �����̴� ������ üũ

        if (controller.isGrounded)
        {
            anim.SetBool("IsJump", false); // ���� �ִϸ��̼� ��Ȱ��ȭ
            if (!anim.GetBool("IsAttack"))
            {
                if (Input.GetButton("Jump") && !anim.GetBool("IsRoll"))
                {
                    anim.SetBool("IsJump", true); // ���� �ִϸ��̼� Ȱ��ȭ
                }
                if (!anim.GetBool("IsJump") && !anim.GetBool("IsRoll"))
                {
                    if (Input.GetKeyDown(KeyCode.R) && Player.Instance.InventoryItems.Count > 0)
                    {
                        Player.Instance.UsingItem(); // ������ ���
                        anim.SetBool("IsBuff", true); // ���� �ִϸ��̼� Ȱ��ȭ
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!anim.GetBool("IsRoll"))
                {
                    Roll(); // ������ �޼ҵ� ȣ��
                    anim.SetTrigger("Rolling"); // ������ �ִϸ��̼� Ʈ���� ����
                }
            }
        }

        HandleMouseInput(KeyCode.Mouse0, "LeftClick"); // ���콺 ���� ��ư �Է� ó��
        HandleMouseInput(KeyCode.Mouse1, "RightClick"); // ���콺 ������ ��ư �Է� ó��

        // �ִϸ������� �Ķ���� ����
        anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

    void HandleMouseInput(KeyCode mouseButton, string boolName) // �޺�
    {
        if (Input.GetKeyDown(mouseButton))
        {
            anim.SetBool(boolName, true); // ���콺 ��ư �Է¿� ���� �Ҹ��� ���� ����
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack"); // ���� ���� Ʈ���� ����
                anim.SetBool("AbleCombo", false); // �޺� ���� ���� ��Ȱ��ȭ
            }
        }
        if (Input.GetKeyUp(mouseButton))
        {
            anim.SetBool(boolName, false); // ���콺 ��ư �� �� �Ҹ��� ���� ��Ȱ��ȭ
        }
    }

    public void Roll()
    {
        StartCoroutine(Roll_Movement()); // ������ �޼ҵ� ����
    }

    private IEnumerator Roll_Movement()
    {
        anim.SetBool("IsRoll", true); // ������ �ִϸ��̼� Ȱ��ȭ

        // ������ ������ ���� ���� ����
        Vector3 rollDir = (cam.forward * moveInput.y + cam.right * moveInput.x).normalized;
        rollDir.y = 0; // Y���� 0���� �����Ͽ� ���� �̵� ������ ����

        // ��ǥ �̵� �Ÿ� ����
        float rollDistance = 5.0f;

        // ���� ���� ����
        Vector3 startPos = transform.position;

        // ��ǥ ���� ���
        Vector3 endPos = startPos + rollDir.normalized * rollDistance;

        while (Vector3.Distance(transform.position, endPos) > 1f)
        {
            // ���� ��ǥ ���� �������� �̵�
            Vector3 moveDirection = (endPos - transform.position).normalized;
            controller.Move(moveDirection * (moveSpeed + (moveSpeed / 2)) * Time.deltaTime);
            yield return null;
        }
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return; // �̵� ������ ������ ȸ������ ����

        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z); // ȸ�� ���� ���� ����
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed); // �ε巯�� ȸ�� ����
    }
}