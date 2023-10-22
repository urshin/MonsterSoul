using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class DefMovement : MonoBehaviour
{
    public static DefMovement Instance;
    GameManager GM;
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

    [Header("Settings")]  // ���߿� �÷��̾ ���� �ٲ� // start���� �����ؿ���
    public float gravity = 25f; // �߷�
    public float moveSpeed = 4f; // �̵� �ӵ�
    public float RunningSpeed = 7f; // ĳ���� �ٱ� ���ǵ�
    public float rotateSpeed = 3f; // ȸ�� �ӵ�
    public float JumpSpeed = 5f; // ���� 

    public bool lockMovement; // �̵� ��� ����


    
    void Start()
    {
        GM = GameManager.Instance;
        Invoke("LateStart",1f);
        
    }


    void LateStart()
    {
        anim = Player.Instance.PlayerAvatar.GetComponent<Animator>();
        controller = GetComponent<CharacterController>(); // ĳ���� ��Ʈ�ѷ� ������Ʈ ��������
        cam = Camera.main.transform; // ���� ī�޶��� Transform ��������
        InitializedPlayerInfo();

    }
    void InitializedPlayerInfo()
    {
        moveSpeed = GM.PlayerSpeed;
        Nowspeed = moveSpeed; // �̵� �ӵ� �ʱ�ȭ
        JumpSpeed = GM.PlayerJumpPower;
        RunningSpeed = moveSpeed * 1.4f;
    }

    public void Update()
    {
        GetInput(); // �Է� �ޱ�


        if (Player.Instance.isPlayerBulling) //���ݹ޴� ����
        {
           if( Player.Instance.IsDown)
            {
            anim.SetTrigger("Down");
                Player.Instance.IsDown = false;
            }
            

        }
        else
        {
            PlayerMovement(); // �÷��̾� �̵�

        }



        if (!lockMovement) PlayerRotation(); // �̵��� ��� ���°� �ƴϸ� �÷��̾� ȸ��
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
        if (anim.GetBool("IsAttack") || anim.GetBool("IsRoll") || anim.GetBool("IsJump"))
        {
            // �̵� �Է��� ����
        }
        else
        {
            Nowspeed = moveSpeed;
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
            anim.SetBool("IsJump", false);
            if (!anim.GetBool("IsAttack"))
            {
                if (Input.GetButton("Jump") && !anim.GetBool("IsRoll"))
                {
                    anim.SetBool("IsJump", true);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!anim.GetBool("IsRoll"))
                {
                    Roll();
                    anim.SetTrigger("Rolling");
                }
            }
        }

        HandleMouseInput(KeyCode.Mouse0, "LeftClick");
        HandleMouseInput(KeyCode.Mouse1, "RightClick");

        // �ִϸ������� �Ķ���� ���� 
        anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

    void HandleMouseInput(KeyCode mouseButton, string boolName) // �޺�
    {
        if (Input.GetKeyDown(mouseButton))
        {
            anim.SetBool(boolName, true);
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack");
                anim.SetBool("AbleCombo", false);
            }
        }
        if (Input.GetKeyUp(mouseButton))
        {
            anim.SetBool(boolName, false);
        }
    }

    public void Roll()
    {
        StartCoroutine(Roll_Movement());
    }

    private IEnumerator Roll_Movement()
    {
        anim.SetBool("IsRoll", true);

        // ������ ������ ���� ���� ����
        Vector3 rollDir = (cam.forward * moveInput.y + cam.right * moveInput.x).normalized;
        rollDir.y = 0; // Y���� 0���� �����Ͽ� ���� �̵� ������ ����

        // ��ǥ �̵� �Ÿ� ����
        float rollDistance = 5.0f;

        // ���� ���� ����
        Vector3 startPos = transform.position;

        // ��ǥ ���� ���
        Vector3 endPos = startPos + rollDir.normalized * rollDistance;

        //while (Vector3.Distance(transform.position, endPos) > 1f)
        //{
        // ���� ��ǥ ���� �������� �̵�
        Vector3 moveDirection = (endPos - transform.position).normalized;
        controller.Move(moveDirection * (moveSpeed + (moveSpeed / 2)) * Time.deltaTime);
        yield return null;
        //}
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return; // �̵� ������ ������ ȸ������ ����

        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z); // ȸ�� ���� ���� ����
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed); // �ε巯�� ȸ�� ����
    }
}