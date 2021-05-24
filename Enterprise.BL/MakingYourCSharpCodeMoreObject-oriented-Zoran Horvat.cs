using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.BL
{
    /* Foundations of OO programming
      1.this Pointer : silently passed with a call to an instance-level function that brings operations more close to data making operations the quality of DS. 
      2.Dynamic Dispatch : object's duty to carry data about its type - type's duty to keep track of its virtual functions table to override some from its base type
                           runtime's duty to find V-table of its generating type and find concrete function address 
      And then the Consequences : Encapsulation - Abstraction - Inheritance - Ploymorphism
      Define Objects: which data goes with which operations
      Use Ploymorphism : which operations require dynamic dispatch
      Advice: avoid changing existing code.Each change my breake a feature which used to work fine.
      Advice: try to assign responsibilities to classes.
      Null is not an object. We have a class but donot have an object.Cannot build OO code on null references.
      Advice: consider constructing an object which represents nothing.That will be the proper object again.
      Advice: do not keep money as decimal.Introduce special Money type class to keep amount and currency together.
      Advice: start worrying as soon as number of unit tests has started to double with every new feature added.
      Advice: make a clean-cut branching instruction,Either a guard or Full if-then-else and avoid incomplete if-then instructions without else.
      Note: code after the guard clause is executed unconditionally

      */

    public class Account
    {
        public decimal Balance { get; private set; }
        private bool IsVerified { get; set; }
        private bool IsClosed { get; set; }
        private bool IsFrozen { get; set; }
        private Action OnUnFreeze { get; set; }

        private IFreezable Freezable { get; set; }
        public Account(Action onUnFreeze)
        {
            this.OnUnFreeze = onUnFreeze;
            this.Freezable = new Active(onUnFreeze);
        }
        public void Deposit(decimal amount)
        {
            if (this.IsClosed) return;
            if (this.IsFrozen)
            {
                this.IsFrozen = false;
                this.OnUnFreeze();
            }
            this.Freezable = this.Freezable.Deposit();
            this.Balance += amount;
        }
        public void Withdraw(decimal amount)
        {
            if (!this.IsVerified) return;
            if (this.IsClosed) return;
            if (this.IsFrozen)
            {
                this.IsFrozen = false;
                this.OnUnFreeze();
            }
            this.Freezable = this.Freezable.Withdraw();
            this.Balance -= amount;
        }
        public void HolderVerified() { this.IsVerified = true; }
        public void Close() { this.IsClosed = true; }
        public void Freeze()
        {
            if (this.IsClosed) return;
            if (!this.IsVerified) return;
            this.IsFrozen = true;
            this.Freezable = this.Freezable.Freeze();
        }
    }

    /*
     Try Turn branching into functions using:
     State Pattern : object of the state class represents one state.Change the object when you want to change the state.
     And Consequence of implementation : class does not have to represent its state explicitly anymore.Class does not have to manage state transition logic.
     Runtime type of the state object replaces branching and dynamic dispatch used to choose one implementation or the other.
     Two operations are coupled : call to op.A must be followed by a call to op.B : pass op.B as an argument to op.A and let op.A call op.B at its end.
     Advice: let the class do only one thing : SRP.So one new requirement means one new class will be added.And does not require an existing class to change. 

     */

    interface IFreezable
    {
        IFreezable Deposit();
        IFreezable Withdraw();
        IFreezable Freeze();
    }
    class Active : IFreezable
    {
        private Action OnUnFreeze;
        public Active(Action onUnFreeze)
        {
            this.OnUnFreeze = onUnFreeze;
        }
        public IFreezable Deposit() => this;

        public IFreezable Freeze() => this;

        public IFreezable Withdraw() => new Frozen(this.OnUnFreeze);
    }
    class Frozen : IFreezable
    {
        private Action OnUnFreeze;
        public Frozen(Action onUnFreeze)
        {
            this.OnUnFreeze = onUnFreeze;
        }
        public IFreezable Deposit()
        {
            this.OnUnFreeze();
            return new Active(this.OnUnFreeze);
        }

        public IFreezable Withdraw()
        {
            this.OnUnFreeze();
            return new Active(this.OnUnFreeze);
        }
        public IFreezable Freeze() => this;
    }

    /* The Final refactor of removing branches using explict state*/
    interface IAccountState
    {
        IAccountState Deposit(Action addToBalance);
        IAccountState Withdraw(Action subtractFromBalance);
        IAccountState Freeze();
        IAccountState HolderVerified();
        IAccountState Close();
    }

    class NotVerified : IAccountState
    {
        private Action OnUnFreeze;
        public NotVerified(Action onUnFreeze)
        {
            this.OnUnFreeze = onUnFreeze;
        }
        public IAccountState Close() => new Closed();

        public IAccountState Deposit(Action addToBalance)
        {
            addToBalance();
            return this;
        }

        public IAccountState Freeze() => this;

        public IAccountState Withdraw(Action subtractFromBalance) => this;

        public IAccountState HolderVerified() => new Active2(this.OnUnFreeze);
    }
    class Closed : IAccountState
    {
        public IAccountState Deposit(Action addToBalance) => this;

        public IAccountState Freeze() => this;

        public IAccountState HolderVerified() => this;

        public IAccountState Withdraw(Action subtractFromBalance) => this;

        public IAccountState Close() => this;
    }
    class Frozen2 : IAccountState
    {
        private Action OnUnFreeze;
        public Frozen2(Action onUnFreeze)
        {
            this.OnUnFreeze = onUnFreeze;
        }
        public IAccountState Close() => new Closed();

        public IAccountState Deposit(Action addToBalance)
        {
            this.OnUnFreeze();
            addToBalance();
            return new Active2(this.OnUnFreeze);
        }

        public IAccountState HolderVerified() => new Active2(this.OnUnFreeze);

        public IAccountState Withdraw(Action subtractFromBalance)
        {
            this.OnUnFreeze();
            subtractFromBalance();
            return new Active2(this.OnUnFreeze);
        }

        public IAccountState Freeze() => this;
    }
    class Active2 : IAccountState
    {
        private Action OnUnFreeze;
        public Active2(Action onUnFreeze)
        {
            this.OnUnFreeze = onUnFreeze;
        }
        public IAccountState Close() => new Closed();

        public IAccountState Deposit(Action addToBalance)
        {
            addToBalance();
            return this;
        }

        public IAccountState Freeze() => new Frozen2(this.OnUnFreeze);

        public IAccountState HolderVerified() => this;

        public IAccountState Withdraw(Action subtractFromBalance)
        {
            subtractFromBalance();
            return this;
        }
    }

    public class Account2
    {
        public decimal Balance { get; private set; }
        private IAccountState state { get; set; }
        public Account2(Action onUnFreeze) { state = new NotVerified(onUnFreeze); }
        public void Deposit(decimal amount) { this.state = this.state.Deposit(() => this.Balance += amount); }
        public void Withdraw(decimal amount) { this.state = this.state.Withdraw(() => this.Balance -= amount); }
        public void HolderVerified() { this.state = this.state.HolderVerified(); }
        public void Close() { this.state = this.state.Close(); }
        public void Freeze() { this.state = this.state.Freeze(); }
    }

    /*
     * Loops and branches instructions are infrastructure.And we do not deal with infrastructure.We only deal with functional requirements.
       Advice : avoid looping for data collections.
       Aggregate Function: takes a sequence and returns a single value or object.E.g. Min,Max,Avg
       Given a sequence of N elements find the best fitting one:
       Bad Idea:Sorting-by sort and pick the first element yields O(NlogN) running time.
       Better Idea:Picking- expect a solution with O(N) running time.
       General rule : do not surprise fellow coders.
       The principle of least surprise (a.k.a) The principle of least astonishment: If a necessary feature has a high astonishment factor,it may be necessary to redesign the feature. 
       Advice: implement the extension methods only to wrap the infrastructure code which is never going to change.
     
     */

    interface IPainter { bool Isavilable { get; } TimeSpan EstimateTimeToPaint(double squareMeters); double EstimateCompensation(double squareMeters); }
    class PaintingOpearions
    {
        public IPainter FindCheapestPainter(double squareMeters, IEnumerable<IPainter> painters)
        {
            //takes NLogN time.
            var solutionNo1 = painters.Where(p => p.Isavilable).OrderBy(p => p.EstimateCompensation(squareMeters)).FirstOrDefault();

            //EstimateCompensation called twice (used to called once)
            //throw exception if sequence is empty(used to return null)
            //poor readability
            var solutionNo2 = painters.Where(p => p.Isavilable).Aggregate((best, cur) => best.EstimateCompensation(squareMeters) < cur.EstimateCompensation(squareMeters) ? best : cur);

            //EstimateCompensation called twice (used to called once)
            //poor readability
            var solutionNo3 = painters.Where(p => p.Isavilable).Aggregate((IPainter)null,
                                                            (best, cur) => best == null || cur.EstimateCompensation(squareMeters) < best.EstimateCompensation(squareMeters) ? cur : best);
            //more readability
            //but still EstimateCompensation called twice(used to called once) on the extension method.
            var solutionNo4 = painters.Where(p => p.Isavilable).WithMinimum(painter => painter.EstimateCompensation(squareMeters));

            //more readability
            var solutionNo5 = painters.Where(p => p.Isavilable).WithMinimum2(painter => painter.EstimateCompensation(squareMeters));

            return solutionNo5;
        }
        public IPainter FindFastestPainter(double squareMeters, IEnumerable<IPainter> painters)
        {
            var solutionNo5 = painters.Where(p => p.Isavilable).WithMinimum2(painter => painter.EstimateTimeToPaint(squareMeters));

            return solutionNo5;
        }

        public IPainter WorkTogether(double sqMeters, IEnumerable<IPainter> painters)
        {
            TimeSpan time = TimeSpan.FromHours(1 / painters.Where(p => p.Isavilable).Select(p => 1 / p.EstimateTimeToPaint(sqMeters).TotalHours).Sum());
            double cost = painters.Where(p => p.Isavilable).Select(p => p.EstimateCompensation(sqMeters) / p.EstimateTimeToPaint(sqMeters).TotalHours * time.TotalHours).Sum();
            return new ProportionalPainter { TimePerSqMeter = TimeSpan.FromHours(time.TotalHours / sqMeters), DollarsPerHour = cost / time.TotalHours };
        }
    }

    /* Untangling structure from opeartions on business data
     * IEnumerable<T> does not obligate user to know its data structure.
     * Warning: use of IEnumerable<T> often provokes code duplication. يستفز
     Advice: if you notice opearions or data that always go together, then invent a type which wraps them together.
     Two faces of abstrat Types in programming : Corresponds to a physical phenomenon, or simplifies another phenomenon(real or not).
     Composite Pattern : compose a number of objects into a new object exposing the same public interface,Looks like a single object.
     Advice: give concrete classes menanogful names especailly when implementing interface.
     Advice: avoid exposing public setters in production code.
     Advice: try to invent classes that support both real and imaginary concepts.Invent abstractions.
     Tip: property of an object from inside a collection is two levels of indirection.That is too much knowledge about the structure of data.
     Goal#1: methods should not see the painters as a sequence.
     Goal#2: methods should not know internal logic of painters. 
     Advice: name classes the same as you talk.Avoid artificial mapping between spoken langauge and code.
     Should we expose a collection : are we interested in having the work done?are we interested to organize who did the work.
     Advice: do not go open the collection class and start coding it.Instead design the consuming end first!
     Collction class should not incorporates a combination of primitive oprations.It should be called from the consumer side.
     Advice: do not apply optimization early.Write simple code first,then use it.Otimize only if consuming leads to poor performance.

     */

    class ProportionalPainter : IPainter
    {
        public TimeSpan TimePerSqMeter { get; set; }
        public double DollarsPerHour { get; set; }
        public bool Isavilable => true;

        public double EstimateCompensation(double squareMeters) => this.EstimateTimeToPaint(squareMeters).TotalHours * DollarsPerHour;

        public TimeSpan EstimateTimeToPaint(double squareMeters) => TimeSpan.FromHours(this.TimePerSqMeter.TotalHours * squareMeters);
    }
    class Painters
    {
        private IEnumerable<IPainter> ContinedPainters { get; }
        public Painters(IEnumerable<IPainter> painters)
        {
            this.ContinedPainters = painters.ToList();
        }
        public Painters GetAvailable()
        {
            if (this.ContinedPainters.All(p => p.Isavilable)) return this;
            return new Painters(this.ContinedPainters.Where(p => p.Isavilable));
        }
        public IPainter GetCheapestOne(double sqMeters)
        {
            return this.ContinedPainters.WithMinimum2(p => p.EstimateCompensation(sqMeters));
        }
        public IPainter GetFastestOne(double sqMeters)
        {
            return this.ContinedPainters.WithMinimum2(p => p.EstimateTimeToPaint(sqMeters));
        }

    }
    class PaintingGroup : IPainter
    {
        private IEnumerable<IPainter> Painters { get; }
        public PaintingGroup(IEnumerable<IPainter> painters)
        {
            this.Painters = painters.ToList();
        }
        public bool Isavilable => this.Painters.Any(p => p.Isavilable);

        public double EstimateCompensation(double squareMeters) => this.Reduce(squareMeters).EstimateCompensation(squareMeters);

        public TimeSpan EstimateTimeToPaint(double squareMeters) => this.Reduce(squareMeters).EstimateTimeToPaint(squareMeters);
        private IPainter Reduce(double sqMeters)
        {
            TimeSpan time = TimeSpan.FromHours(1 / this.Painters.Where(p => p.Isavilable).Select(p => 1 / p.EstimateTimeToPaint(sqMeters).TotalHours).Sum());
            double cost = this.Painters.Where(p => p.Isavilable).Select(p => p.EstimateCompensation(sqMeters) / p.EstimateTimeToPaint(sqMeters).TotalHours * time.TotalHours).Sum();
            return new ProportionalPainter { TimePerSqMeter = TimeSpan.FromHours(time.TotalHours / sqMeters), DollarsPerHour = cost / time.TotalHours };
        }
    }

    class PaintingOpearionsAfterRefactor
    {
        public IPainter FindCheapestPainter(double squareMeters, Painters painters) => painters.GetAvailable().GetCheapestOne(squareMeters);

        public IPainter FindFastestPainter(double squareMeters, Painters painters) => painters.GetAvailable().GetFastestOne(squareMeters);

        public IPainter WorkTogether(double sqMeters, IEnumerable<IPainter> painters)
        {
            TimeSpan time = TimeSpan.FromHours(1 / painters.Where(p => p.Isavilable).Select(p => 1 / p.EstimateTimeToPaint(sqMeters).TotalHours).Sum());
            double cost = painters.Where(p => p.Isavilable).Select(p => p.EstimateCompensation(sqMeters) / p.EstimateTimeToPaint(sqMeters).TotalHours * time.TotalHours).Sum();
            return new ProportionalPainter { TimePerSqMeter = TimeSpan.FromHours(time.TotalHours / sqMeters), DollarsPerHour = cost / time.TotalHours };
        }
    }

    /* Prefer delegate Action/Func delegates over abstract methods
     * delegate can be supplied from outside world. <==> abstract methods require a derived class
     * we can extend the implementation without adding new classes <===> we have to add new derived class in order to extend the implemetation.
     */

    class CompositePainter<TPainter> : IPainter where TPainter : IPainter
    {
        private IEnumerable<TPainter> Painters { get; }
        private Func<double, IEnumerable<TPainter>, IPainter> Reduce { get; }

        public bool Isavilable => this.Painters.Any(p => p.Isavilable);

        public CompositePainter(IEnumerable<TPainter> painters, Func<double, IEnumerable<TPainter>, IPainter> reduce)
        {
            this.Painters = painters.ToList();
            this.Reduce = reduce;
        }
        public double EstimateCompensation(double squareMeters) => this.Reduce(squareMeters, this.Painters).EstimateCompensation(squareMeters);
        public TimeSpan EstimateTimeToPaint(double squareMeters) => this.Reduce(squareMeters, this.Painters).EstimateTimeToPaint(squareMeters);
    }

    static class CompositePaintersFactories
    {
        public static IPainter CreateCheapestSelector(IEnumerable<IPainter> painters) => new CompositePainter<IPainter>(painters, (sqMeters, sequence) => new Painters(sequence).GetAvailable().GetCheapestOne(sqMeters));
        public static IPainter CreateFastestSelector(IEnumerable<IPainter> painters) => new CompositePainter<IPainter>(painters, (sqMeters, sequence) => new Painters(sequence).GetAvailable().GetFastestOne(sqMeters));

        public static IPainter CreateGroup(IEnumerable<ProportionalPainter> propPainters) => new CompositePainter<ProportionalPainter>(propPainters, (sqMeters, painters) =>
         {
             TimeSpan time = TimeSpan.FromHours(1 / painters.Where(p => p.Isavilable).Select(p => 1 / p.EstimateTimeToPaint(sqMeters).TotalHours).Sum());
             double cost = painters.Where(p => p.Isavilable).Select(p => p.EstimateCompensation(sqMeters) / p.EstimateTimeToPaint(sqMeters).TotalHours * time.TotalHours).Sum();
             return new ProportionalPainter { TimePerSqMeter = TimeSpan.FromHours(time.TotalHours / sqMeters), DollarsPerHour = cost / time.TotalHours };
         });

    }
    class PaintingOpearionsAfterRefactor2
    {
        public static void DoOperations()
        {
            IEnumerable<ProportionalPainter> painters = new ProportionalPainter[10];
            IPainter painter = CompositePaintersFactories.CreateGroup(painters);
        }
    }

    /* Try Remove Aliasing Bugs:-
           Refrain from modifying the shared objects : objects that are received as an argument is shared with the caller.  
     Value-typed behavior : Existing objects do not change.Operation produces entirely new Object. 
     Reference types are susceptiple to aliasing bugs.By changing content of an object we are risking an aliasing bug to appear.


     */
    class MoneyAmount
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
    class Seller
    {
        public bool IsHappyHour { get; set; }
        public MoneyAmount Reserve(MoneyAmount cost)
        {
            MoneyAmount newCost = cost;
            if (IsHappyHour)
            {
                newCost = new MoneyAmount { Amount = cost.Amount * 0.5, Currency = cost.Currency };
            }
            Console.WriteLine("Reserve an item that cost {0}", cost.Amount);
            return newCost;
        }

        public void Buy(MoneyAmount wallet, MoneyAmount cost)
        {
            bool enoughMoney = wallet.Amount >= cost.Amount;
            MoneyAmount finalCost = Reserve(cost);
            bool finalEnough = wallet.Amount >= finalCost.Amount;

            if (enoughMoney && finalEnough)
                Console.WriteLine("You will Pay {0} with your {1}.", cost, wallet);
            else if (finalEnough)
                Console.WriteLine("This time,{0} will be enough to Pay {1}.", wallet, finalCost);
            else Console.WriteLine("You Cannot Pay {0} with your {1}.", cost, wallet);

        }
    }

    /*
     Immutable class: instance can not change after it has been created.Impossible to reproduce aliasing bug on one of it.
     Try to avoid implementing features at the consuming side.That leads to code duplications and endemic bugs.
     Advice: try to avoid instantiating objects directly.Modification to the constructor requires updates in all consumers.
             Let objects of the class construct subsequent objects.And make the signature of the method indicate that it will return a new object of the same kind.
     Advice on overloaded operators: would you guess that an opeartor exists on some class.Custom operators are not discoverable through intellisence.
                                     Try to avoid operator overloads and let consumers rely on proper method calls.
     Value-Typed Equality: two objects are equal if their types are the same and their contained values are the same.
     Tip: opreator == tests eqaulity of references unless overload.Comiplier will not turn this into a call to the Equals method.
     GetHashCode method : implementation inherited from System.Object returns value based on the objects's refrence.
     Whenever you override Equals method you have to override GetHashCode as well and let them depen on the same set of fields.
     Advice: when overriding Equals method,provide == and != operator overloads as well.
     Warning: try avoid recursive call to the == operator.This will cause infinite recursion and stack overflow error.
     Entity requires mutation over its liftime.
     Value object remains unchanged after instantiation.

    Remove property setters.
    Introduce factory method.Constructor is just fine.
    Add operations closed under the value type.Do not force consumers to do that.Operation returns new instance of the same type
    that makes the class safe to use.
    Implement full Value-typed symantic: declare the class sealed.Override Equals method.Implement IEquitable<T> interface.Override GetHashCode.
    Overload == and != operators.
   */
    sealed class MoneyAmountImmutable : IEquatable<MoneyAmountImmutable>
    {
        public double Amount { get; }
        public string Currency { get; }
        public MoneyAmountImmutable(double amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Money cannot be negative", nameof(amount));
            this.Amount = amount;
            this.Currency = currency;
        }

        public MoneyAmountImmutable Scale(double factor) => new MoneyAmountImmutable(this.Amount * factor, this.Currency);
        public static MoneyAmountImmutable operator *(MoneyAmountImmutable amount, double factor) => amount.Scale(factor);
        public override string ToString() => $"{this.Amount} {this.Currency}";

        public override bool Equals(object obj) => this.Equals(obj as MoneyAmountImmutable);
        public bool Equals(MoneyAmountImmutable other) => other != null && this.Amount == other.Amount && this.Currency == other.Currency;
        public override int GetHashCode() => this.Amount.GetHashCode() ^ this.Currency.GetHashCode();

        public static bool operator ==(MoneyAmountImmutable a, MoneyAmountImmutable b) =>
            (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null)) ||
            (!object.ReferenceEquals(a, null) && a.Equals(b));
        public static bool operator !=(MoneyAmountImmutable a, MoneyAmountImmutable b) => !(a == b);

    }

    /* remove nulls checking
     Advice: keep operations close to data they relate to.
     Cohesion between data and opeartions is important criterion when deciding.
     Warning: be aware of boolean methods arguments.Their appearance is the indication of leaving the OO design principles.
     Advice: tell the objects what to do.Let them decide how to do it.With boolean flags its the client who has to decide how to perform oprations.
     OO Design Rules: Object keeps track of its state.Object does not return null from its methods.
     Caller's wish: never return null from the property.
     Constructor preconidtions: must be satisfied before constructor is invoked.Otherwise, will fail and object will not be created.
     Postconditions: guarantees made by the class implementation.E.g. these property getters will never return null.
     Avoiding null references: devise an object that will stand in place of a null.Substitue object must behave exactly as the proper object.
     Null object: an object which exposes a certain interface,but internally it deos nothing.Has no state.Has no behavior.
     Advice: class that implements an interface should have a meaningful name.Avoid just repeating the interface name.
     Advice: do not branch over null tests.Substitute objects to choose one execution path or the other.
     Special case pattern : provide an object which universally addresses one situation.
     Specail case objects Contains no specific information(e.g. user data).Unlike null objects they contain some logic.
     Trap of OO design: do not design interfaces which are just making it possible for tha caller to implement features.
     Design interfaces which are forcing their implementing classes to provide features.
     Advice: avoid branching over boolean conditions calculated from the object's state.Make polymorphic calls on the state object instead.
    
    */
    class Warranty
    {
        private DateTime DateIssued { get; }
        private TimeSpan Duration { get; }
        public Warranty(DateTime dateIssued, TimeSpan duration)
        {
            this.DateIssued = dateIssued;
            this.Duration = TimeSpan.FromDays(duration.Days);
        }
        public bool IsValidOn(DateTime dateTime) => dateTime.Date >= this.DateIssued && dateTime.Date < this.DateIssued + this.Duration;
    }
    class SoldArticle
    {
        public SoldArticle(Warranty moneyBack, Warranty express)
        {
            if (moneyBack == null) throw new ArgumentNullException(nameof(moneyBack));
            if (express == null) throw new ArgumentNullException(nameof(express));
            this.MoneyBackGurantee = moneyBack;
            this.ExpressWarranty = express;
        }

        public Warranty MoneyBackGurantee { get; }
        public Warranty ExpressWarranty { get; }
    }
    class NullChecking
    {
        public void ClaimsWarranty(SoldArticle article, bool isInGoodCondition, bool isBroken)
        {
            DateTime now = DateTime.Now;
            if (isInGoodCondition && !isBroken && article.MoneyBackGurantee != null && article.MoneyBackGurantee.IsValidOn(now)) { Console.WriteLine("offer money back"); }
            if (isBroken && article.ExpressWarranty != null && article.ExpressWarranty.IsValidOn(now)) { Console.WriteLine("offer repair"); }
        }

        public void ClaimsWarrantyRefactor(SoldArticleRefactor article)
        {
            DateTime now = DateTime.Now;
            article.MoneyBackGurantee.Claim(now, () => Console.WriteLine("offer money back"));
            article.ExpressWarranty.Claim(now, () => Console.WriteLine("offer repair"));
        }

        public void CallOperartions()
        {
            DateTime sellingDate = new DateTime(2020, 12, 5);
            TimeSpan moneyBackSpan = TimeSpan.FromDays(30);
            TimeSpan warrantySpan = TimeSpan.FromDays(365);

            IWarranty moneyBack = new TimeLimitedWarranty(sellingDate, moneyBackSpan);
            IWarranty warranty = new TimeLimitedWarranty(sellingDate, warrantySpan);

            SoldArticleRefactor goods = new SoldArticleRefactor(moneyBack, warranty);

            ClaimsWarrantyRefactor(goods);

            SoldArticleRefactor anotherGoodsWithNoMoneyBack = new SoldArticleRefactor(VoidWarranty.Instance, warranty);

            ClaimsWarrantyRefactor(anotherGoodsWithNoMoneyBack);

            Console.ReadLine();
        }
    }

    interface IWarranty { void Claim(DateTime onDate, Action onValidClaim); }
    class TimeLimitedWarranty : IWarranty
    {
        private DateTime DateIssued { get; }
        private TimeSpan Duration { get; }
        public TimeLimitedWarranty(DateTime dateIssued, TimeSpan duration)
        {
            this.DateIssued = dateIssued;
            this.Duration = TimeSpan.FromDays(duration.Days);
        }
        private bool IsValidOn(DateTime dateTime) => dateTime.Date >= this.DateIssued && dateTime.Date < this.DateIssued + this.Duration;

        public void Claim(DateTime onDate, Action onValidClaim)
        {
            if (!this.IsValidOn(onDate))
                return;
            onValidClaim();
        }
    }
    class SoldArticleRefactor
    {
        public SoldArticleRefactor(IWarranty moneyBack, IWarranty express)
        {
            if (moneyBack == null) throw new ArgumentNullException(nameof(moneyBack));
            if (express == null) throw new ArgumentNullException(nameof(express));
            this.MoneyBackGurantee = moneyBack;
            this.ExpressWarranty = VoidWarranty.Instance;
            this.NotOperationalWarranty = express;
        }

        public void VisibleDamage() { this.MoneyBackGurantee = VoidWarranty.Instance; }
        public void NotOperational() { this.MoneyBackGurantee = VoidWarranty.Instance; this.ExpressWarranty = this.NotOperationalWarranty; }
        public IWarranty MoneyBackGurantee { get; private set; }
        public IWarranty ExpressWarranty { get; private set; }
        public IWarranty NotOperationalWarranty { get; }
    }
    class VoidWarranty : IWarranty
    {
        [ThreadStatic]
        private static VoidWarranty _instance;
        private VoidWarranty() { }
        public static VoidWarranty Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new VoidWarranty();
                return _instance;
            }
        }
        private bool IsValidOn(DateTime dateTime) => false;
        public void Claim(DateTime onDate, Action onValidClaim) { }
    }

    class LifeTimeWarranty : IWarranty
    {
        private DateTime DateIssued { get; }
        public LifeTimeWarranty(DateTime dateIssued)
        {
            this.DateIssued = dateIssued;
        }
        private bool IsValidOn(DateTime dateTime) => dateTime.Date >= this.DateIssued;
        public void Claim(DateTime onDate, Action onValidClaim)
        {
            if (!this.IsValidOn(onDate))
                return;
            onValidClaim();
        }
    }
}
