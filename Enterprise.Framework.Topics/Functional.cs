using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    public static class Functional
    {
        public static R Connect<R>(string connStr, Func<IDbConnection, R> func)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                return func(conn);
            }
        }



    }

  
}

namespace Enterprise.Framework.Topics.ImmutableTypes
{
    public class Age
    {
        public int Value { get; }

        public Age(int value)
        {

            this.Value = value;
        }
        private bool IsValid(int age) => age > 0 && age < 120;
    }
}
