using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Enterprise.BL.Interfaces
{
    /* Interfaces: 
     * What : contract descibes a group of related functions that can belong to any class or struct fulfill that contract.
     * Why: maintain,extend,test.Resilience in the face of change and insulation from implementation details.   
     * How: create and maintain.
     * Where: practical bits , dependency injection,and mocking.
     * Repository pattern : mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects.
     *                      separate the application from data storage technology.
     * Compile-time factory: has a parameter.Caller picks the repository.Compile-time reference.
     * Dynamic factory: no parameter.Repository based on configuration.No compile-time referene.Decesions made at run-time.
     * Dynamic loading: get type and assembly from configuration.Load assembly through refelection.Crate the type instance with the Activator
     * use post-build event command line to copy references to executable directory:
     * xcopy "$(SolutionDir)RepositoriesForBin\*.*" "$(TargetDir)" /Y
     * xcopy "$(SolutionDir)RepositoriesForBin\\x86\*.*" "$(TargetDir)x86\" /Y
     * xcopy "$(SolutionDir)RepositoriesForBin\\x64\*.*" "$(TargetDir)x64\" /Y
     * use explicit interface implementation to resolve conflicting methods.
    */
    public class DynamicTypeCreation
    {
        public static T CreateType<T>(string configurationName)
        {
            string typeName = ConfigurationManager.AppSettings[configurationName];
            Type typeToCreate = Type.GetType(typeName);
            object typeAsObject = Activator.CreateInstance(typeToCreate);
            T createdObject = (T)typeAsObject;
            return createdObject;
        }
    }
    public class OddNumberGenerator : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            int i = 1;
            yield return i;
            while (true)
            {
                i += 2;
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class TipsAndTraps
    {
        public void CheckUniCodeCharacter(string sentence)
        {
            char charToCheck = sentence.First();
            UnicodeCategory unicodeCategory = char.GetUnicodeCategory(charToCheck);
            if (unicodeCategory != UnicodeCategory.OtherNotAssigned)
                Console.WriteLine("Valid Unicode Character");
        }

        public void AlignStringColumns(string sentence)
        {
            Console.WriteLine(string.Format("{0,-20}", sentence)); // left align
            Console.WriteLine(string.Format("{0,20}", sentence)); // right align
            Console.WriteLine($"{sentence,-20}");
            Console.WriteLine(sentence.PadRight(20));
        }

        public void ConditionalFormattingForNumbers(int number)
        {
            string threePartFormat = "(good) #;(bad) -#;(N/A)";
            Console.WriteLine(number.ToString(threePartFormat));
        }

        public void ParsingStringIntoNumberWithNumberStyle(string numberString)
        {
            int number = int.Parse(numberString, NumberStyles.AllowParentheses);
            int anotherNumber = int.Parse(numberString, NumberStyles.AllowParentheses | NumberStyles.AllowTrailingSign);
        }

        public void ParsingStringIntoDate(string dateString)
        {
            DateTime dob = DateTime.Parse(dateString);
            DateTime dob2 = DateTime.ParseExact(dateString, "MM/dd/yyyy", null);

            DateTime d1 = DateTime.Parse(dateString);
            DateTime d2 = DateTime.Parse(dateString, null, DateTimeStyles.AssumeUniversal);
            DateTime d3 = DateTime.Parse(dateString, null, DateTimeStyles.AssumeLocal);
            DateTime d4 = DateTime.Parse("13:35:00");//datetimestyles.none
            DateTime d5 = DateTime.Parse("13:35:00", null, DateTimeStyles.NoCurrentDateDefault);// 1/1/0001
        }

        public void GenerateRandomId()
        {
            using (RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[4];
                rnd.GetBytes(randomBytes);
                int result = BitConverter.ToInt32(randomBytes, 0);
            }
        }

        public void CheckPathOprations(string path)
        {
            path = Path.ChangeExtension(path, "bak");
            string dirName = Path.GetDirectoryName(path);
            string ext = Path.GetExtension(path);
            string file = Path.GetFileName(path);
            string fileNoExt = Path.GetFileNameWithoutExtension(path);
            bool hasExt = Path.HasExtension(path);

            ////////////////////////////////////
            char[] invalidNamesChar = Path.GetInvalidFileNameChars();
            string rndFileName = Path.GetRandomFileName();
            string rndTempFile = Path.GetTempFileName();
            string userTempPath = Path.GetTempPath();
            char platformSpecificSeparator = Path.DirectorySeparatorChar;

            /////////////////////////////////////////////////////////////
            string result = Path.Combine(@"\data", @"c:\temp");
            result = Path.Combine(@"c:\temp", @"\data");
            result = Path.Combine(@"c:\temp", @"data");
            result = Path.Combine(@"c:\temp", @"\data".TrimStart(Path.DirectorySeparatorChar));
            result = Path.Combine(@"c:\temp\data", @"..");
            result = Path.GetFullPath(result);
        }

        public void CheckEnumValues(StringProcessingOptions options)
        {
            bool noProcessingRequired = options == StringProcessingOptions.None;
            bool isUpperCaseRequired = (options & StringProcessingOptions.ConvertToUpperCase) != 0;
            bool addLengthIsRequired = options.HasFlag(StringProcessingOptions.AddLength);
        }

        [Flags]
        public enum StringProcessingOptions
        {
            None = 0,
            ConvertToUpperCase = 1,
            AddLength = 2,
            All = ConvertToUpperCase | AddLength
        }
    }

    public struct WithRefOverride
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Description { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is WithRefOverride))
                return false;

            var other = (WithRefOverride)obj;
            return X == other.X && Y == other.Y && Description == other.Description;
        }
    }
    public class CustomNumberFormatter : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            int number = (int)arg;
            if (number > 0)
                return $"{number} (Good)";
            return $"{number} N/A";
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            return null;
        }
    }
}
