
using UnityEngine;

public class Looktoplayer : MonoBehaviour
{


    private void Update()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
    }




}























//���� ������
//GameObject playerObject;
//AutoMover worm;
//private void Start()
//{

//    playerObject = GameObject.FindGameObjectWithTag("Player");


//    worm = gameObject.AddComponent<AutoMover>();

//    MakingSinMovement(10, 10);
//}

//private void MakingSinMovement(float _sideVectorMagnitude, int _HowMany)
//{
//    worm.RunOnStart = false;

//    worm.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

//    worm.FaceForward = true;
//    worm.DynamicUpVector = false;
//    worm.AddAnchorPoint(transform.position + new Vector3(0, 0, 0), new Vector3(0, 0, 0), transform.localScale);
//    // Sin �Լ��� ����Ͽ� X ��ǥ�� �����Ͽ� �翷���� ������


//    for (int i = 1; i <= _HowMany; i++)
//    {

//        Vector3 position = (i * (((playerObject.transform.position - gameObject.transform.position) / (_HowMany+1f))) + transform.position);


//        // �� ������Ʈ�� �÷��̾� ������Ʈ�� �ٶ󺸴� ������ ����մϴ�.
//        Vector3 directionToPlayer = playerObject.transform.position - transform.position;

//        // ���� ���͸� ����մϴ�. �̸� ���� x�� z ���� ��Ҹ� ��ȯ�ϰ� �� �� �ϳ��� ��ȣ�� �ٲپ� ����մϴ�.
//        Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

//        // �ʿ��� ��� sideVector�� ũ�⸦ ������ �� �ֽ��ϴ�.
//        float sideVectorMagnitude = _sideVectorMagnitude; // �ʿ��� ũ�⿡ �°� �� ���� �����ϼ���.
//        sideVector *= sideVectorMagnitude;

//        if (i % 2 == 0)
//        {
//            // ���� sideVector�� ��ġ�� �߰��� �� �ֽ��ϴ�.
//            position += sideVector;

//        }
//        else
//        {
//            position -= sideVector;
//        }

//        // ������ �ڵ�...


//        Vector3 rotation = playerObject.transform.eulerAngles;
//        Vector3 scale = gameObject.transform.localScale;
//        worm.AddAnchorPoint(position, rotation, scale);
//    }
//    worm.Length = 10f;
//    worm.LoopingStyle = AutoMoverLoopingStyle.repeat;
//    worm.CurveStyle = AutoMoverCurve.SplineThroughPoints;


//    worm.AddAnchorPoint(playerObject.transform.position, playerObject.transform.eulerAngles, playerObject.transform.localScale);



//    worm.Length = 10f;
//    worm.LoopingStyle = AutoMoverLoopingStyle.repeat;
//    worm.CurveStyle = AutoMoverCurve.SplineThroughPoints;

//    worm.StartMoving();
//}

