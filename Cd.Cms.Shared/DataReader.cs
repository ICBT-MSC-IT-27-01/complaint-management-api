using Microsoft.Data.SqlClient;

namespace Cd.Cms.Shared
{
    public static class DataReader
    {
        public static string GetString(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? string.Empty : r[col].ToString()!;
        public static long GetLong(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? 0L : Convert.ToInt64(r[col]);
        public static long? GetNullableLong(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? null : Convert.ToInt64(r[col]);
        public static int GetInt(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? 0 : Convert.ToInt32(r[col]);
        public static bool GetBool(SqlDataReader r, string col)
            => r[col] != DBNull.Value && Convert.ToBoolean(r[col]);
        public static DateTime GetDate(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(r[col]);
        public static DateTime? GetNullableDate(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? null : Convert.ToDateTime(r[col]);
        public static decimal GetDecimal(SqlDataReader r, string col)
            => r[col] == DBNull.Value ? 0m : Convert.ToDecimal(r[col]);
    }
}
