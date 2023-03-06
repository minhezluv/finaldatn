using Dapper;
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
    public class BaseRepository<Entity> : IBaseRepository<Entity>
    {




        #region props and constructor
        protected IDbConnection _dbConnection = null;
        protected readonly string connString;
        protected readonly MySqlConnection conn;
        protected readonly string tableName;
        public BaseRepository(IConfiguration config)
        {
            tableName = typeof(Entity).Name.ToLower();
            // Create connection:
            connString = config.GetConnectionString("ConnectionString1");
            _dbConnection = new MySqlConnection(connString);
            _dbConnection.Open();
            conn = new MySqlConnection(connString);
        }
        #endregion
        public bool checkDuplicateCode(string EntityCode)
        {
            var sqlCmd = $"SELECT {tableName}ID from {tableName} WHERE {tableName}ID = '@{tableName}ID'";
            var dparams = new DynamicParameters();
            dparams.Add($"@{tableName}ID", EntityCode);
            var duplicateCode = conn.QueryFirstOrDefault<string>(sqlCmd, dparams);
            return (string.IsNullOrEmpty(duplicateCode));
        }

        public int Delete(Guid EntityId)
        {
            string sqlCmd = $"DELETE FROM {tableName} WHERE guid = @guid";
            DynamicParameters dparams = new DynamicParameters();
            dparams.Add($"@guid", EntityId);
            return conn.Execute(sqlCmd, dparams);
        }

        public List<Entity> GetAll()
        {
            var sqlCmd = $"SELECT * FROM {tableName}";
            return conn.Query<Entity>(sqlCmd).ToList();
        }

        public Entity GetById(Guid EntityId)
        {
            string sqlCmd = $"SELECT * FROM {tableName} WHERE guid = @guid";
            var dparams = new DynamicParameters();
            dparams.Add($"@guid", EntityId.ToString());
            return conn.QueryFirstOrDefault<Entity>(sqlCmd, dparams);
        }

        public int Insert(Entity entity)
        {
            var dparams = new DynamicParameters();
            var sqlCols = "";
            var sqlDParams = "";
            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                sqlCols += $"{propName},";
                sqlDParams += $"@{propName},";

                var propValue = prop.GetValue(entity);
                dparams.Add($"@{propName}", propValue);
            }
            sqlCols = sqlCols.Substring(0, sqlCols.Length - 1);
            sqlDParams = sqlDParams.Substring(0, sqlDParams.Length - 1);
            var sqlCmd = $"INSERT INTO {tableName} ({sqlCols}) VALUES({sqlDParams})";
            return conn.Execute(sqlCmd, dparams);
        }

        public int Update(Guid EntityId, Entity entity)
        {
            var dparams = new DynamicParameters();
            var col_param = "";
            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                col_param += $"{propName}=@{propName},";

                var propValue = prop.GetValue(entity);
                dparams.Add($"@{propName}", propValue);
            }
            col_param = col_param.Substring(0, col_param.Length - 1);
            var sqlCmd = $"UPDATE {tableName} SET {col_param} WHERE guid = @guid";
            return conn.Execute(sqlCmd, dparams);
        }

        public int MultiDeleteEntity(IEnumerable<Guid> listId)
        {
            int rowAffects = 0;

            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var id in listId)
                    {
                        var idParam = new DynamicParameters();
                        idParam.Add($"StaffID", id);

                        //Xóa 
                        rowAffects += _dbConnection.Execute($"Proc_Delete{tableName}ByID", idParam, commandType: CommandType.StoredProcedure, transaction: transaction);
                    }

                    //Nếu xóa tất cả được thì commit
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return rowAffects;
        }

        public Entity GetEntityByProperty(Entity entity, string propName)
        {
            var propvalue = entity.GetType().GetProperty(propName).GetValue(entity);

            string query = $"select * FROM {tableName} where {propName} = '{propvalue}'";
            var entitySearch = _dbConnection.QueryFirstOrDefault<Entity>(query);

            return entitySearch;
        }

        public Entity GetEntityByProperty(string propName, string propValue)
        {
            //Query string
            string query = $"select * FROM {tableName} where {propName} = '{propValue}'";

            //Lấy entity
            var entitySearch = _dbConnection.QueryFirstOrDefault<Entity>(query);

            return entitySearch;
        }
    }
}
