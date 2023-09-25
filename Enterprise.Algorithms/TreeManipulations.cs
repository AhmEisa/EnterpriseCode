using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Algorithms
{
    /***/
    //Definition for a binary tree node.
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }

    public class Trees
    {
        public IList<int> PreorderTraversal(TreeNode root)
        {
            List<int> result = new List<int>();
            Preorder(root, result);
            //Follow up: Recursive solution is trivial, could you do it iteratively?
            ItPreorder(root, result);
            return result;
        }
        private void Preorder(TreeNode root, List<int> result)
        {
            if (root == null) return;
            result.Add(root.val);
            Preorder(root.left, result);
            Preorder(root.right, result);
        }
        private void ItPreorder(TreeNode root, List<int> result)
        {
            if (root == null) return;
            Stack<TreeNode> nodes = new Stack<TreeNode>();
            nodes.Push(root);
            TreeNode topNode;
            while (nodes.Count > 0)
            {
                topNode = nodes.Pop();
                result.Add(topNode.val);
                if (topNode.right != null)
                    nodes.Push(topNode.right);
                if (topNode.left != null)
                    nodes.Push(topNode.left);
            }
        }
        public IList<int> InorderTraversal(TreeNode root)
        {
            List<int> result = new List<int>();
            //Follow up: Recursive solution is trivial, could you do it iteratively?
            Inorder(root, result);
            return result;
        }
        private void Inorder(TreeNode root, List<int> result)
        {
            if (root == null) return;
            Inorder(root.left, result);
            result.Add(root.val);
            Inorder(root.right, result);
        }
        public IList<int> PostorderTraversal(TreeNode root)
        {
            List<int> result = new List<int>();
            //Follow up: Recursive solution is trivial, could you do it iteratively?
            Postorder(root, result);
            return result;
        }
        private void Postorder(TreeNode root, List<int> result)
        {
            if (root == null) return;
            Postorder(root.left, result);
            Postorder(root.right, result);
            result.Add(root.val);
        }
        public IList<IList<int>> LevelOrder(TreeNode root)
        {
            var result = new List<IList<int>>();
            Levelorder(root, result);
            return result;
        }
        private void Levelorder(TreeNode root, IList<IList<int>> result)
        {
            if (root == null) return;
            Queue<TreeNode> nodes = new Queue<TreeNode>();
            nodes.Enqueue(root);
            int currSize;
            TreeNode curr;
            while (nodes.Count > 0)
            {
                var levelResult = new List<int>();
                currSize = nodes.Count;
                for (int i = 0; i < currSize; i++)
                {
                    curr = nodes.Dequeue();
                    levelResult.Add(curr.val);
                    if (curr.left != null)
                        nodes.Enqueue(curr.left);
                    if (curr.right != null)
                        nodes.Enqueue(curr.right);
                }
                result.Add(levelResult);
            }
        }

        public int MaxDepth(TreeNode root)
        {
            if (root == null) { return 0; }
            int leftAns = MaxDepth(root.left);
            int rightAns = MaxDepth(root.right);
            return Math.Max(leftAns, rightAns) + 1;
        }
        public bool IsSymmetric(TreeNode root)
        {
            return IsMirror(root.left, root.right);
        }
        public bool ItIsSymmetric(TreeNode root)
        {
            Queue<TreeNode> nodes = new Queue<TreeNode>();
            nodes.Enqueue(root.left);
            nodes.Enqueue(root.right);
            while (nodes.Count > 0)
            {
                var leftTree = nodes.Dequeue();
                var rightTree = nodes.Dequeue();
                if (leftTree == null && rightTree == null) { continue; }
                if (leftTree == null || rightTree == null) { return false; }
                if(leftTree.val == rightTree.val) { return false; }
                nodes.Enqueue(leftTree.left);
                nodes.Enqueue(rightTree.right);
                nodes.Enqueue(leftTree.right);
                nodes.Enqueue(rightTree.left);
            }
            return true;
        }
        private bool IsMirror(TreeNode leftTree, TreeNode rightTree)
        {
            if (leftTree == null && rightTree == null) { return true; }
            if (leftTree == null || rightTree == null) { return false; }
            return leftTree.val == rightTree.val && IsMirror(leftTree.left, rightTree.right) && IsMirror(leftTree.right, rightTree.left);
        }
        public bool HasPathSum(TreeNode root, int targetSum)
        {

        }
        public int CountUnivalSubtrees(TreeNode root)
        {

        }
    }
}
