
using UnityEngine;

public class Looktoplayer : MonoBehaviour
{


    private void Update()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
    }




}























//사인 움직임
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
//    // Sin 함수를 사용하여 X 좌표를 조정하여 양옆으로 움직임


//    for (int i = 1; i <= _HowMany; i++)
//    {

//        Vector3 position = (i * (((playerObject.transform.position - gameObject.transform.position) / (_HowMany+1f))) + transform.position);


//        // 이 오브젝트가 플레이어 오브젝트를 바라보는 방향을 계산합니다.
//        Vector3 directionToPlayer = playerObject.transform.position - transform.position;

//        // 수직 벡터를 계산합니다. 이를 위해 x와 z 구성 요소를 교환하고 그 중 하나를 부호를 바꾸어 사용합니다.
//        Vector3 sideVector = new Vector3(-directionToPlayer.z, 0, directionToPlayer.x).normalized;

//        // 필요한 경우 sideVector의 크기를 조절할 수 있습니다.
//        float sideVectorMagnitude = _sideVectorMagnitude; // 필요한 크기에 맞게 이 값을 조절하세요.
//        sideVector *= sideVectorMagnitude;

//        if (i % 2 == 0)
//        {
//            // 이제 sideVector를 위치에 추가할 수 있습니다.
//            position += sideVector;

//        }
//        else
//        {
//            position -= sideVector;
//        }

//        // 나머지 코드...


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

