using System.Collections.Generic;
using UnityEngine;

namespace DistantLands
{
    public class Worm : MonoBehaviour
    {
        [System.Serializable]
        public class Segment
        {
            public Transform transform; // 각 세그먼트의 Transform (위치 및 회전 정보)을 저장하는 변수
            public float offset; // 현재 세그먼트와 다음 세그먼트 간의 거리 차이
            public Transform nextSegment; // 현재 세그먼트의 다음 세그먼트
        }

        [HideInInspector]
        public List<Segment> segments; // 세그먼트들을 저장하는 리스트

        public Transform[] tail; // 세그먼트들의 위치 정보를 포함하는 배열

        [Tooltip("Which axis is the world up")]
        public Vector3 up; // 세계에서 '위'를 나타내는 벡터

        Vector3 oldTravelVector; // 이전 프레임에서의 이동 벡터

        // Start is called before the first frame update
        void Start()
        {
            int j = 0;

            foreach (Transform i in tail)
            {
                if (i != tail[0])
                {
                    // 새로운 세그먼트 객체 생성
                    Segment k = new Segment();
                    k.transform = i;
                    k.nextSegment = tail[j];
                    segments.Add(k);
                    j++;
                }
                else
                {
                    // 첫 번째 세그먼트의 경우, 다음 세그먼트가 없으므로 nextSegment는 null입니다.
                    Segment k = new Segment();
                    k.transform = i;
                    segments.Add(k);
                }
            }

            // 각 세그먼트 사이의 초기 거리(offset)를 계산
            foreach (Segment i in segments)
            {
                if (i.nextSegment)
                    i.offset = Vector3.Distance(i.transform.position, i.nextSegment.position);
            }

            oldTravelVector = segments[0].transform.position; // 초기 이전 이동 벡터 설정
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 travelVector = segments[0].transform.position - oldTravelVector;

            foreach (Segment i in segments)
            {
                if (i.nextSegment)
                {
                    // 각 세그먼트를 다음 세그먼트 쪽으로 이동시키고, 방향을 조절하여 따라가는 뱀처럼 보이게 합니다.
                    float travelAmount = Vector3.Distance(i.transform.position, i.nextSegment.position) - i.offset;
                    i.transform.LookAt(i.nextSegment, up);
                    i.transform.position += i.transform.forward * travelAmount;
                }
            }

            oldTravelVector = travelVector; // 현재 이동 벡터를 이전 이동 벡터로 설정
        }
    }
}