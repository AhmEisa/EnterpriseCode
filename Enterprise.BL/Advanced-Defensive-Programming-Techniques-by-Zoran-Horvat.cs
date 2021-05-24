using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Enterprise.BL
{
    /*
     * Defensive coding: write code which explicitly defends from negative cases.Always applicable and produces poor results.
     * Defensive programming: produce design which defend out of the box.Positive and negative scenarios treated the same.
     *                        Somtime applicable and produces great results.
     * The first law of defensive programming: When you have to defend,You have already lost.
     * 
     */

    /* 02.Removing-Corruption-by-Only-Creating-Consistent-Objects
       Object's Internal Operation: what will happen to the object if its internal state inconsisten?
       Dependee's Operation: what will happen to others if an object publicly exposes inconsisten state?
       Factory function : constructs objects and does that correctly and completely.
       Implicit parameterless constructor is the built-in factory function.Sets numeric fields to zero,Boolean falgs to false,references to null.
       Every stateful objects should have a factory function which constructs it in complete and consistent state.
       Multiple factory methods indicate that the class is doing more than one thing.
       Rule of thumb: define one factor function per class.Have no discrete parameters (no booleans , no enums,etc)
       Rule of thumb: never accept null argument value.
       Fight by introduce derived classes by removing flags
       Wrap validation and construction into an object.Builder Concept.
       Existential precondition: a rule which must be satisfied before an object can be constructed.
       When addressing complex validation rules : abandon constructor validations and introduce builder.

     */
    abstract class Student
    {
        private IDictionary<Subject, Grade> Grades { get; } = new Dictionary<Subject, Grade>();
        public string Name { get; }
        public Semester Enrolled { get; private set; }
        public Student(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (char.IsHighSurrogate(name[name.Length - 1])) throw new ArgumentNullException(nameof(name));
            this.Name = name;
        }

        public void Enroll(Semester semester)
        {
            if (semester == null)
                throw new ArgumentException();
            this.Enrolled = semester;
        }
        public abstract bool CanEnroll(Semester semester);
        public int NameLength => this.Name.Length;
        public string NameIntializer => this.Name.Substring(0, char.IsHighSurrogate(this.Name[0]) ? 2 : 1);
        public void AddGrade(Subject subject, Grade grade)
        {
            // pitfall defense
            if (!Enum.IsDefined(typeof(Grade), grade)) throw new ArgumentException();
            if (subject == null || !this.IsEnlistedFor(subject)) throw new ArgumentException();
            if (this.Grades.ContainsKey(subject) && this.Grades[subject] != Grade.F) throw new ArgumentException();
            this.Grades[subject] = grade;
        }
        private bool IsEnlistedFor(Subject subject) => true;
        public bool HasPassedExam(Subject subject) => false;

        public bool CanTake(Exam exam) => true;
        ExamApplication Take(Exam exam) => new ExamApplication(new Subject(), new Professor(null), this);
        public IExamApplication ApplyFor(Subject examOn, Professor professor)
        {
            ExamApplicationBuilder builder = new ExamApplicationBuilder();
            builder.OnSubject(examOn);
            builder.AdminstredBy(professor);
            builder.TakenBy(this);

            if (builder.CanBuild()) { return builder.Build(); }
            else { throw new ArgumentException(); }
        }

        public Func<IExamApplication> GetExamApplicationFactory(Subject examOn, Professor professor)
        {
            ExamApplicationBuilder builder = new ExamApplicationBuilder();
            builder.OnSubject(examOn);
            builder.AdminstredBy(professor);
            builder.TakenBy(this);

            if (!builder.CanBuild()) throw new ArgumentException();

            return builder.Build;
        }
    }
    class RegularStudent : Student
    {
        public RegularStudent(string name) : base(name) { }
        public override bool CanEnroll(Semester semester) => semester != null && semester.Predecessor == base.Enrolled;
    }
    class ExchangeStudent : Student
    {
        public ExchangeStudent(string name) : base(name) { }
        public override bool CanEnroll(Semester semester) => semester != null;
    }
    class Semester
    {
        public Semester Predecessor { get; private set; }
    }
    class Professor
    {
        public PersonalName Name { get; }

        public Professor(PersonalName name)
        {
            if (name == null) throw new ArgumentNullException();
            this.Name = name;
        }
        public bool IsTeaching(Subject subject) => true;
        public Exam AdministerExam(Subject subject) => new Exam(null, this);
    }
    class Subject
    {
        public Semester TaughtDuring { get; private set; }
        public Professor TaughtBy { get; private set; }
    }
    class Exam //: IExam
    {
        private Subject OnSubject { get; }

        private Professor AdministeredBy { get; }

        public Exam(Subject subject, Professor professor)
        {
            if (subject == null) throw new ArgumentNullException();
            if (professor == null) throw new ArgumentNullException();

            this.OnSubject = subject;
            this.AdministeredBy = professor;
        }

    }
    enum Grade
    {
        A,
        B,
        C,
        D,
        E,
        F
    }
    class ExamApplication : IExamApplication
    {
        public Subject subject { get; }
        public Professor adminstrator { get; }
        public Student candidate { get; }

        public ExamApplication(Subject subject, Professor adminstrator, Student candidate)
        {
            this.subject = subject;
            this.adminstrator = adminstrator;
            this.candidate = candidate;
        }
    }
    interface IExamApplication
    {
        Subject subject { get; }
        Professor adminstrator { get; }
        Student candidate { get; }
    }
    class ExamApplicationBuilder // should have to moved to another namespace to avoid making bugs
    {
        private Professor Adminstrator { get; set; }
        private Subject Subject { get; set; }
        private Student Candidate { get; set; }

        public void AdminstredBy(Professor professor) { this.Adminstrator = professor; }
        public void OnSubject(Subject subject) { this.Subject = subject; }
        public void TakenBy(Student student) { this.Candidate = student; }
        public bool CanBuild() => this.Adminstrator != null
            && this.Subject != null
            && this.Candidate != null
            && this.Candidate.Enrolled == this.Subject.TaughtDuring
            && !this.Candidate.HasPassedExam(this.Subject)
            && this.Subject.TaughtBy == this.Adminstrator;

        public IExamApplication Build()
        {
            if (!this.CanBuild()) throw new ArgumentException();
            return new ExamApplication(this.Subject, this.Adminstrator, this.Candidate);
        }
    }
    /* 03.Removing-Corruption-by-Only-Making-Valid-State-Transitions
     * When a class exposes mutators,then some mutations may cause bugs,and then we have to include defensive code.
     * The simplest mutator is property setter.More Complex mutators : a method which changes state.
     * Mutations must be defended.
     * Raw data properties invite the caller to implement the operation.That breaks encapsulation.
     * One valid object leads to the creation of another valid object.
     * Persisting Rich Domain Model: Persist domain model and make it ORM-firendly Or Persist separate Model and keep the domain model persistence-ignorant. 
     * On Persisting the Domain Model:
     * Default Constructor: include parameterless in all model classes and can be private and orm will access it via reflection
     * Property Setters: setters on all properties defining persistable state.Can be private and orm will access it via reflection.
     * No Setter Validation: property setters must be dump as ORM is not aware of any setter rules.Object materailzation may fail if setters can throw.
     * Database ID Field.include database identity in every model class.
     * Separate Persistence Model: 
     * New Model: define new model in the infrastructure layer with no construction rules and no validations.
     * Plain getters/setters with no validations.As it designed to support fast and easy persistence.
     * There must be a mapping between domain model and persistence model.Map new domain obejcts before persisting it.
     * Map persisted object to domain obejct before using it.
     * If Simple Model: make the domain model persistable.That save a lot of work.
     * If Complex Model : invest in separating persistence for domain.Hard to add persistence to already complex one.
     * That would add unwanted complexity.And Persistence tradeoffs are not welcome in complex domains.
     * No Need to make concessions to persitence in the domain model.
     * State Mutations: changes happen after construction.Transition leads to new consistent state.
     * One method to enfore consistency: defend in all state changing methods.
     * Defend from invalid argurments: define domain of every function so that state and arguments combined must belong to the domain.
     * Restricting argument domains: define custom argument types.This removes reason to fail. 
     */


    /* 04.Dismissing-Defensive-Code-by-Avoiding-Primitive-Types
     * Enumerations: Assignment must be guarded.Change affects users.Guarding requires alternative implementation.Enumeration produces no objects.
     * Class interface dismisses defense.If you have an obejct,then it will work.
     * Stringfication: turning obejcts into strings to send over network,save to database,etc.So Why strings in domain objects.
     * Command-Query Segregation Principle: keep commands separate from queries.
     * Command: modifies the system state.Applies all the rules.Perform all validations.
     * Query: keeps the system state intact.Loading lots of data.Expects to get the result fast.No rule checking.
     * Data transfer object(DTO): enables transfer over a flat channel.Pulbic property getters/setters for serailization/deserialization.Parameterless constructor.
     * View Model: meant to be rendered on the view.Pulic property getters to populate the view.Public property setters for fast materilization.Paramterless constructor.
     * Infrastructure layer is home to persistence logic.
     * 
     */
    class GradeEnum
    {
        private GradeEnum(double numericValue) { this.NumericValue = numericValue; }
        public static GradeEnum A { get; } = new GradeEnum(1);
        public static GradeEnum B { get; } = new GradeEnum(2);
        public static GradeEnum C { get; } = new GradeEnum(3);
        public static GradeEnum D { get; } = new GradeEnum(4);
        public static GradeEnum E { get; } = new GradeEnum(5);
        public static GradeEnum F { get; } = new GradeEnum(6);
        public double NumericValue { get; }
    }
    abstract class Student2
    {
        private IDictionary<Subject2, GradeEnum> Grades { get; } = new Dictionary<Subject2, GradeEnum>();
        public PersonalName Name { get; }
        public Student2(PersonalName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.Name = name;
        }

        public void AddGrade(Subject2 subject, GradeEnum grade)
        {
            if (grade == null) throw new ArgumentException(nameof(grade));
            if (subject == null || !this.IsEnlistedFor(subject)) throw new ArgumentException();
            if (this.Grades.ContainsKey(subject) && this.Grades[subject] != GradeEnum.F) throw new ArgumentException();
            this.Grades[subject] = grade;
        }
        public double AverageGrade => this.Grades.Values.Select(r => r.NumericValue).DefaultIfEmpty(0).Average();
        private bool IsEnlistedFor(Subject2 subject) => true;
    }
    class Subject2
    {
        public string Name { get; }
        public Semester TaughtDuring { get; }
        public Professor TaughtBy { get; }
        private IList<Student2> EnlistedStudents { get; } = new List<Student2>();
        public Subject2(string name, Semester semester, Professor professor)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException();
            if (semester == null) throw new ArgumentException();
            if (professor == null) throw new ArgumentException();
            this.Name = name;
            this.TaughtDuring = semester;
            this.TaughtBy = professor;
        }
        public void Enlist(Student2 student)
        {
            if (student == null) throw new ArgumentException();
            this.EnlistedStudents.Add(student);
        }

        public void AssignGrades(IEnumerable<Tuple<PersonalName, GradeEnum>> grades)
        {
            var listedGrades = grades.Select(tuple => new { Student = this.EnlistedStudents.First(x => x.Name.Equals(tuple.Item1)), Grade = tuple.Item2 });
            foreach (var studentGrade in listedGrades)
                studentGrade.Student.AddGrade(this, studentGrade.Grade);
        }
    }
    class PersonalName
    {
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }
        public IEnumerable<string> lastNamePatterns { get; }
        public PersonalName(IEnumerable<string> lastNamePatterns, string firstName, string middleName, string lastName)
        {
            if (lastNamePatterns == null || lastNamePatterns.Any(pattern => pattern == null)) throw new ArgumentNullException();
            if (this.IsBadMandatoryPart(firstName) || this.IsBadOptionalPart(middleName) || this.IsBadMandatoryPart(lastName)) throw new ArgumentException();
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.lastNamePatterns = new List<string>(lastNamePatterns) { @"(?<letter>\w)" };
        }
        public char LastNameInitial
        {
            get
            {
                int firstLetterAt = 0;
                if (this.LastName.StartsWith("O'")) firstLetterAt = 2;
                if (this.LastName.ToLower().StartsWith("de ")) firstLetterAt = 3;

                return this.LastName[firstLetterAt];
            }
        }
        public string RefactoredLastNameInitial
        {
            get
            {
                int index = this.LastNameInitialPosition;
                if (char.IsHighSurrogate(this.LastName, index))
                    return $"{this.LastName[index]}{this.LastName[index + 1]}";
                return $"{this.LastName[index]}";
            }
        }

        private int LastNameInitialPosition => LastNamePatterns.Select(expr => Regex.Match(this.LastName, expr))
                                                               .Where(match => match.Success)
                                                               .Select(match => match.Groups["letter"].Index)
                                                               .First();
        private IEnumerable<string> LastNamePatterns => new[] { @"^[Dd][aei] (?<letter>\w)", @"^O (?<letter>\w)", @"(?<letter>\w)" };
        private bool IsBadOptionalPart(string part) => part == null || (part.Length > 0 && char.IsHighSurrogate(part[part.Length - 1]));
        private bool IsBadMandatoryPart(string part) => this.IsBadOptionalPart(part) || part == string.Empty;
        public override bool Equals(object obj)
        {
            return this.Equals(obj as PersonalName);
        }
        private bool Equals(PersonalName other) => other != null &&
                                                   this.ArePartsEqual(this.FirstName, other.FirstName) &&
                                                   this.ArePartsEqual(this.MiddleName, other.MiddleName) &&
                                                   this.ArePartsEqual(this.LastName, other.LastName);
        public override int GetHashCode() => this.FirstName.GetHashCode() ^ this.MiddleName.GetHashCode() ^ this.LastName.GetHashCode();
        private bool ArePartsEqual(string part1, string part2) => string.Compare(part1, part2, StringComparison.OrdinalIgnoreCase) == 0;
    }
    /* 05.Defining-Function-Domains-as-the-Primary-Line-of-Defense
     * Function fails if called outside its domain.Typically throws an exception.
     * Partial Function: function which is defined on a subset of all possible argument values.
     * Total Function: function which is defined for all possible argument values.
     * All kinds of guards: Null input,Zero value,Negative value,Empty string,Whitespace string,Incompatible values,Violated rules.
     * Domain rules: specific,and could be dynamuc and configurable.
     * Algerbraic rules: not specific to any domain.Execution impossible if violated.
     * Guaring against null is fine: that defines the domain on which function is defined.
     * Gurading against violating dynamic rules is not fine: that would be controlling execution flow with exceptions.
     * Adivce: if you do not use null in code,then null checks will never fail.
     * Do not pass null as argument.Do not return null from a method.Do not set variables to null.
     * Hard-coded Rules: no support for future requirements.No possibility to recover from an error.
     * Bad design: objects that tells the caller whether it can perform an opeartion or not.
     * Implicit validation principle: one consistent object constructs another consistent object.The process builds on data that are already valid.
     * Defenisve design: caller's execution flow must assume that action execution is optional.
     * Techniques to make functions total:
     * define custom argument types with sepecific domain.
     * Apply callback pattern and execute only when possible.
     * Filter objects that belong to domain.
     */
    interface IExam { Subject OnSubject { get; } Professor AdministeredBy { get; } }
    interface IExamRules
    {
        void WhenAplicable(Student candidate, IExam exam, Action<IExamApplication> action);
        // or use selector
        IEnumerable<IExamApplication> Filter(IEnumerable<Student> candidates, IExam exam);
    }
    class RegularExamRules : IExamRules
    {
        private bool CanApply(Student student, IExam exam) => student.Enrolled == exam.OnSubject.TaughtDuring && !student.HasPassedExam(exam.OnSubject);
        public void WhenAplicable(Student candidate, IExam exam, Action<IExamApplication> action)
        {
            if (candidate == null) throw new ArgumentNullException();
            if (exam == null) throw new ArgumentNullException();
            if (action == null) throw new ArgumentNullException();
            if (this.CanApply(candidate, exam))
            {
                IExamApplication application = new ExamApplication(exam.OnSubject, exam.AdministeredBy, candidate);
                action(application);
            }
        }

        // an alternative for removing branching and throwing exceptions
        public IEnumerable<IExamApplication> Filter(IEnumerable<Student> candidates, IExam exam)
        {
            if (candidates == null) throw new ArgumentNullException();
            if (exam == null) throw new ArgumentNullException();
            return candidates.Where(c => this.CanApply(c, exam)).Select(c => new ExamApplication(exam.OnSubject, exam.AdministeredBy, c));
        }
    }

    /* 06.Building-Defensive-Design-Instead-of-Writing-Defensive-Code
     Encapsulation: mechanism for restricting direct access to some of the object's components.
                   construct that facilitates the bundling of data with the methods operating on that data.
     When only state is encapsualted,then behavior is put far away from the data.
     Advice: strike the right balance between only exposing state and only exposing behavior.
             keep raw data private.Expose processed data only.Project state into a generally useful form.Method returning a value is also projecting private state,in away.
     Cohesion: keeping related things together.
     Advice: class with low cohesion should be split into two parts.
     Chain of responsibility: request is passed down the chain until served.
     Catch-all element:terminates the chain and processes all requests.
     Capturing group: part of the expression within parentheses. ex.:^[Dd][aei] (?<letter>\w)
     Benefits of using regular expressions: are plain text,are not executable.Serve as text processing language,no need for general purpose code.
                                            keep them separate from code,pull them from configuration,database,file,etc.
     Domain layer is the consumer of the persistence feature.It defines the interface which persistence feature must satisfy.

     */

    /* 07.The-Principle-of-Working-with-Objects-Not-with-Nulls
     * Null serves two major purposes: it serves as a value for unintilalized fields to be type-safe.Programmers use it to encode missing value result.
     * Partially intialized objects appear when constructor does not run to completion.
     * The first method to remove null is using callback pattern.
     * Callback pattern is good in small portions, but does not scale well across layers and classes.
     * The second method is Option(Maybe) in functional languages either contains a value or is none.
     */
    abstract class StudentNull
    {
        private IDictionary<SubjectNull, Grade> Grades { get; } = new Dictionary<SubjectNull, Grade>();
        public void WithGrade(SubjectNull onSubject, Action<Grade> doThis) => this.Grades.WithValue(onSubject, doThis);
        public Option<Grade> TryGetGrade(SubjectNull subject)
        {
            Grade grade;
            if (this.Grades.TryGetValue(subject, out grade))
                return Option.Some(grade);

            return Option.None<Grade>();
        }
    }
    class SubjectNull
    {
        public double? GetPassingRate(IEnumerable<StudentNull> candidates)
        {
            int passingGrades = 0;
            int totalGrades = 0;
            foreach (var candidate in candidates)
            {
                candidate.WithGrade(this, grade =>
                {
                    if (!grade.Equals(Grade.F)) { passingGrades += 1; }
                    totalGrades += 1;
                });
            }
            if (totalGrades > 0)
                return passingGrades / (double)totalGrades;
            return null;
        }

        public Option<double> TryGetPassingRate(IEnumerable<StudentNull> candidates)
        {
            var stats = candidates.Flatten(candidate => candidate.TryGetGrade(this))
                                .Aggregate(new { Passed = 0, Total = 0 }, (running, grade) => new
                                {
                                    Passed = running.Passed + (grade.Equals(Grade.F) ? 0 : 1),
                                    Total = running.Total + 1
                                });
            if (stats.Total == 0) return Option.None<double>();

            return Option.Some(stats.Passed / (double)stats.Total);
        }
        private IEnumerable<Grade> GetAllGrades(IEnumerable<StudentNull> candidates) => candidates.SelectMany(candidate => candidate.TryGetGrade(this).AsEnumerable());
    }

    /* 08.Building-Rich-Domain-Model-as-an-Effective-Defense-by-Design
     * Aliasing bugs: an object modifies a shared reference without telling the other object.
     * Immutable object forbids content modifications and only references other immutable objects.
     * 
     */

    /* 09.Designing-Alternative-Workflows-Instead-of-Defending-from-Errors
     * Status Code as return value: discrete value returned from an operation to indicate completion status.
     * So caller must checks status and that invites to bugs,and increase caller's complexity.
     * Exception Execution Flow: let the immediate caller catch the execption or catch it at the boundary of your module/service/layer.
     * Design rule: only catch an exception you can hanlde.
     * Avoid rethrowing the same exception and catching an exception you cannot handle.
     * Do not let out exceptions which are giving out your implementation details.
     * Advice: handling implementation-specific exceptions lets you change implementation later without affecting consumers.
     * 
     * 
     */
    class Module09
    {
        public void Process(Uri address)
        {
            var request = WebRequest.Create(address);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if(response.StatusCode== HttpStatusCode.NotFound)
                {
                    // indicate notfound
                }
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                // indicate timeout
            }
            catch (WebException)
            {
                // indicate network problem
            }
        }
    }
}
