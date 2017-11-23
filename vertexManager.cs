using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vertex//벡터정보담고있는 버텍스 구조체
{
    public Vector3 position;//정점의 위치
    public Vector3 parentPosition;//부모의 정점정보
    public bool isVisit;//정점의 방문정보
    public bool isWater;//물일경우 갈수없는곳으로 판정
}

public class vertexManager : MonoBehaviour
{
    public int height;//맵의높이
    public int width;//맵의너비
    public int targetNum;//목적지 번호 인덱스번호아님!

    public GameObject bottomVertex;//바닥
    public GameObject waterVertex;//물
    public GameObject targetVertex;//목표지점
    public Transform ball;//움직일공

    private Queue<Vertex> queue = new Queue<Vertex>();
    private List<Vertex> allSearch = new List<Vertex>();//갈수있는 모든 BFS탐색리스트
    private List<Vertex> minSearch = new List<Vertex>();//목적지까지 갈수있는 최단거리 리스트
    private Vertex[] vertex;//정점의 정보들
   
    void Start()
    {
        vertex = new Vertex[height * width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j + width * i == 4|| j + width * i == 8|| j + width * i == 12|| j + width * i == 21|| j + width * i == 22|| j + width * i == 19|| j + width * i == 11)
                {
                    Instantiate(waterVertex).transform.position = new Vector3(j, 0, i);
                    vertex[j + width * i].isWater = true;//물로취급하고 갈수없음
                }
                else if(j + width * i==targetNum-1)
                {
                    Instantiate(targetVertex).transform.position = new Vector3(j, 0, i);
                    vertex[j + width * i].isWater = false;//모든곳은 갈수있게 초기화
                }
                else
                {
                    Instantiate(bottomVertex).transform.position = new Vector3(j, 0, i);
                    vertex[j + width * i].isWater = false;//모든곳은 갈수있게 초기화
                }
                vertex[j + width * i].position = new Vector3(j, 0, i);
                vertex[j + width * i].isVisit = false;//처음에 모든버텍스의 방문정보는 false로 초기화
            }
        }
        queue.Enqueue(vertex[0]);//큐에 초기 버텍스값 삽입
        vertex[0].isVisit = true;//큐에 삽입되면 방문했다고 함
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))//스페이스 한번누르면 실행
        {
            StartCoroutine(StartFindRoutine(targetNum));   
        }
    }
    IEnumerator StartFindRoutine(int _targetNum)
    {
        FindTarget(_targetNum);
        for (int i = 0; i < minSearch.Count; i++)
        {
            ball.position = minSearch[i].position;
            yield return new WaitForSeconds(0.2f);
            if (ball.position == vertex[_targetNum-1].position)//공이 목적지 까지도달하면 끝내기
                yield break;
        }
    }

    void BFS()
    {
        while (queue.Count != 0)
        {
            Vertex myVec = queue.Dequeue();
            allSearch.Add(myVec);

            for (int i = 0; i < height*width; i++)
            {
                 if (Vector3.Distance(myVec.position, vertex[i].position) == 1 && !vertex[i].isVisit&&!vertex[i].isWater)//큐에서 나온 정점의 거리가 1이고 방문정보가false인 정점이고 물이아닌정점만
                 {
                    vertex[i].isVisit = true;//큐에넣을때 중복방지위해 방문정보true로
                    vertex[i].parentPosition = myVec.position;//큐에서 빼준버텍스는 큐로넣으려는 버텍스의 부모정점으로 지정
                    queue.Enqueue(vertex[i]);//큐에 삽입
                 }
            }
        }
    }
    void FindTarget(int targetIndex)
    {
        BFS();
        minSearch.Add(vertex[targetIndex-1]);
        int num = 0;
        while(true)
        {
            if (allSearch[num].position == vertex[targetIndex-1].position)//버텍스배열 인덱스와 bfs배열인덱스가 맞지 않아서 맞춰주는 과정
                break;
            num++;
        }
        for (int i = num; i >= 0; i--)
        {
            if (allSearch[num].parentPosition == allSearch[i].position)//타겟포지션의 부모포지션과 탐색포지션이 일치하면
            {
                minSearch.Add(allSearch[i]);//최단거리 리스트에 삽입
                num = i;//목적지의 부모를 다시 비교값으로 변경
            }
        }
        minSearch.Reverse();//목적지부터 원점순서대로 되있는것을 다시 뒤집는다
    }
}
   