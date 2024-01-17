using System;
using System.Collections.Generic;
using System.IO;

namespace Enterprise.Algorithms
{
    public class SinglyLinkedListNode
    {
        public int data;
        public SinglyLinkedListNode next;

        public SinglyLinkedListNode(int nodeData)
        {
            this.data = nodeData;
            this.next = null;
        }
    }
    public class SinglyLinkedList
    {
        public SinglyLinkedListNode head;
        public SinglyLinkedListNode tail;

        public SinglyLinkedList()
        {
            this.head = null;
            this.tail = null;
        }

        public void InsertNode(int nodeData)
        {
            SinglyLinkedListNode node = new SinglyLinkedListNode(nodeData);

            if (this.head == null)
            {
                this.head = node;
            }
            else
            {
                this.tail.next = node;
            }

            this.tail = node;
        }
    }
    public class DoublyLinkedListNode
    {
        public int data;
        public DoublyLinkedListNode next;
        public DoublyLinkedListNode prev;

        public DoublyLinkedListNode(int nodeData)
        {
            this.data = nodeData;
            this.next = null;
            this.prev = null;
        }
    }

    public class DoublyLinkedList
    {
        public DoublyLinkedListNode head;
        public DoublyLinkedListNode tail;

        public DoublyLinkedList()
        {
            this.head = null;
            this.tail = null;
        }

        public void InsertNode(int nodeData)
        {
            DoublyLinkedListNode node = new DoublyLinkedListNode(nodeData);

            if (this.head == null)
            {
                this.head = node;
            }
            else
            {
                this.tail.next = node;
                node.prev = this.tail;
            }

            this.tail = node;
        }
    }

    public class LinkedListSolution
    {
        public static SinglyLinkedListNode insertNodeAtHead(SinglyLinkedListNode llist, int data)
        {
            var newNode = new SinglyLinkedListNode(data);
            if (llist == null) return newNode;

            newNode.next = llist;
            return newNode;
        }

