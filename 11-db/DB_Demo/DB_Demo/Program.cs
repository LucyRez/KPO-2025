using System;
using Microsoft.Data.Sqlite;

// SCALAR QUERY EXAMPLE
// class Program
// {
//     static void Main()
//     {
//         string connectionString = "Data Source=../../../../vinyl_shop.db";

//         using (var connection = new SqliteConnection(connectionString))
//         {
//             connection.Open();

//             string scalarQuery = "SELECT SUM(stock) FROM Vinyl;";

//             using (var command = new SqliteCommand(scalarQuery, connection))
//             {

//                 object result = command.ExecuteScalar();
//                 int totalStock = result != DBNull.Value ? Convert.ToInt32(result) : 0;

//                 Console.WriteLine($"Total vinyl records in stock: {totalStock}");
//             }
//         }
//     }
// }


// READ EXAMPLE
// class Program
// {
//     static void Main()
//     {
//         string connectionString = "Data Source=../../../../vinyl_shop.db";
        
//         using (var connection = new SqliteConnection(connectionString))
//         {
//             connection.Open();
//             string query = "SELECT * FROM Vinyl";

//             using (var command = new SqliteCommand(query, connection))
//             {
//                 using (var reader = command.ExecuteReader())
//                 {
//                     while (reader.Read())
//                     {
//                         Console.WriteLine($"Title: {reader["title"]}, Artist: {reader["artist"]}");
//                     }
//                 }
//             }
//         }
//     }
// }




// NON QUERY EXAMPLE 1
// class Program
// {
//     static void Main()
//     {
//         string connectionString = "Data Source=../../../../vinyl_shop.db";

//         using (var connection = new SqliteConnection(connectionString))
//         {
//             connection.Open();

//             string userInputPrice = "74.99";
//             string userInputRecordId = "1"; 


//             string updateQuery = $"UPDATE Vinyl SET price = {userInputPrice} WHERE record_id = {userInputRecordId};";

//             using (var command = new SqliteCommand(updateQuery, connection))
//             {
//                 int rowsAffected = command.ExecuteNonQuery();
//                 Console.WriteLine(rowsAffected > 0 ? "Price updated successfully." : "Record not found.");
//             }
//         }
//     }
// }




//NON QUERY EXAMPLE 2

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=../../../../vinyl_shop.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string updateQuery = $"UPDATE Vinyl SET price = @price WHERE record_id = @record_id";

            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@price", 74.99); // New price
                command.Parameters.AddWithValue("@record_id", 1); // ID of the record to update

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected > 0 ? "Price updated successfully." : "Record not found.");
            }
        }
    }
}
