using Enterprise.Algorithms;
using System.Collections.Generic;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class LinkedListsTests
    {
        [Fact]
        public void InsertNodeAtHead_Test()
        {
            var testData = new int[] { 383, 484, 392, 975, 321 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                SinglyLinkedListNode llist_head = LinkedListSolution.insertNodeAtHead(llist.head, testData[i]);
                llist.head = llist_head;
            }

            Assert.NotNull(llist.head);
            Assert.Equal(321, llist.head.data);
        }

        [Fact]
        public void InsertNodeAtPosition_Test()
        {
            var testData = new int[] { 16, 13, 7 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }
            SinglyLinkedListNode llist_head = LinkedListSolution.insertNodeAtPosition(llist.head, 1, 2);
            llist.head = llist_head;

            Assert.NotNull(llist.head);
            Assert.Equal(16, llist.head.data);
        }


        [Fact]
        public void DeleteNodeAtPosition_Test()
        {
            var testData = new int[] { 20, 6, 2, 19, 7, 4, 15, 9 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }
            SinglyLinkedListNode llist_head = LinkedListSolution.deleteNode(llist.head, 3);
            llist.head = llist_head;

            Assert.NotNull(llist.head);
            Assert.Equal(20, llist.head.data);
        }

        [Fact]
        public void PrintReverseList_Test()
        {
            var testData = new int[] { 16, 12, 4, 2, 5 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }
            var newList = LinkedListSolution.reversePrint2(llist.head);

            Assert.NotNull(newList);
            Assert.Equal(5, newList.data);
        }

        [Fact]
        public void MergeSortedList_Test()
        {
            var testData = new int[] { 1, 2, 3 };
            var testData2 = new int[] { 3, 4 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }
            SinglyLinkedList llist2 = new SinglyLinkedList();
            for (int i = 0; i < testData2.Length; i++)
            {
                llist2.InsertNode(testData2[i]);
            }
            var newList = LinkedListSolution.mergeLists(llist.head, llist2.head);

            Assert.NotNull(newList);
            Assert.Equal(1, newList.data);
        }

        [Fact]
        public void RemoveDuplicatesInList_Test()
        {
            var testData = new int[] { 1, 2, 2, 3, 4 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }

            var newList = LinkedListSolution.removeDuplicates(llist.head);

            Assert.NotNull(newList);
            Assert.Equal(1, newList.data);
        }

        [Fact]
        public void RemoveDuplicatesInList_Test2()
        {
            var testData = new int[] { 3, 3, 3, 4, 5, 5 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }

            var newList = LinkedListSolution.removeDuplicates(llist.head);

            Assert.NotNull(newList);
            Assert.Equal(3, newList.data);
        }


        [Fact]
        public void CycleCheckList_Test()
        {
            var testData = new int[] { 1, 2, 3 };
            SinglyLinkedList llist = new SinglyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }

            SinglyLinkedListNode extra = new SinglyLinkedListNode(-1);
            SinglyLinkedListNode temp = llist.head;
            int index = 1;
            int llistCount = testData.Length;

            for (int i = 0; i < llistCount; i++)
            {
                if (i == index)
                {
                    extra = temp;
                }

                if (i != llistCount - 1)
                {
                    temp = temp.next;
                }
            }

            temp.next = extra;

            var resul = LinkedListSolution.hasCycle(llist.head);

            Assert.True(resul);
        }

        [Fact]
        public void InsertIntoSortedDobuleLinkedList_Test()
        {
            var testData = new int[] { 1, 2, 4 };
            DoublyLinkedList llist = new DoublyLinkedList();
            for (int i = 0; i < testData.Length; i++)
            {
                llist.InsertNode(testData[i]);
            }

            var newList = LinkedListSolution.sortedInsert(llist.head, 3);

            Assert.NotNull(newList);
            Assert.Equal(1, newList.data);
        }
    }
}