        public static SinglyLinkedListNode insertNodeAtTail(SinglyLinkedListNode head, int data)
        {
            var newNode = new SinglyLinkedListNode(data);
            if (head == null) return newNode;
            var tail = head;
            while (tail.next != null)
            {
                tail = tail.next;
            }
            tail.next = newNode;
            return head;
        }
        public static SinglyLinkedListNode insertNodeAtPosition(SinglyLinkedListNode llist, int data, int position)
        {
            if (position == 0) return insertNodeAtHead(llist, data);

            SinglyLinkedListNode newNode = new SinglyLinkedListNode(data);
            int index = 0;
            SinglyLinkedListNode currentNode = llist;
            while (index < (position - 1) && currentNode != null)
            {
                currentNode = currentNode.next;
                index++;
            }
            if (currentNode.next == null) return insertNodeAtTail(llist, data);

            SinglyLinkedListNode nextNode = currentNode.next;
            currentNode.next = newNode;
            newNode.next = nextNode;
            return llist;
        }
        public static SinglyLinkedListNode deleteNode(SinglyLinkedListNode llist, int position)
        {
            if (llist == null) return llist;
            int index = 0;
            SinglyLinkedListNode currentNode = llist;
            while (index < (position - 1) && currentNode != null)
            {
                currentNode = currentNode.next;
                index++;
            }
            if (position == 0)
            {
                var newHeadNode = currentNode.next;
                llist = newHeadNode;
            }
            else
            {
                SinglyLinkedListNode nodeToDelete = currentNode.next;
                SinglyLinkedListNode nextNode = nodeToDelete.next != null ? nodeToDelete.next : null;
                currentNode.next = nextNode;
            }
            return llist;
        }
        static bool CompareLists(SinglyLinkedListNode head1, SinglyLinkedListNode head2)
        {
            bool areListsEqual = true;
            while (true)
            {
                if (head1 == null && head2 == null) break;
                if ((head1 == null && head2 != null) || (head1 != null && head2 == null) || (head1.data != head2.data)) { areListsEqual = false; break; }
                head1 = head1.next;
                head2 = head2.next;
            }
            return areListsEqual;
        }
        public static SinglyLinkedListNode mergeLists(SinglyLinkedListNode head1, SinglyLinkedListNode head2)
        {
            SinglyLinkedListNode newList = null;

            if (head1 != null && head2 == null) return head1;
            if (head1 == null && head2 != null) return head2;

            while (true)
            {
                if (head1 == null && head2 == null) break;
                if (head2 == null && head1 != null)
                {
                    newList = insertNodeAtTail(newList, head1.data);
                    head1 = head1 != null ? head1.next : null;
                }
                else if (head1 == null && head2 != null)
                {
                    newList = insertNodeAtTail(newList, head2.data);
                    head2 = head2 != null ? head2.next : null;
                }
                else if (head1.data <= head2.data)
                {
                    newList = insertNodeAtTail(newList, head1.data);
                    head1 = head1 != null ? head1.next : null;
                }
                else if (head1.data > head2.data)
                {
                    newList = insertNodeAtTail(newList, head2.data);
                    head2 = head2 != null ? head2.next : null;
                }
            }

            return newList;
        }
        public static SinglyLinkedListNode removeDuplicates(SinglyLinkedListNode llist)
        {
            if (llist == null) return llist;
            var tempList = llist;
            while (tempList != null && tempList.next != null)
            {
                var nextNode = tempList.next;
                while (nextNode != null && tempList.data == nextNode.data)
                {
                    tempList.next = null;
                    nextNode = nextNode.next;
                    tempList.next = nextNode;
                }
                tempList = tempList.next;
            }
            return llist;
        }
        public static bool hasCycle(SinglyLinkedListNode head)
        {
            var list = new Dictionary<SinglyLinkedListNode, bool>();
            while (head != null)
            {
                if (list.ContainsKey(head))
                    return true;
                else list[head] = true;

                head = head.next;
            }
            return false;
        }
        public static int findMergeNode(SinglyLinkedListNode head1, SinglyLinkedListNode head2)
        {
            var head1Next = head1;
            while (head1Next != null)
            {
                var head2Next = head2;
                while (head2Next != null)
                {
                    if (head1Next == head2Next) return head1Next.data;
                    head2Next = head2Next.next;
                }
                head1Next = head1Next.next;
            }


            return -1;
        }
        public static DoublyLinkedListNode sortedInsert(DoublyLinkedListNode llist, int data)
        {
            var node = new DoublyLinkedListNode(data);
            if (llist == null) return node;

            var tempList = llist;
            var currentNode = tempList;
            while (tempList != null && data >= tempList.data)
            {
                currentNode = tempList;
                tempList = tempList.next;
            }

            if (currentNode.next == null)//insert at end
            {
                currentNode.next = node;
                node.prev = currentNode;
            }
            else if (tempList == llist)//insert at start
            {
                tempList.prev = node;
                node.next = tempList;
                llist = node;
            }
            else
            {
                var nextNode = currentNode.next;
                currentNode.next = node;
                node.prev = currentNode;
                node.next = nextNode;
                nextNode.prev = node;
            }
            return llist;
        }
        public static DoublyLinkedListNode reverse(DoublyLinkedListNode llist)
        {
            if (llist == null) return llist;
            var tlist = new DoublyLinkedListNode(llist.data);
            while (llist.next != null)
            {
                var newNode = new DoublyLinkedListNode(llist.next.data);
                newNode.next = tlist;
                tlist.prev = newNode;
                tlist = newNode;
                llist = llist.next;
            }
            return tlist;
        }
        public static int getNode(SinglyLinkedListNode llist, int positionFromTail)
        {
            if (llist == null) return -1;

            SinglyLinkedListNode newList = GetReveresedList(llist);

            int index = 0;
            SinglyLinkedListNode currentNode = newList;
            while (index < positionFromTail && currentNode != null)
            {
                currentNode = currentNode.next;
                index++;
            }
            return currentNode.data;
        }
        // Complete the printLinkedList function below.
        public static void reversePrint(SinglyLinkedListNode llist)
        {
            if (llist == null) return;
            var newNode = GetReveresedList(llist);
            printLinkedList(newNode);
        }
        public static SinglyLinkedListNode reversePrint2(SinglyLinkedListNode llist)
        {
            if (llist == null) return null;
            var newNode = GetReveresedList(llist);
            return newNode;
        }
        static SinglyLinkedListNode GetReveresedList(SinglyLinkedListNode llist)
        {
            SinglyLinkedListNode newNode = null;
            while (llist != null)
            {
                newNode = insertNodeAtHead(newNode, llist.data);
                llist = llist.next;
            }
            return newNode;
        }

