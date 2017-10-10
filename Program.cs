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
    //리스트의 상태
    enum state{
        EMPTY=0,//비어있음
        USED,//데이터있음
        FULL//꽉참
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
            if(Count==5)
                TableState=state.FULL;
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
                Console.WriteLine("[탐색실패:삭제불가]");
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
    class HashtableH{
        private LinkedListH[] buket;
        private int hashCount;//해싱 2번의 제한을 주기위해
        public int NumOfBuket{get;set;}
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
                        
                    if(doubleHashCode!=hashcode&&buket[doubleHashCode].TableState!=state.FULL)
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
                Console.WriteLine("탐색실패:탐색할 데이터가 없습니다");
                return null;
            }
            else
            {
                Console.Write("탐색성공:");
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
        public string SearchToValue(int key,string value)//변경 키에값은있으나 입력하는값이 틀리는 경우 나눠야함
        {
            if(buket[key].TableState!=state.EMPTY)//키에 값있을때
            {
                if(buket[key].SearchList(value)!=null)//값을 탐색 성공했을때
                {
                    //Console.WriteLine("탐색성공:");
                    return buket[key].SearchList(value);
                }
                else
                {
                    Console.WriteLine("탐색실패:키에 입력된 값을 찾지 못했습니다");
                    return null;
                }
            }
            else//키에 값 없을때
            {
                Console.WriteLine("탐색실패:해당 키에 값이 없습니다");
                return null;
            }
        }
        public string Delete(int key,string value)//변경 
        {
            Node rpos=new Node();
            rpos.Name=value;
            if(buket[key].TableState!=state.EMPTY)
            {
                buket[key].DeleteList(SearchToValue(key,value));
                if(buket[key].TableState==state.EMPTY)
                    NumOfBuket--;
                return rpos.Name;
            }
            else
            {
                buket[key].DeleteList(SearchToValue(key,value));
                return null;
            }
        }
        public void DeleteAll(int key)
        {
            if(buket[key].TableState!=state.EMPTY)
            {
                buket[key].ClearList();
                NumOfBuket--;
                Console.WriteLine($"****{key}번째 테이블 삭제****");
            }
            else
            {
                Console.WriteLine("삭제하려는 테이블에 데이터가 없습니다");
            }
        }
        public void Clear()
        {
            for(int i=0; i<100; i++)
            {
                buket[i].ClearList();
            }
            NumOfBuket=0;
            Console.WriteLine("테이블 초기화");
        }
        public HashtableH Clone()
        {
            return this;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            HashtableH hashtable=new HashtableH();

            //1.데이터 삽입:Add(string key,string value)=>데이터 30개 삽입(이중해싱은 2번까지밖에 허용하지 않아 황도영16은 저장할수없다)
            hashtable.Add("hello","황도영1");hashtable.Add("hello","황도영2");
            hashtable.Add("hello","황도영3");hashtable.Add("hello","황도영4");
            hashtable.Add("hello","황도영5");hashtable.Add("hello","황도영6");
            hashtable.Add("hello","황도영7");hashtable.Add("hello","황도영8");
            hashtable.Add("hello","황도영9");hashtable.Add("hello","황도영10");
            hashtable.Add("hello","황도영11");hashtable.Add("hello","황도영12");
            hashtable.Add("hello","황도영13");hashtable.Add("hello","황도영14");
            hashtable.Add("hello","황도영15");hashtable.Add("hello","황도영16");
            hashtable.Add("world","황도영17");hashtable.Add("world","황도영18");
            hashtable.Add("world","황도영19");hashtable.Add("world","황도영20");
            hashtable.Add("world","황도영21");hashtable.Add("world","황도영22");
            hashtable.Add("world","황도영23");hashtable.Add("world","황도영24");
            hashtable.Add("animal","황도영25");hashtable.Add("animal","황도영26");
            hashtable.Add("animal","황도영27");hashtable.Add("animal","황도영28");
            hashtable.Add("animal","황도영29");hashtable.Add("animal","황도영30");
            
            //2.데이터 출력:SearchAll()=>전체출력
            hashtable.SearchAll();

            //3.현재 테이블의 수 출력:Count
            Console.WriteLine($"현재 테이블의 수:{hashtable.NumOfBuket}");
            Console.WriteLine("\r\n");

            //4.데이터 삭제:Delete(int key,string value)=>키에 해당하는 벨류값을 찾아서 삭제
            int inputNum;
            string inputValue;
            Console.Write("삭제할 데이터의 키값 입력:");
            inputNum=int.Parse(Console.ReadLine());
            Console.Write("삭제할 데이터의 벨류값 입력:");
            inputValue=Console.ReadLine();
            hashtable.Delete(inputNum,inputValue);
            hashtable.SearchAll();
            Console.WriteLine($"현재 테이블의 수:{hashtable.NumOfBuket}");
            Console.WriteLine("\r\n");

            //5.데이터 삭제:DeleteAll(int key)=>입력한 키값에 해당하는 테이블 삭제
            Console.Write("삭제할 테이블의 키값 입력:");
            inputNum=int.Parse(Console.ReadLine());
            hashtable.DeleteAll(inputNum);
            hashtable.SearchAll();
            Console.WriteLine($"현재 테이블의 수:{hashtable.NumOfBuket}");
            Console.WriteLine("\r\n");
            
            //6.데이터 탐색:Search(int key)=>키에 해당하는 헤드 정보를 나타냄
            Console.Write("탐색할 데이터의 키값 입력(헤드값출력):");
            inputNum=int.Parse(Console.ReadLine());
            Console.WriteLine($"[{hashtable.Search(inputNum)}]");
            Console.WriteLine("\r\n");

            //7.데이터 탐색:SearchToValue(int key,string value)=>키에 해당하는 벨류값을 탐색
            Console.Write("탐색할 데이터의 키값 입력:");
            inputNum=int.Parse(Console.ReadLine());
            Console.Write("탐색할 데이터의 벨류값 입력:");
            inputValue=Console.ReadLine();
            Console.WriteLine($"[{hashtable.SearchToValue(inputNum,inputValue)}]");
            Console.WriteLine("\r\n");

            //8.테이블 복사:Clone()
            Console.WriteLine("테이블 복사를 하려면 아무키나 누르시오");
            Console.ReadLine();
            HashtableH hashtable2=new HashtableH();
            hashtable2=hashtable.Clone();
            Console.WriteLine("****복사된 테이블의 데이터 출력****");
            hashtable2.SearchAll();
            Console.WriteLine($"현재 테이블의 수:{hashtable2.NumOfBuket}");
            Console.WriteLine("\r\n");

            //9.테이블 초기화:Clear()=>모든테이블을 초기화한다
            Console.WriteLine("테이블 초기화를 하려면 아무키나 누르시오");
            Console.ReadLine();
            hashtable.Clear();
            hashtable.SearchAll();
            Console.WriteLine($"현재 테이블의 수:{hashtable.NumOfBuket}");
               
        }
    }
}