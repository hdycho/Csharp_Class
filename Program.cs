using System;
using System.Collections.Generic;
using System.Collections;

namespace HashTableProj
{
    public static class ExtensionMethod{
        public static int GetHeshCodeH(this string st)
        {
            int hashcode=0;
            for(int i=0; i<st.Length; i++)
            {
                hashcode+=Convert.ToInt16(st[i]);
            }
            hashcode=hashcode%100;
            return hashcode;
        }

        public static int Hashing(this int hCode)
        {
            hCode=(hCode%47)+1;
            return hCode;
        }
    }
    public class Node{
        public string Name{get;set;}
        public Node next;
        
    }
    class LinkedListH{
        public Node head;
        private Node tail;
        private Node cur;
        private Node before;
        public state  TableState;
        public int Key{get;set;}
        public int Count{get;set;}

        public LinkedListH()//생성자 메서드에서 초기화 
        {
            head=null;
            tail=null;
            cur=null;
            before=null;
            Count=0;
            TableState=state.EMPTY;
            
        }
        public void ClearList()
        {
            head=null;
            tail=null;
            cur=null;
            before=null;
            Count=0;
            TableState=state.EMPTY;
        }
        public void InsertList(string inputName)//데이터 넣으면 뒤로가는 형식
        {
            Node pnode=new Node();
            pnode.Name=inputName;
            pnode.next=null;

            if(head==null) //데이터가 없을때(head==null일때)
            {
                head=pnode;
                tail=pnode;
            }
            else//데이터 있을때(head!=null일때)
            {
                tail.next=pnode;
                tail=pnode;
            }
            Count++;
            TableState=state.USED;
        }
        public string SearchList(string searchName)//찾을 데이터를 앞에서부터 탐색(삭제에서 사용하기위해 만듬)
        {
            cur=head;
            before=null;
           
            while(cur!=null)
            {
                if(cur.Name==searchName)
                {
                    break;
                }
                before=cur;
                cur=cur.next;
            }
            if(cur==null)
            {
                return null;    
            }
            else
            {
                return cur.Name;
            }
        }
        public void DeleteList(string deleteName)//특정 데이터 삭제
        {
            if(SearchList(deleteName)!=null)
            {
                if(head.next==null)//데이터 한개일때
                {
                    head=null;
                    cur=null;
                }
                else//데이터 여러개일때
                {
                    if(cur==head)//머리일때
                    {
                        head=head.next;
                        cur=null;
                    }
                    else if(cur==tail)//꼬리일때
                    {
                        tail=before;
                        tail.next=null;
                        cur=null;
                    }
                    else//둘다아닐때
                    {
                        before.next=cur.next;
                        cur=null;
                        cur=before.next;
                    }
                }
                Count--;
                if(Count==0)
                    TableState=state.EMPTY;
            }
            else
            {
                Console.WriteLine("탐색실패:삭제불가");
            }
        }
        public void SearchListAll()
        {
            Node sCur=new Node();
            sCur=head;
            if(Count!=0)
            {
                for(int i=0; i<Count; i++)
                {
                    if(i==0)
                        Console.Write($"({sCur.Name})---->");
                    else if(i==4)
                        Console.Write($"{sCur.Name}  ");
                    else
                        Console.Write($"{sCur.Name}---->");
                    sCur=sCur.next;
                }
                Console.WriteLine("[Key번호:{0}]",Key);
            }
        }
    }

    enum state{
        EMPTY=0,//비어있음
        USED,//데이터있음
        FULL
    }
    class HashtableH{
        private LinkedListH[] buket;
        private int hashCount;
        public int NumOfBuket{get;set;}//구현하기
        public HashtableH()
        {
            buket=new LinkedListH[100];
            for(int i=0; i<100; i++)
            {
                buket[i]=new LinkedListH();
            }
            hashCount=0;
        }
        public void Add(string key,string value)
        {
            int hashcode=key.GetHeshCodeH();
            int doubleHashCode=0;
            if(buket[hashcode].Count<5)
            {
                buket[hashcode].Key=hashcode;
                buket[hashcode].InsertList(value);
                if(buket[hashcode].Count==1)
                    NumOfBuket++;
            }
            else
            {
                while(hashCount!=2)
                {
                    hashCount++;
                    if(hashCount==1)
                        doubleHashCode=hashcode.Hashing();
                    else if(hashCount==2)
                        doubleHashCode=doubleHashCode.Hashing();
                        
                    if(doubleHashCode!=hashcode&&buket[doubleHashCode].Count<5)
                    {
                        hashCount=0;
                        buket[doubleHashCode].Key=doubleHashCode;
                        buket[doubleHashCode].InsertList(value);
                        if(buket[doubleHashCode].Count==1)
                            NumOfBuket++;
                        break;
                    }
                }
                if(hashCount==2)
                {
                    hashCount=0;
                    Console.WriteLine($"***{value} 삽입불가***");
                }
            }
        }
        public string Search(int key)
        {
            if(buket[key].TableState==state.EMPTY)
            {   
                Console.Write("탐색할 데이터가 없습니다");
                return null;
            }
            else
            {
                return buket[key].head.Name;
            }
        }
        public void SearchAll()
        {   
            for(int i=0; i<100; i++)
            {        
                buket[i].SearchListAll();
            }
        }
        public string SearchToName(int key,string value)
        {
            if(buket[key].TableState!=state.EMPTY)//키에 값있을때
                return buket[key].SearchList(value);
            else//키에 값 없을때
            {
                Console.Write("해당 키에 값이 없습니다=>");
                return null;
            }
        }
        public string Delete(int index,string value)
        {
            Node rpos=new Node();
            rpos.Name=value;
            if(buket[index].TableState!=state.EMPTY)
            {
                buket[index].DeleteList(SearchToName(index,value));
                if(buket[index].TableState==state.EMPTY)
                    NumOfBuket--;
                return rpos.Name;
            }
            else
            {
                buket[index].DeleteList(SearchToName(index,value));
                return null;
            }
        }
        public void Clear()
        {
            for(int i=0; i<10; i++)
            {
                buket[i].ClearList();
            }
            NumOfBuket=0;
            Console.WriteLine("테이블 초기화");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int selNum=0;
            HashtableH hs =new HashtableH();
            hs.Add("hdy","황도영1");
            hs.Add("hdy","황도영2");
            hs.Add("hdy","황도영3");
            hs.Add("hdy","황도영4");
            hs.Add("hdy","황도영5");
            hs.Add("hdy","황도영6");
            hs.Add("hdy","황도영7");
            hs.Add("hdy","황도영8");
            hs.Add("hdy","황도영9");
            hs.Add("hdy","황도영10");
            hs.Add("hdy","황도영11");
            hs.Add("hdy","황도영12");
            hs.Add("hdy","황도영13");
            hs.Add("hdy","황도영14");
            hs.Add("hdy","황도영15");
            hs.Add("hdy","황도영16");
            hs.Add("hdy","황도영17");
            hs.Add("hdy","황도영18");

            hs.SearchAll();
            Console.WriteLine($"현재 테이블 갯수{hs.NumOfBuket}");
            Console.Write("탐색할 키를 입력하시오:");
            selNum=int.Parse(Console.ReadLine());
            Console.WriteLine($"{selNum}번째 테이블값:[{hs.Search(selNum)}]");

            
               
        }
    }
}