using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Generator.Importer.DAL.Services
{
    public class StoredProceduresService
    {
        public string GetIntSumAndDoubleMedian()
        {
            SqlParameter intSum = new SqlParameter("@Sum", SqlDbType.BigInt);
            intSum.Direction = ParameterDirection.Output;
            intSum.Value = 0;

            SqlParameter doubleMedian = new SqlParameter("@Median", SqlDbType.Float);
            doubleMedian.Direction = ParameterDirection.Output;

            using (var databaseContext = new DatabaseContext())
            {
                databaseContext.Database.ExecuteSqlRaw("EXEC GetIntSummAndDoubleMedian @Sum OUT, @Median OUT", intSum, doubleMedian);
            }

            return $"Integers sum:{(long)intSum.Value}. Doubles median:{(double)doubleMedian.Value}";
        }
    }
}
