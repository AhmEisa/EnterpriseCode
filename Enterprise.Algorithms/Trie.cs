using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Enterprise.Algorithms
{
    public class Trie
    {
        private TrieNode head;

        public Trie()
        {
            head = new TrieNode();
        }

        /**
		 * Add a word to the trie.
		 * Adding is O (|A| * |W|), where A is the alphabet and W is the word being searched.
		 */
        public void AddWord(string word)
        {
            TrieNode curr = head;

            curr = curr.GetChild(word[0], true);

            for (int i = 1; i < word.Length; i++)
            {
                curr = curr.GetChild(word[i], true);
            }

            //curr.AddCount();
        }

        /**
		 * Get the count of a partictlar word.
		 * Retrieval is O (|A| * |W|), where A is the alphabet and W is the word being searched.
		 */
        public int GetCount(string word)
        {
            TrieNode curr = head;

            foreach (char c in word)
            {
                curr = curr.GetChild(c);

                if (curr == null)
                {
                    return 0;
                }
            }

            return curr.count;
        }
        public bool IsPrefix(string word)
        {
            TrieNode curr = head;
            curr = curr.GetChild(word[0]);
            if (curr == null) return false;

            for (int i = 1; i < word.Length; i++)
            {
                var currChilds = curr.Children;
                curr = currChilds.FirstOrDefault(x => x.data == word[i]);
                if (currChilds.Count > 0 && curr == null) return false;
                else if (currChilds.Count == 0) return true;
            }

            return true;
        }

        internal class TrieNode
        {
            private LinkedList<TrieNode> children;

            public int count { private set; get; }
            public char data { private set; get; }

            public TrieNode(char data = ' ')
            {
                this.data = data;
                count = 0;
                children = new LinkedList<TrieNode>();
            }

            public TrieNode GetChild(char c, bool createIfNotExist = false)
            {
                foreach (var child in children)
                {
                    if (child.data == c)
                    {
                        if (createIfNotExist) child.AddCount();
                        return child;
                    }
                }

                if (createIfNotExist)
                {
                    return CreateChild(c);
                }

                return null;
            }

            public int ChildCount => this.children.Count;
            public LinkedList<TrieNode> Children => this.children;
            public void AddCount()
            {
                count++;
            }

            public TrieNode CreateChild(char c)
            {
                var child = new TrieNode(c);
                child.AddCount();
                children.AddLast(child);

                return child;
            }
        }
    }

    public class ContactsUsingTrie
    {

        /*
		 * Complete the 'contacts' function below.
		 *
		 * The function is expected to return an INTEGER_ARRAY.
		 * The function accepts 2D_STRING_ARRAY queries as parameter.
		 */

        public static List<int> contacts(List<List<string>> queries)
        {
            var result = new List<int>();
            var currentList = new Trie();
            queries.ForEach(item =>
            {
                if (item[0].Equals("add"))
                    currentList.AddWord(item[1]);
                if (item[0].Equals("find"))
                    result.Add(currentList.GetCount(item[1]));
            });
            return result;
        }

    }
    public class NoPrefixTrie
    {

        /*
         * Complete the 'noPrefix' function below.
         *
         * The function accepts STRING_ARRAY words as parameter.
         */

        public static void noPrefix(List<string> words)
        {
            var currentList = new Trie();
            currentList.AddWord(words[0]);

            for (int i = 1; i < words.Count; i++)
            {
                if (currentList.IsPrefix(words[i]))
                {
                    Console.WriteLine("BAD SET");
                    Console.WriteLine(words[i]);
                    Debug.WriteLine("BAD SET");
                    Debug.WriteLine(words[i]);
                    return;
                }
                else { currentList.AddWord(words[i]); }
            }
            Console.WriteLine("GOOD SET");
            Debug.WriteLine("GOOD SET");
        }

    }
}
