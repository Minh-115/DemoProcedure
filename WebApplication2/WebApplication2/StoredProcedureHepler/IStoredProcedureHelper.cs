using Dapper;
using Microsoft.Data.SqlClient;

namespace WebApplication2.StoredProcedureHepler
{
    public interface IStoredProcedureHelper : IDisposable
    {
        // Phương thức thực hiện procedure không trả về kết quả (ví dụ: Insert/Update/Delete)
        int ExecuteNonQuery(string procedureName, object parameters = null);
        List<T> ExecuteQuery<T>(string procedureName, object parameters = null);
        object ExecuteScalar(string procedureName, object parameters = null);
        //DynamicParameters CreateSqlParameters(params (string Name, object Value)[] parameters);
    }
}
