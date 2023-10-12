using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoMoverPro
{
    /// <summary>
    /// AutoMover를 스크립트로 사용하는 예제.
    /// 보이지 않는 "레일"을 따라 이동하는 포탄이 부착된 대포 기지를 이동시킵니다.
    /// </summary>
    public class AutoMoverScriptDemo : MonoBehaviour
    {
        // 오른쪽 대포를 위한 게임 오브젝트
        public GameObject rightCannon;

        // 포탄을 위한 게임 오브젝트
        public GameObject ammo;

        // 프로펠러를 위한 게임 오브젝트
        public GameObject propeller;

        // 모든 포탄의 목록. 이 목록은 Update() 메서드에서 사용됩니다.
        private List<AutoMover> ammos;

        void Start()
        {
            // 컴포넌트를 생성하고 즉시 이동 비활성화
            AutoMover submarine = gameObject.AddComponent<AutoMover>();
            submarine.RunOnStart = false;
            AutoMover cannonMover = rightCannon.AddComponent<AutoMover>();
            cannonMover.RunOnStart = false;
            AutoMover ammoMover = ammo.AddComponent<AutoMover>();
            ammoMover.RunOnStart = false;

            // 잠수함은 월드 공간에서 이동하고 대포는 로컬 공간에서 이동하지만 포탄은 대포가 발사된 후에 로컬 공간이 아닌 월드 공간에서 이동합니다.
            submarine.AnchorPointSpace = AutoMoverAnchorPointSpace.world;
            cannonMover.AnchorPointSpace = AutoMoverAnchorPointSpace.local;
            ammoMover.AnchorPointSpace = AutoMoverAnchorPointSpace.world;

            // 대포 기지가 "레일"을 따라 이동하도록 설정
            submarine.FaceForward = true;
            submarine.DynamicUpVector = false;

            // 객체의 첫 번째 앵커 포인트 추가
            submarine.AddAnchorPoint(transform.position + new Vector3(0, 0, 0), new Vector3(0, 90, 0), transform.localScale);
            submarine.AddAnchorPoint(transform.position + new Vector3(3, 0, 0), new Vector3(0, 90, 0), transform.localScale);
            submarine.AddAnchorPoint(transform.position + new Vector3(5, 2, -0.5f), new Vector3(0, 90, 0), transform.localScale);
            submarine.AddAnchorPoint(transform.position + new Vector3(7, 1, 0.5f), new Vector3(0, 90, 0), transform.localScale);
            submarine.AddAnchorPoint(transform.position + new Vector3(10, 0, 0), new Vector3(0, 90, 0), transform.localScale);

            // 주 객체는 10초 동안 이동하며 곡선은 앵커 포인트를 통과하는 스플라인입니다.
            submarine.Length = 10f;
            submarine.LoopingStyle = AutoMoverLoopingStyle.repeat;
            submarine.CurveStyle = AutoMoverCurve.SplineThroughPoints;

            // 자식 이동 (대포)을 위한 앵커 포인트 추가
            cannonMover.AddAnchorPoint(new Vector3(0, 0, 0), new Vector3(0, 0, 0), rightCannon.transform.localScale);
            cannonMover.AddAnchorPoint(new Vector3(0, 0.6f, 0), new Vector3(0, 0, 0), rightCannon.transform.localScale);

            // 대포는 1초 주기로 상하로 이동합니다.
            cannonMover.Length = 1f;
            cannonMover.LoopingStyle = AutoMoverLoopingStyle.loop;
            cannonMover.CurveStyle = AutoMoverCurve.Linear;

            // 대포에 약간의 부드러운 노이즈 포함
            cannonMover.PositionNoiseAmplitude = new Vector3(0.1f, 0.1f, 0.1f);
            cannonMover.PositionNoiseType = AutoMoverNoiseType.smoothRandom;

            // 포탄은 대포에서 일정한 거리로 선형 이동합니다.
            ammoMover.AddAnchorPoint(cannonMover.transform.TransformPoint(new Vector3(1.4f, 0.25f, 0)), cannonMover.transform.rotation * new Vector3(90, 0, 0), ammo.transform.lossyScale);
            ammoMover.AddAnchorPoint(cannonMover.transform.TransformPoint(new Vector3(1.4f, 0.25f, 10f)), cannonMover.transform.rotation * new Vector3(90, 0, 0), ammo.transform.lossyScale);

            // 포탄은 0.8초 동안 공중에 있으며 1번 루프 후에 정지합니다.
            // Update() 메서드를 사용하여 포탄의 월드 공간에서 앵커 포인트를 업데이트합니다.
            ammoMover.Length = 0.8f;
            ammoMover.LoopingStyle = AutoMoverLoopingStyle.repeat;
            ammoMover.CurveStyle = AutoMoverCurve.Linear;
            ammoMover.StopAfter = 1;

            // 포탄을 더 자주 발사하기 위해 포탄을 복제합니다.
            GameObject ammo2 = Instantiate(ammo);
            AutoMover ammo2Mover = ammo2.GetComponent<AutoMover>();
            ammo2.transform.parent = rightCannon.transform;

            // 두 번째 포탄은 첫 번째 포탄보다 0.4초 후에 발사됩니다.
            ammo2Mover.DelayStartMin = 0.4f;

            // 첫 번째 대포를 복제하여 두 번째 대포를 만듭니다.
            GameObject cannon2 = Instantiate(rightCannon);
            AutoMover cannon2Mover = cannon2.GetComponent<AutoMover>();
            cannon2.transform.parent = submarine.transform;

            // 대포를 반대쪽으로 이동시킵니다.
            cannon2Mover.SetAnchorPointPosition(0, new Vector3(-2.7f, 0, 0));
            cannon2Mover.SetAnchorPointPosition(1, new Vector3(-2.7f, 0.6f, 0));

            // 두 번째 대포의 포탄의 AutoMover 가져오기
            AutoMover[] cannon2AndAmmo = cannon2.GetComponentsInChildren<AutoMover>();

            // 마지막으로 프로펠러를 회전시킵니다!
            // 로컬 앵커 포인트 공간에서 1초에 한 바퀴 회전합니다.
            AutoMover propellerSpinner = propeller.AddComponent<AutoMover>();
            propellerSpinner.AnchorPointSpace = AutoMoverAnchorPointSpace.local;
            propellerSpinner.Length = 1;
            propellerSpinner.AddAnchorPoint();
            propellerSpinner.AddAnchorPoint(propeller.transform.localPosition, new Vector3(0, 0, 360));

            // 모든 포탄 AutoMover를 업데이트하기 위해 포탄 목록에 추가합니다.
            ammos = new List<AutoMover>();
            ammos.Add(ammoMover);
            ammos.Add(ammo2Mover);
            ammos.Add(cannon2AndAmmo[cannon2AndAmmo.Length - 2]);
            ammos.Add(cannon2AndAmmo[cannon2AndAmmo.Length - 1]);

            // 객체를 이동 시작합니다.
            submarine.StartMoving();
            cannonMover.StartMoving();
            cannon2Mover.StartMoving();
            for (int i = 0; i < ammos.Count; ++i) { ammos[i].StartMoving(); }
        }

        private void Update()
        {
            // 포탄이 발사되면 포탄의 앵커 포인트를 업데이트합니다.
            for (int i = 0; i < ammos.Count; ++i)
            {
                if (!ammos[i].Moving)
                {
                    // 포탄의 시작 지연 시간을 동기화하지 않으므로 0으로 설정합니다.
                    ammos[i].DelayStartMin = 0;
                    ammos[i].DelayStartMax = 0;

                    // 현재 대포의 위치로 앵커 포인트를 업데이트합니다.
                    ammos[i].SetAnchorPointPosition(0, ammos[i].transform.parent.TransformPoint(new Vector3(1.4f, 0.25f, 0)));
                    ammos[i].SetAnchorPointPosition(1, ammos[i].transform.parent.TransformPoint(new Vector3(1.4f, 0.25f, 10f)));

                    // 현재 대포의 회전으로 앵커 포인트를 업데이트합니다.
                    ammos[i].SetAnchorPointRotation(0, (ammos[i].transform.parent.rotation * Quaternion.Euler(new Vector3(90, 0, 0))).eulerAngles);
                    ammos[i].SetAnchorPointRotation(1, (ammos[i].transform.parent.rotation * Quaternion.Euler(new Vector3(90, 0, 0))).eulerAngles);

                    // 포탄을 다시 이동 시작합니다.
                    ammos[i].StartMoving();
                }
            }
        }
    }
}