using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Enterprise.Try
{
    public class CodePerformance
    {
        public static void Test()
        {
            int shift = 5;
            string output = "";
            Console.Write("Input: ");
            string input = Console.ReadLine();
            if (input != null)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] < 65 || input[i] > 90)
                    {
                        throw new Exception("Only A-Z supported.");
                    }
                    int shifted = input[i] + shift;
                    if (shifted > 90)
                    {
                        shifted = 65 + shifted - 91;
                    }
                    output = output + (char)shifted;
                }
            }
            Console.WriteLine("Output: " + output);
        }

        public static string ShiftStringCharacters(string input, int numberOfShifts, int startCharValue, int endCharValue)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            if (input.Any(c => c < startCharValue || c > endCharValue))
                throw new Exception($"Only {(char)startCharValue}-{(char)endCharValue}  supported.");

            StringBuilder output = new StringBuilder();
            Dictionary<char, char> cachedChars = new Dictionary<char, char>();
            
            for (int i = 0; i < input.Length; i++)
            {
                //if (input[i] < startCharValue || input[i] > endCharValue)
                //{
                //    throw new Exception($"Only {(char)startCharValue}-{(char)endCharValue}  supported.");
                //}
                if (cachedChars.TryGetValue(input[i], out char cachedChar))
                {
                    output.Append(cachedChar);
                    continue;
                }

                int shifted = input[i] + numberOfShifts;
                if (shifted > endCharValue)
                {
                    shifted = startCharValue + shifted - (endCharValue + 1);
                }

                output.Append(cachedChars[input[i]] = (char)shifted);
            }

            return output.ToString();
        }
    }
    public static class CharactersUtility
    {
        public static char ShiftedBy(this char charToBeShifted, int numberOfShifts)
        {
            int A = 65;
            int Z = 90;
            int baseRound = 91;
            if (charToBeShifted < A || charToBeShifted > Z)
            {
                throw new Exception("Only A-Z supported.");
            }

            int shifted = charToBeShifted + numberOfShifts;
            if (shifted > Z)
            {
                shifted = A + shifted - baseRound;
            }
            return (char)shifted;
        }
    }
    public class DriverExam
    {
        public static void ExecuteExercise(IExercise exercise)
        {
            try
            {
                exercise.Start();
                exercise.Execute();
            }
            catch
            {
                exercise.MarkNegativePoints();
            }
            finally
            {
                exercise.End();
            }
        }
    }

    public interface IExercise
    {
        void Start();
        void Execute();
        void MarkNegativePoints();
        void End();
    }

    public class Exercise : IExercise
    {
        public void Start() { Console.WriteLine("Start"); }
        public void Execute() { Console.WriteLine("Execute"); }
        public void MarkNegativePoints() { Console.WriteLine("MarkNegativePoints"); }
        public void End() { Console.WriteLine("End"); }
    }

    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        DriverExam.ExecuteExercise(new Exercise());
    //    }
    //}


    public class DocumentStore
    {
        private readonly List<string> documents = new List<string>();
        private readonly int capacity;

        public DocumentStore(int capacity)
        {
            this.capacity = capacity;
        }

        public int Capacity { get { return this.capacity; } }

        public IReadOnlyList<string> Documents { get { return documents.AsReadOnly(); } }

        public void AddDocument(string document)
        {
            if (documents.Count >= capacity)
                throw new InvalidOperationException();

            documents.Add(document);
        }

        public override string ToString()
        {
            return String.Format("Document store: {0}/{1}", documents.Count, capacity);
        }
    }

    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        DocumentStore documentStore = new DocumentStore(2);
    //        documentStore.AddDocument("item");
    //        Console.WriteLine(documentStore);
    //    }
    //}

    public enum Side { None, Left, Right }

    public class ChainLink
    {
        public ChainLink Left { get; private set; }
        public ChainLink Right { get; private set; }

        public void Append(ChainLink rightPart)
        {
            if (this.Right != null)
                throw new InvalidOperationException("Link is already connected.");

            this.Right = rightPart;
            rightPart.Left = this;
        }

        public Side LongerSide()
        {
            if (this.Right != null && this.Left == null) return Side.Right;
            else if (this.Right == null && this.Left != null) return Side.Left;
            else return Side.None;
        }

        //public static void Main(string[] args)
        //{
        //    ChainLink left = new ChainLink();
        //    ChainLink middle = new ChainLink();
        //    ChainLink right = new ChainLink();
        //    left.Append(middle);
        //    middle.Append(right);
        //    Console.WriteLine(left.LongerSide());
        //}
    }
}

