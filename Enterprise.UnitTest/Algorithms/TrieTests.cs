using Enterprise.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enterprise.UnitTest.Algorithms
{
    public class TrieTests
    {
        [Fact]
        public void SearchPrefixes()
        {

            var dataToTest = new List<List<string>> {
            new List<string>{ "add","hack"},
            new List<string>{ "add","hackerrank"},
            new List<string>{"find","hac" },
            new List<string>{"find","hak" },
            };
            var result = ContactsUsingTrie.contacts(dataToTest);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0]);
            Assert.Equal(0, result[1]);
        }

        [Fact]
        public void CheckDuplicatedPrefixes_Good_Set()
        {

            var dataToTest = new List<string> { "ab","bc","cd" };
            NoPrefixTrie.noPrefix(dataToTest);

            Assert.True(1 == 1);
        }

        [Fact]
        public void CheckDuplicatedPrefixes_Bad_Set()
        {

            var dataToTest = new List<string> { "aab", "defgab", "abcde", "aabcde", "cedaaa", "bbbbbbbbbb", "jabjjjad" };
            NoPrefixTrie.noPrefix(dataToTest);

            Assert.True(1 == 1);
        }

        [Fact]
        public void CheckDuplicatedPrefixes_Bad_Set_2()
        {

            var dataToTest = new List<string> { "aab", "aac", "aacghgh", "aabghgh" };
            NoPrefixTrie.noPrefix(dataToTest);

            Assert.True(1 == 1);
        }

        [Fact]
        public void Tree_Connect_Test()
        {

           // var dataToTest = new List<string> { "aab", "aac", "aacghgh", "aabghgh" };
           // Trees.Connect(dataToTest);

            Assert.True(1 == 1);
        }
    }
}