        static void printLinkedList(SinglyLinkedListNode head)
        {
            while (head != null)
            {
                Console.WriteLine(head.data);
                head = head.next;
            }
        }
        static void PrintSinglyLinkedList(SinglyLinkedListNode node, string sep, TextWriter textWriter)
        {
            while (node != null)
            {
                textWriter.Write(node.data);

                node = node.next;

                if (node != null)
                {
                    textWriter.Write(sep);
                }
            }
        }
        static void PrintDoublyLinkedList(DoublyLinkedListNode node, string sep, TextWriter textWriter)
        {
            while (node != null)
            {
                textWriter.Write(node.data);

                node = node.next;

                if (node != null)
                {
                    textWriter.Write(sep);
                }
            }
        }
        static void Main(string[] args)
        {
            SinglyLinkedList llist = new SinglyLinkedList();

            int llistCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < llistCount; i++)
            {
                int llistItem = Convert.ToInt32(Console.ReadLine());
                llist.InsertNode(llistItem);
            }

            printLinkedList(llist.head);
        }
        static void MainAddToTail(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            SinglyLinkedList llist = new SinglyLinkedList();

            int llistCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < llistCount; i++)
            {
                int llistItem = Convert.ToInt32(Console.ReadLine());
                SinglyLinkedListNode llist_head = insertNodeAtTail(llist.head, llistItem);
                llist.head = llist_head;

            }



            PrintSinglyLinkedList(llist.head, "\n", textWriter);
            textWriter.WriteLine();

            textWriter.Flush();
            textWriter.Close();
        }
        static void MainAddToHead(string[] args)
        {
            TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            SinglyLinkedList llist = new SinglyLinkedList();

            int llistCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < llistCount; i++)
            {
                int llistItem = Convert.ToInt32(Console.ReadLine());
                SinglyLinkedListNode llist_head = insertNodeAtHead(llist.head, llistItem);
                llist.head = llist_head;
            }



            PrintSinglyLinkedList(llist.head, "\n", textWriter);
            textWriter.WriteLine();

            textWriter.Flush();
            textWriter.Close();
        }
        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int val = 0, ListNode next = null)
            {
                this.val = val;
                this.next = next;
            }
        }
        public static ListNode SwapPairs(ListNode head)
        {
            if (head == null || head.next == null) return head;
            ListNode tail = SwapPairs(head.next.next);

            ListNode newHead = head.next;
            head.next = tail;
            newHead.next = head;
            return newHead;
        }
        public ListNode ReverseList(ListNode head)
        {
            if (head == null || head.next == null) return head;
            ListNode newHead = ReverseList(head.next);
            head.next.next = head;
            head.next = null;
            return newHead;
        }
        public ListNode InsertionSortList(ListNode head)
        {
            if (head == null) { return head; }
            ListNode sortedHead = head;
            head = head.next;
            while (head != null)
            {
                ListNode tempSortedHead = sortedHead;
                ListNode prevNode = null;
                while (tempSortedHead != null && head.val <= tempSortedHead.val)
                {
                    prevNode = tempSortedHead;
                    tempSortedHead = tempSortedHead.next;
                }
                ListNode tempHead = head;
                tempHead.next = tempSortedHead;
                if (prevNode == null) { sortedHead = tempHead; }
                else { prevNode.next = tempHead; }

                head = head.next;
            }
            return sortedHead;
        }
    }
}

