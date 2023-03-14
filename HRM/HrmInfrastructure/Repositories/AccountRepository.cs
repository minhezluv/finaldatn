using Dapper;
using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmInfrastructure.Repositories
{
    public class AccountRepository: BaseRepository<Account>, IAccountRepository {

        public AccountRepository(IConfiguration config) : base(config)
        { }

        public bool checkAccount(string username, string password, string role)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            using (IDbConnection dbConnection = new MySqlConnection(connString))

            {
                string sqlCmd = $"SELECT * FROM {tableName} WHERE Username = @username and Password = @password and RoleID = @role";
                dynamicParameters.Add($"@username", username);
                dynamicParameters.Add($"@password", password);
                dynamicParameters.Add($"@role", role);
                Account res = res = conn.QueryFirstOrDefault<Account>(sqlCmd, dynamicParameters);
                if (res != null) { return true; }
                return false;
            }
        }

        public int insertAccount(Account account)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            using (IDbConnection dbConnection = new MySqlConnection(connString))

            {

                int rowAffects = dbConnection.Execute($"Proc_InsertAccount", account, commandType: CommandType.StoredProcedure);
                return rowAffects;
            }

        }
    }
}
