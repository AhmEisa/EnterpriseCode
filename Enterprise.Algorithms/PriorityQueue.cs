using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    public class Heap
    {
        public void Insert((int element, int priority)[] pairs, int element, int priority)
        {
            var newElement = (element, priority);
            pairs.Append(newElement);
            OptimizedBubbleUp(pairs, pairs.Length - 1);
        }
        public (int element, int priority) Top((int element, int priority)[] pairs)
        {
            if (pairs.Length == 0) throw new ArgumentException("Array is empty");
            var lastElement = pairs.LastOrDefault();
            if (pairs.Length == 1 && lastElement.element > 0) return lastElement;
            var topElement = pairs.FirstOrDefault();
            pairs[0] = lastElement;
            this.OptimizedPushDown(pairs, 0);
            return topElement;
        }
        public (int element, int priority) Peek((int element, int priority)[] pairs)
        {
            if (pairs.Length == 0) throw new ArgumentException("Array is empty");
            return pairs.FirstOrDefault();
        }
        public void Update((int element, int priority)[] pairs, int oldElement, int newPriority)
        {
            int index = Array.FindIndex(pairs, x => x.element == oldElement);
            int oldPriority = pairs[0].priority;
            if (index >= 0)
            {
                oldPriority = pairs[index].priority;
                pairs[index] = (oldElement, newPriority);
            }
            if (newPriority < oldPriority)
                this.OptimizedBubbleUp(pairs, index);
            else if (newPriority > oldPriority)
                this.OptimizedPushDown(pairs, index);
        }
        public void Heapify((int element, int priority)[] pairs, int branchFactor)
        {
            for (int index = 0; index < (pairs.Length - 1) / branchFactor; index++)
                this.OptimizedPushDown(pairs, index);
        }
        public bool Contains(int element)
        {
            int index = ElementToIndex(element);
            return index >= 0;
        }
        private int ElementToIndex(int element)
        {
            throw new NotImplementedException();
        }
        public void BubbleUp((int element, int priority)[] pairs, int index)//by default last index
        {
            int parentIndex = index;
            int currentIndex = 0;
            while (parentIndex > 0)
            {
                currentIndex = parentIndex;
                parentIndex = GetParentIndex(parentIndex, 2);
                if (pairs[parentIndex].priority < pairs[currentIndex].priority)
                    Swap(pairs, currentIndex, parentIndex);
                else break;
            }
        }
        public void OptimizedBubbleUp((int element, int priority)[] pairs, int index)
        {
            var currentItem = pairs[index];
            while (index > 0)
            {
                int parentIndex = GetParentIndex(index, 2);
                if (pairs[parentIndex].priority < currentItem.priority)
                {
                    pairs[index] = pairs[parentIndex];
                    index = parentIndex;
                }
                else break;
            }
            pairs[index] = currentItem;
        }

        public void PushDown((int element, int priority)[] pairs, int index) //start at 0
        {
            int currentIndex = index;
            while (currentIndex < FirstLeafIndex(pairs, 2))
            {
                var selectedChild = HighestPriorityChild(currentIndex);
                if (selectedChild.child.priority > pairs[currentIndex].priority)
                {
                    Swap(pairs, currentIndex, selectedChild.childIndex);
                    currentIndex = selectedChild.childIndex;
                }
                else break;
            }
        }
        public void OptimizedPushDown((int element, int priority)[] pairs, int index) //start at 0
        {
            var current = pairs[index];
            while (index < FirstLeafIndex(pairs, 2))
            {
                var selectedChild = HighestPriorityChild(index);
                if (selectedChild.child.priority > current.priority)
                {
                    pairs[index] = pairs[selectedChild.childIndex];
                    index = selectedChild.childIndex;
                }
                else break;
            }
            pairs[index] = current;
        }

        private ((int element, int priority) child, int childIndex) HighestPriorityChild(int currentIndex)
        {
            throw new NotImplementedException();
        }

        private int FirstLeafIndex((int element, int priority)[] pairs, int branchFactor)
        {
            return (pairs.Length - 2) / branchFactor + 1;
        }

        private void Swap((int element, int priority)[] pairs, int currentIndex, int parentIndex)
        {
            var temp = pairs[currentIndex];
            pairs[parentIndex] = temp;
            pairs[currentIndex] = pairs[parentIndex];
        }
        private int GetParentIndex(int currentIndex, int branchFactor)
        {
            return (currentIndex - 1) / branchFactor;
        }

        //cases
        public void FindTopK((int element, int priority)[] pairs, int k)
        {
            //create an empty min heap
            Heapify(pairs, 2);
            //for each elment elm in pairs do
            //if heap size == k and heap peek < elm
            //heap.top
            // if heap size < k then heap insert elm
            //return heap
        }

    }
}
