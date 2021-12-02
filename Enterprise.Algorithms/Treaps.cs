using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    public class Node
    {
        public string Key { get; set; }
        public double Priority { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Parent { get; set; }
        public Node(string key, double priority)
        {
            this.Key = key;
            this.Priority = priority;
            this.Left = null;
            this.Right = null;
            this.Parent = null;
        }
        public Node(string key, double priority, Node left, Node right)
        {
            this.Key = key;
            this.Priority = priority;
            this.Left = left;
            this.Right = right;
            left.Parent = this;
            right.Parent = this;
        }
        public void SetLeft(Node node)
        {
            if (node != null)
            {
                this.Left = node;
                node.Parent = this;
            }

        }
        public void SetRight(Node node)
        {
            if (node != null)
            {
                this.Right = node;
                node.Parent = this;
            }

        }
    }

    public class Treap
    {
        public Node Root { get; set; }
        public Treap()
        {
            this.Root = null;
        }

    }
}
