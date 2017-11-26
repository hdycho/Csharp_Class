using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Bingo
{
    public char name;
    public bool isFitShape;//모양맞은 친구는 맞는 숫자셀때 중복방지하기위해
}
public class Kakao : MonoBehaviour
{
    private Bingo[] Board;
    public int width;
    public int height;
    public string[] inputBoardName;//인스펙터창에서 문자열 입력해줘야함
    private int rimitNum;//만들어진 보드에 문자삽일할때 범위안벋어나게 제한
    int fitNum;//맞춰진 모양개수
    bool isCompareNow;//현재 비교중인지
    Vector3 mousePos;
    // Use this for initialization
    void Start()
    {
        isCompareNow = false;
        rimitNum = 0;
        fitNum = 0;
        Board = new Bingo[width * height];
        inputBoardName = new string[height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Board[j + width * i].name = '0';//0일때는 아무것도 없는걸로 취급
                Board[j + width * i].isFitShape = false;
            }
        }
        ///////////////////////////////////카카오톡 문제///////////////////////////////////
        if (height == 4)
        {
            string s="CCBDE,AAADE,AAABF,CCBBF";
            inputBoardName =s.Split(',');
        }
        if(height==6)
        {
            string s = "TTTANT,RRFACC,RRRFCC,TRRRAA,TTMMMF,TMMTTJ";
            inputBoardName = s.Split(',');
        }
        ////////////////////////////////////////////////////////////////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            for (int i = 0; i < height; i++)
            {
                InputChar(inputBoardName[i]);
            }
        }
        if (isCompareNow)
        {
            StartCoroutine(ResultFitShapeNum());
        }
    }
    IEnumerator ResultFitShapeNum()
    {
        for (int i = 0; i < width * height; i++)
        {
            ShapeCompare(Board[i].name);
            yield return null;
        }
        Debug.Log(fitNum);
    }
    void ShapeCompare(char c)//더이상 맞출모양이 없으면 끝나야함
    {
        for (int i = 0; i < height - 1; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                if (Board[j + (width * i)].name == c && Board[j + 1 + (width * i)].name == c && Board[j + (width) * (i + 1)].name == c && Board[(j + 1) + (width) * (i + 1)].name == c)
                {
                    if (!Board[j + (width * i)].isFitShape)
                        fitNum++;
                    if (!Board[j + 1 + (width * i)].isFitShape)
                        fitNum++;
                    if (!Board[j + (width) * (i + 1)].isFitShape)
                        fitNum++;
                    if (!Board[(j + 1) + (width) * (i + 1)].isFitShape)
                        fitNum++;

                    Board[j + (width * i)].isFitShape = true;
                    Board[j + 1 + (width * i)].isFitShape = true;
                    Board[j + (width) * (i + 1)].isFitShape = true;
                    Board[(j + 1) + (width) * (i + 1)].isFitShape = true;
                }
                else
                {
                    isCompareNow = false;
                }
            }
        }
        DestroyShape();
        DownShape();
    }
    void DestroyShape()//맞은 모양은 지운다
    {
        for (int i = 0; i < height * width; i++)
        {
            if (Board[i].isFitShape)
                Board[i].name = '0';
        }
    }
    void DownShape()//위에있는 모양이 지워진모양 대체해서 내려간다
    {
        Bingo temp = new Bingo();
        //위에방식과 반대로 너비에따라 높이를 차례대로비교한다
        for (int i = 0; i < width; i++)
        {
            for (int j = 1; j < height; j++)//맨위에 0이있는경우는 생각안함
            {
                if (Board[j * width + i].name == '0')
                {
                    temp = Board[j * width + i];
                    Board[j * width + i] = Board[(j * (width) - width) + i];
                    Board[(j * (width) - width) + i] = temp;
                }
            }
        }
    }
    void InputChar(string s)//처음 문자열들을 입력한다
    {
        for (int i = 0; i < width; i++)
        {
            if (rimitNum < height)
                Board[i + rimitNum * width].name = s[i];
        }
        rimitNum++;//height보다 높으면안됨 제한걸어야함
        isCompareNow = true;
    }
}
