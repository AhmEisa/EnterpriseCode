using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Enterprise.BL
{
    /*
     FP is programming with Mathematical Functions
    -Chracteristics:
     Method Signature Honest : possible inputs and outputs 
     Referntailly transaparency : does not affect the global state
     -Advantages
     Composable
     Easy to reason about
     Easier to unit test

     */

    public class FP
    {
        public int? Divide(int x, int y)
        {
            if (y == 0) return null;
            return x / y;
        }
    }

    /*
     Immutability: inability to change data
     State: data that changes over time
     Side Effect: a change that is made to some data
     */
    public class UserProfile
    {
        private User _user;
        private string _address;
        public UserProfile(User user, string address)
        {
            _user = user;
            _address = address;
        }
        public UserProfile UpdateUser(int userId, string name) // ====> Stateful ==> Imutablity:increase readability,single place for validating invaraints,automatic thread safety
        {
            var newUser = new User(userId, name);
            return new UserProfile(newUser, _address);
        }
    }
    public class User // =====> Stateless
    {
        public int Id { get; }
        public string Name { get; }
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class CustomerService
    {
        public void Process(string customerName, string addressString)
        {
            Address address = CreateAddress(addressString);
            Customer customer = CreateCustomer(customerName, address);
            SaveCustomer(customer);
        }



        private Address CreateAddress(string addressName)
        {
            return new Address(addressName);
        }

        private Customer CreateCustomer(string name, Address address)
        {
            return new Customer(name, address);
        }

        private void SaveCustomer(Customer customer)
        {
            // get the repository
            //save the model
        }
    }

    public class Address
    {
        public string AddressName { get; }

        public Address(string addressString)
        {
            AddressName = addressString;
        }
    }

    public class Customer
    {
        public string Name { get; }
        public Address Address { get; }
        public Customer(string name, Address address)
        {
            Name = name;
            Address = address;
        }
    }

    /*
     Immutability Limitations: CPU Usage , Memory Usage
     Use ImmutableList Builder Nuget Package to utilize performance
     */

    #region Immutable Core - Business Logic
    public class AuditManager
    {
        private readonly int _maxEntriesPerFile;

        public AuditManager(int maxEntriesPerFile)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
        }

        public FileAction AddRecord(FileContent currentFile, string visitorName, DateTime timeOfVisit)
        {

            List<AuditEntry> entries = Parse(currentFile.Content); //  File.ReadAllLines(currentFile.FileName);
            if (entries.Count < _maxEntriesPerFile)
            {
                entries.Add(new AuditEntry(entries.Count + 1, visitorName, timeOfVisit)); //File.AppendAllLines(currentFile.FileName, currentFile.Content);
                string[] newContent = Serialize(entries);
                return new FileAction(currentFile.FileName, newContent, ActionType.Update);
            }
            else
            {
                var entry = new AuditEntry(1, visitorName, timeOfVisit);
                string[] newContent = Serialize(new List<AuditEntry> { entry });
                string newFileName = GetNewFileName(currentFile.FileName);
                return new FileAction(newFileName, newContent, ActionType.Create);//File.WriteAllLines(currentFile.FileName, currentFile.Content);
            }
        }
        public IReadOnlyList<FileAction> RemoveMentionsAbout(string visitorName, FileContent[] directoryFiles)
        {
            return directoryFiles.Select(file => RemoveMentionsIn(file, visitorName)).Where(action => action != null).Select(action => action.Value).ToList();
        }
        private FileAction? RemoveMentionsIn(FileContent file, string vistorName)
        {
            List<AuditEntry> entries = Parse(file.Content);
            List<AuditEntry> newContent = entries.Where(x => x.Name != vistorName)
                .Select((entry, index) => new AuditEntry(index + 1, entry.Name, entry.TimeOfVisit)).ToList();

            if (newContent.Count == entries.Count)
                return null;
            if (newContent.Count == 0)
                return new FileAction(file.FileName, Serialize(newContent), ActionType.Delete);

            return new FileAction(file.FileName, Serialize(newContent), ActionType.Update);
        }

        private string[] Serialize(List<AuditEntry> entries)
        {
            return entries.Select(entry => entry.Id + ";" + entry.Name + ";" + entry.TimeOfVisit.ToString("s")).ToArray();
        }
        private List<AuditEntry> Parse(string[] content)
        {
            var result = new List<AuditEntry>();
            foreach (string line in content)
            {
                string[] data = line.Split(';');
                result.Add(new AuditEntry(int.Parse(data[0]), data[1], DateTime.Parse(data[2])));
            }
            return result;
        }
        private string GetNewFileName(string existingFileName)
        {
            string fileName = Path.GetFileNameWithoutExtension(existingFileName);
            int index = int.Parse(fileName.Split('_')[1]);
            return "Audit_" + (index + 1) + ".txt";
        }
    }

    public struct AuditEntry
    {
        public AuditEntry(int id, string name, DateTime timeOfVisit)
        {
            Id = id;
            Name = name;
            TimeOfVisit = timeOfVisit;
        }

        public readonly int Id;
        public readonly string Name;
        public readonly DateTime TimeOfVisit;
    }
    public struct FileAction
    {
        public readonly string FileName;
        public readonly string[] Content;
        public readonly ActionType Type;
        public FileAction(string fileName, string[] content, ActionType type)
        {
            FileName = fileName;
            Content = content;
            Type = type;
        }
    }
    public enum ActionType
    {
        Create,
        Update,
        Delete
    }
    public struct FileContent
    {
        public readonly string FileName;
        public readonly string[] Content;
        public FileContent(string fileName, string[] content)
        {
            FileName = fileName;
            Content = content;
        }
    }

    #endregion

    /*
     Make the mutable shell as dumb as pooible
     Apply side effect at the end of a business transaction
     Apply Integration Test on top of that ~1-2 to make sure of implemented operations
   */

    #region Mutable Shell
    public class FilePersister
    {
        public FileContent ReadFile(string fileName)
        {
            return new FileContent(fileName, File.ReadAllLines(fileName));
        }

        public FileContent[] ReadDirectory(string directoryName)
        {
            return Directory.GetFiles(directoryName).Select(x => ReadFile(x)).ToArray();
        }
        public void ApplyChanges(IReadOnlyList<FileAction> actions)
        {
            foreach (FileAction action in actions)
            {
                switch (action.Type)
                {
                    case ActionType.Create:
                    case ActionType.Update:
                        File.WriteAllLines(action.FileName, action.Content);
                        continue;
                    case ActionType.Delete:
                        File.Delete(action.FileName);
                        continue;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
        public void ApplyChange(FileAction action)
        {
            ApplyChanges(new List<FileAction> { action });
        }
    }

    public class ApplicationService
    {
        private readonly string _directoryName;
        private readonly FilePersister _persister;
        private readonly AuditManager _auditManager;

        public ApplicationService(string directoryName, FilePersister persister, AuditManager auditManager)
        {
            _directoryName = directoryName;
            _persister = persister;
            _auditManager = auditManager;
        }

        public void RemoveMentionsAbout(string visitorName)
        {
            FileContent[] files = _persister.ReadDirectory(_directoryName);
            IReadOnlyList<FileAction> actions = _auditManager.RemoveMentionsAbout(visitorName, files);
            _persister.ApplyChanges(actions);
        }

        public void AddRecord(string visitorName, DateTime timeOfVisit)
        {
            FileInfo fileInfo = new DirectoryInfo(_directoryName).GetFiles().OrderByDescending(x => x.LastWriteTime).First();
            FileContent file = _persister.ReadFile(fileInfo.Name);
            FileAction action = _auditManager.AddRecord(file, visitorName, timeOfVisit);
            _persister.ApplyChange(action);
        }
    }

    #endregion

    /*
     Methods with exceptions are not mathematical functions
     Always prefer using return values over exceptions
     Exceptions are for exceptional situations => preconidtions in domain models and 3rd party library and not input validations
     Exceptions should signalize a bug
     Do not use Exceptions in situations you expect to happen 
     Fail Fast Principle : stopping the current operation or shutdown the process based on application stateful or stateless
                           make confidence in working software and protects the persistence state. 
     Catch Exceptions on the most Top level to log exception details and shutdown the operation and do not put any domain logic here
     Catch Exceptions on the lowest level of 3rd party library such as ORM DB and catch only exceptions you know how to handle
     */
    public class Employee
    {
        public void UpdateName(string name)
        {
            if (name == null)
                throw new ArgumentNullException();
        }
    }

    /*
     Helps keep the method honest
     Incorporates the result of an operation with it status
     Unified error model
     Only for expected failures
   */
   
    public enum ErrorType
    {
        DatabaseError,
        InputValidationError,
        EmptyEmail,
        TooLongEmail,
        InValidEmail
    }

    /* Avoiding Primitive Obsession 
       Anti-Pattern : Stands for using primitive types for domain modeling
       drawbacks: method signature dishonesty(Makes code dishonest) , may cause code duplication(Violates the DRY principle)
       Coorelates to defensive programming by reduce the effort of checking and validations
       Do not create types for all domain concepts
       Try not put wrappers around value objects inside domain models instead try use value objects in all places inside domain models 
       Value objects can be converted into primitive types when leaves the domain boundary like at the frontend (html) or store it in database 
     */
    public class PO_User
    {
        public string Email { get; }
        public PO_User(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("not empty email");
            email = email.Trim();
            if (email.Length > 256) throw new ArgumentException("too long");
            if (!email.Contains("@")) throw new ArgumentException("invalid");
            Email = email;
        }
    }
    public class PO_UserFactory
    {
        //method signature dishonesty
        public PO_User CreateUSer(string email)
        {
            return new PO_User(email);
        }
    }
    public class PO_Organization
    {
        public string PrimaryEmail { get; }
        public string SecondaryEmail { get; }
        public PO_Organization(string primaryEmail, string secondaryEmail)
        {
            if (string.IsNullOrEmpty(primaryEmail)) throw new ArgumentException("not empty email");
            primaryEmail = primaryEmail.Trim();
            if (primaryEmail.Length > 256) throw new ArgumentException("too long");
            if (!primaryEmail.Contains("@")) throw new ArgumentException("invalid");
            PrimaryEmail = primaryEmail;

            //may cause code duplication(Violates the DRY principle)
            if (string.IsNullOrEmpty(secondaryEmail)) throw new ArgumentException("not empty email");
            secondaryEmail = secondaryEmail.Trim();
            if (secondaryEmail.Length > 256) throw new ArgumentException("too long");
            if (!secondaryEmail.Contains("@")) throw new ArgumentException("invalid");
            SecondaryEmail = secondaryEmail;
        }
    }

    /*
     provide a class for each domain logic that can be implented on primitive type
     */
    public class Email
    {
        private readonly string _value;
        private Email(string value)
        {
            _value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return Result.Fail<Email>(ErrorType.EmptyEmail);
            email = email.Trim();
            if (email.Length > 256) return Result.Fail<Email>(ErrorType.TooLongEmail);
            if (!email.Contains("@")) return Result.Fail<Email>(ErrorType.InValidEmail);
            return Result.Ok(new Email(email));
        }

        public override bool Equals(object obj)
        {
            var email = obj as Email;
            if (ReferenceEquals(email, null))
                return false;
            return _value == email._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        public static implicit operator string(Email email)
        {
            return email._value;
        }
        public static explicit operator Email(string email)
        {
            return Create(email).Value;
        }
    }
    public class Email2 : ValueObject<Email2>
    {
        private readonly string _value;
        private Email2(string value)
        {
            _value = value;
        }

        public static Result<Email2> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return Result.Fail<Email2>(ErrorType.EmptyEmail);
            email = email.Trim();
            if (email.Length > 256) return Result.Fail<Email2>(ErrorType.TooLongEmail);
            if (!email.Contains("@")) return Result.Fail<Email2>(ErrorType.InValidEmail);
            return Result.Ok(new Email2(email));
        }

        protected override bool EqualsCore(Email2 other)
        {
            return _value == other._value;
        }
        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }

        public static implicit operator string(Email2 email)
        {
            return email._value;
        }
        public static explicit operator Email2(string email)
        {
            return Create(email).Value;
        }
    }

    /* relates to the defensive programming concept */
    public class DFPRogrammingCodeSmell
    {
        public void ProcessUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (name.Trim().Length > 100) throw new ArgumentException(nameof(name));
            // process Code
        }

        public void UpdateUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (name.Trim().Length > 100) throw new ArgumentException(nameof(name));
            // process Code
        }

        public void CreateUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (name.Trim().Length > 100) throw new ArgumentException(nameof(name));
            // process Code
        }
    }
    public class DFPRogrammingAfterRemovePO
    {
        public void ProcessUser(PO_UserName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            // process Code
        }

        public void UpdateUser(PO_UserName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            // process Code
        }

        public void CreateUser(PO_UserName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            // process Code
        }
    }

    public class PO_UserName
    {
        private readonly string _value;
        public PO_UserName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (name.Trim().Length > 100) throw new ArgumentException(nameof(name));
            _value = name;
        }
    }

    /* Avoiding Nulls with Maybe Type
     * achieve code honesty
     * Using Fody.NullGuard
     * fail fast principle and less defensive programming
     * Convert nulls into maybe when they enter the domain model
     * Convert them back to nulls when they leave the domain model
     * Apply Null guards only on the domain models
     */

    /* Traditional aprroaches to handling failures and input errors
     Handle all input errors at the boundaries of domain model
     Catch all expected failures at the lowest level possible
     Use Rail-way oriented programming as possible as you can
     But it is not suitable for sophisticated scenarios
     */

}

