using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefMovement : MonoBehaviour
{
    CharacterController controller; // ĳ���� ��Ʈ�ѷ� ������Ʈ
    Animator anim; // �ִϸ����� ������Ʈ
    Transform cam; // ���� ī�޶��� Transform

    float speedSmoothVelocity; // �ӵ� �ε巴�� ��ȭ��Ű�� �� ���Ǵ� ����
    float speedSmoothTime; // �ӵ� �ε巴�� ��ȭ��Ű�� �� ���Ǵ� �ð�
    [SerializeField] float currentSpeed; // ���� �̵� �ӵ�
    [SerializeField] float velocityY; // ���� �ӵ� (�߷� ����)
    Vector3 moveInput; // �̵� �Է� ����
    Vector3 dir; // �̵� ���� ����

    [Header("Settings")]
    [SerializeField] float gravity = 25f; // �߷�
    [SerializeField] float Nowspeed;      // ĳ���� ���� ������ ���ǵ�.
    [SerializeField] float moveSpeed = 4f; // �̵� �ӵ�
    [SerializeField] float RunningSpeed = 7f; //ĳ���� �ٱ� ���ǵ�
    [SerializeField] float rotateSpeed = 3f; // ȸ�� �ӵ�
    [SerializeField] float JumpSpeed = 5f; //���� 

    public bool lockMovement; // �̵� ��� ����

    void Start()
    {
        //anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        anim = GetComponentInChildren<Animator>(); // �ִϸ����� ������Ʈ ��������
        controller = GetComponent<CharacterController>(); // ĳ���� ��Ʈ�ѷ� ������Ʈ ��������
        cam = Camera.main.transform; // ���� ī�޶��� Transform ��������
        Nowspeed = moveSpeed; //�̵��ӵ� �ʱ�ȭ
    }

    void Update()
    {
        GetInput(); // �Է� �ޱ�
        PlayerMovement(); // �÷��̾� �̵�
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

        dir = (forward * moveInput.y + right * moveInput.x).normalized; // �Է��� ������� �̵� ���� ���� ����
    }

    private void PlayerMovement()
    {
        GameManager.Instance.PlayerCurrentState = PlayerState.Move; //�÷��̾� �������
        currentSpeed = Mathf.SmoothDamp(currentSpeed, Nowspeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime); // �ε巯�� �ӵ� ��ȭ ���

        if (velocityY > -10) velocityY -= Time.deltaTime * gravity; // �߷� ����

        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY; // �̵� �ӵ� ���� ���

        controller.Move(velocity * Time.deltaTime); // �̵� �ӵ��� ĳ���� �̵�

        if (controller.isGrounded)
        {
            anim.SetBool("Jump", false);

            if (Input.GetButton("Jump"))
            {

                velocityY = JumpSpeed; // ���� �ӵ��� ���� �ӵ��� ����
                anim.SetBool("Jump", true);
                //anim.Play("Jump");

            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!anim.GetBool("Roll"))
                {
                    
                    StartCoroutine(Roll_Movement());
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //NormalAttack();
            anim.SetBool("LeftClick",true);
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack");
                anim.SetBool("AbleCombo", false);
            }
        }
        if (Input.GetKeyUp (KeyCode.Mouse0))
            {
            anim.SetBool("LeftClick", false);

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //NormalAttack();
            anim.SetBool("RightClick", true);
            if (anim.GetBool("AbleCombo"))
            {
                anim.SetTrigger("GoNextAttack");
                anim.SetBool("AbleCombo", false);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool("RightClick", false);

        }



        // �ִϸ������� �Ķ���� ���� 
        anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

   


    IEnumerator Roll_Movement()
    {
        anim.SetBool("Roll", true);

        // ������ ������ ���� ���� ����
        Vector3 rollDir = dir;
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

        if (Vector3.Distance(transform.position, endPos) <= 1f)
        {
            anim.SetBool("Roll", false);
        }
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return; // �̵� ������ ������ ȸ������ ����

        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z); // ȸ�� ���� ���� ����
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed); // �ε巯�� ȸ�� ����
    }
}