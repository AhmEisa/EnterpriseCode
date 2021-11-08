using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    /*
    • Fake It—Return a constant and gradually replace constants with variables until you have the real code.
    • Use Obvious Implementation—Type in the real implementation.
     */

    /*
    • Translated a design objection (side effects) into a test case that failed because of the objection
    • Got the code to compile quickly with a stub implementation
    • Made the test work by typing in what seemed to be the right code
     */

    //One of the constraints on Value Objects is that the values of the instance variables of the object never change once they have been set in the constructor.

    public class Dollar
    {
        private readonly int amount;

        public Dollar(int amount)
        {
            this.amount = amount;
        }

        public Dollar Times(int multiplier)
        {
            return new Dollar(this.amount * multiplier);
        }

        public override bool Equals(object obj)
        {
            //compare with null and other objects
            Dollar dollar = obj as Dollar;
            return this.amount == dollar.amount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
