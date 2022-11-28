using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Customers
{

    class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Customer> listCustomers = GetExcelCustomer();

            string connectionString = GetConnectionString();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    try
                    {
                        var dt = CreateDataTableItem(listCustomers);
                        using var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null);
                        sqlBulkCopy.DestinationTableName = "dbo.customers";
                        sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                        sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                        sqlBulkCopy.ColumnMappings.Add("Address", "Address");
                        sqlBulkCopy.ColumnMappings.Add("City", "City");
                        sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                        sqlBulkCopy.ColumnMappings.Add("PostalCode", "PostalCode");
                        sqlBulkCopy.ColumnMappings.Add("Phone", "Phone");
                        
                        sqlBulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

        private static DataTable CreateDataTableItem(List<Customer> ItemList)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.TableName = "customers";

                foreach (PropertyInfo property in typeof(Customer).GetProperties())
                {
                    dt.Columns.Add(new DataColumn() { ColumnName = property.Name, DataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType, AllowDBNull = true });
                }

                foreach (var item in ItemList)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo property in typeof(Customer).GetProperties())
                    {
                        newRow[property.Name] = item.GetType().GetProperty(property.Name)?.GetValue(item, null) ?? DBNull.Value;
                    }
                    dt.Rows.Add(newRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static List<Customer> GetExcelCustomer()
        {
            string path = @"../../../archivos/Customers.csv";
            var reader = new StreamReader(File.OpenRead(path));
            List<Customer> list = new List<Customer>();
            int cont = 1;
            while (!reader.EndOfStream)
            {
                var linea = reader.ReadLine();
                if (cont != 1)
                {
                    var valores = linea.Split(';');
                    Customer ct = new Customer();
                    ct.Id = valores[0] ?? "";
                    ct.Name = valores[1] ?? "";
                    ct.Address = valores[2] ?? "";
                    ct.City = valores[3] ?? "";
                    ct.Country = valores[4] ?? "";
                    ct.PostalCode = valores[5] ?? "0";
                    ct.Phone = valores[6] ?? "";
                    list.Add(ct);
                }
                cont++;
            }
            return list;
        }

        private static string GetConnectionString()
        {
            var conect = "Server=RUBEN-PC\\SQLEXPRESS;Database=Customers; Integrated Security=true";
            return conect;

        }
    }
}


