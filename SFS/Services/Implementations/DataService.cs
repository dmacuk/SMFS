using SMFS.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using CustomDataSource;
using CustomDataSource.Model;

namespace SMFS.Services.Implementations
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataService : IDataService
    {
        private const string InsertPerson = @"INSERT INTO person (firstname, lastname, address, phone, email, notes, hidden) 
                                              VALUES (@firstname, @lastname, @address, @phone, @email, @notes, @hidden);
                                              SELECT @@IDENTITY";

        private const string InsertTransaction =
                        @"INSERT INTO [transaction] (transactiondate, personid, payee, reference, amount) 
                          VALUES (@transactiondate, @personid, @payee, @reference, @amount);"
            ;

        private const string LoadPerson = "SELECT * FROM person WHERE id = @id";
        private const string SelectAll = "SELECT * FROM person ORDER BY lastname, firstname";
        private const string SelectTransactions = "SELECT * FROM [transaction] WHERE personid = @personId ORDER BY transactiondate";
        private const string UpdatePerson = @"UPDATE person 
                                              SET firstname = @firstname,
                                                  lastname = @lastname,
                                                  address = @address,
                                                  phone = @phone,
                                                  email = @email,
                                                  notes = @notes,
                                                  hidden = @hidden,
                                                  lastupdate = getdate() 
                                              WHERE id = @id";
        private static readonly ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings["Vista"];
        private readonly DbProviderFactory _factory = DbProviderFactories.GetFactory(ConnectionStringSettings.ProviderName);

        public async Task AddTransaction(long personId, Transaction transaction) => await Task.Factory.StartNew(() =>
         {
             using (var conn = _factory.CreateConnection())
             {
                 conn.ConnectionString = ConnectionStringSettings.ConnectionString;
                 conn.Open();
                 using (var stmt = conn.CreateCommand())
                 {
                     stmt.CommandText = InsertTransaction;
                     stmt.Parameters.Add(CreateParameter(stmt, "@transactiondate", DbType.DateTime,
                         transaction.TransactionDate));
                     stmt.Parameters.Add(CreateParameter(stmt, "@personid", DbType.Int64, personId));
                     stmt.Parameters.Add(CreateParameter(stmt, "@payee", DbType.String, transaction.Payee));
                     stmt.Parameters.Add(CreateParameter(stmt, "@reference", DbType.String, transaction.Reference));
                     stmt.Parameters.Add(CreateParameter(stmt, "@amount", DbType.Decimal, transaction.Amount));
                     stmt.ExecuteNonQuery();
                 }
             }
         });

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public async Task<List<PaymentReportEntry>> GetPaymentReportEntries(long? personId, DateTime? startDate, DateTime? endDate)
        {
            return await Task.Factory.StartNew(() =>
            {
                var paymentReportEntries = new List<PaymentReportEntry>();
                using (var conn = _factory.CreateConnection())
                {
                    conn.ConnectionString = ConnectionStringSettings.ConnectionString;
                    conn.Open();
                    using (var stmt = conn.CreateCommand())
                    {
                        var addId = personId.HasValue;
                        stmt.CommandText = PaymentReportSqlText(addId);
                        stmt.Parameters.Add(CreateParameter(stmt, "@StartDate", DbType.DateTime,
                            startDate ?? DateTime.MinValue));
                        stmt.Parameters.Add(CreateParameter(stmt, "@EndDate", DbType.DateTime,
                            endDate ?? DateTime.MaxValue));
                        if (addId)
                            stmt.Parameters.Add(CreateParameter(stmt, "@PersonId", DbType.Int64, personId.Value));
                        using (var rs = stmt.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                paymentReportEntries.Add(new PaymentReportEntry(rs));
                            }
                        }
                    }
                }
                return paymentReportEntries;
            });
        }

        public Person GetPerson(long id)
        {
            using (var conn = _factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionStringSettings.ConnectionString;
                conn.Open();
                using (var stmt = conn.CreateCommand())
                {
                    stmt.CommandText = LoadPerson;
                    stmt.Parameters.Add(CreateParameter(stmt, "@id", DbType.Int64, id));
                    using (var rs = stmt.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            return new Person(rs);
                        }
                    }
                }
            }

            return new Person();
        }

        public async Task<List<Transaction>> GetTransactions(long personId) => await Task.Factory.StartNew(() =>
                                                                             {
                                                                                 using (var conn = _factory.CreateConnection())
                                                                                 {
                                                                                     conn.ConnectionString = ConnectionStringSettings.ConnectionString;
                                                                                     conn.Open();
                                                                                     var transactions = new List<Transaction>();
                                                                                     using (var stmt = conn.CreateCommand())
                                                                                     {
                                                                                         stmt.CommandText = SelectTransactions;
                                                                                         stmt.Parameters.Add(CreateParameter(stmt, "@personId", DbType.Int64, personId));
                                                                                         using (var rs = stmt.ExecuteReader())
                                                                                         {
                                                                                             while (rs.Read())
                                                                                             {
                                                                                                 transactions.Add(new Transaction(rs));
                                                                                             }
                                                                                         }
                                                                                     }
                                                                                     return transactions;
                                                                                 }
                                                                             });

        public async Task<List<Person>> LoadAll() => await Task.Factory.StartNew(() =>
        {
            using (var conn = _factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionStringSettings.ConnectionString;
                conn.Open();
                using (var stmt = conn.CreateCommand())
                {
                    stmt.CommandText = SelectAll;
                    var results = new List<Person>();
                    using (var rs = stmt.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            results.Add(new Person(rs));
                        }
                        return results;
                    }
                }
            }
        });

        public async Task<List<PaymentReportEntry>> SampleReportData()
        {
            return await Task.Factory.StartNew(() =>
            {
                return SamplePaymentEntries.SampleData();
            });
        }

        public async Task<Person> SavePerson(Person person) => await Task.Factory.StartNew(() =>
                                                                         {
                                                                             using (var conn = _factory.CreateConnection())
                                                                             {
                                                                                 conn.ConnectionString = ConnectionStringSettings.ConnectionString;
                                                                                 conn.Open();
                                                                                 using (var stmt = conn.CreateCommand())
                                                                                 {
                                                                                     var newRecord = person.Id == 0;
                                                                                     stmt.CommandText = newRecord ? InsertPerson : UpdatePerson;
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@firstname", DbType.String, person.FirstName));
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@lastname", DbType.String, person.LastName));
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@address", DbType.String, person.Address));
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@phone", DbType.String, person.Phone));
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@email", DbType.String, person.Email));
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@notes", DbType.String, person.Notes));
                                                                                     stmt.Parameters.Add(CreateParameter(stmt, "@hidden", DbType.Boolean, person.Hidden));
                                                                                     long id;
                                                                                     if (newRecord)
                                                                                     {
                                                                                         id = (long)stmt.ExecuteScalar();
                                                                                     }
                                                                                     else
                                                                                     {
                                                                                         stmt.Parameters.Add(CreateParameter(stmt, "@id", DbType.Int64, person.Id));
                                                                                         stmt.ExecuteNonQuery();
                                                                                         id = person.Id;
                                                                                     }
                                                                                     return GetPerson(id);
                                                                                 }
                                                                             }
                                                                         });

        private static DbParameter CreateParameter(DbCommand cmd, string name, DbType paramType, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = paramType;
            parameter.Value = value;
            return parameter;
        }

        private static string PaymentReportSqlText(bool addId)
        {
            var sb = new StringBuilder("");
            sb.Append("SELECT person.id, firstname, lastname, address, phone, email, transactiondate, payee, reference, amount ").Append(Environment.NewLine);
            sb.Append("FROM person ").Append(Environment.NewLine);
            sb.Append("JOIN [transaction] ON personid = person.id ").Append(Environment.NewLine);
            sb.Append("WHERE hidden<>TRUE ").Append(Environment.NewLine);
            sb.Append("AND transactiondate BETWEEN @StartDate AND @EndDate").Append(Environment.NewLine);
            if (addId)
                sb.Append("AND person.id = @PersonId").Append(Environment.NewLine);
            sb.Append("ORDER BY lastname, firstname, transactiondate").Append(Environment.NewLine);
            return sb.ToString();
        }

    }
}