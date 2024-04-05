﻿using Microsoft.Data.SqlClient;


Func<IEnumerable<Abonent>> GetAbonents = () =>
{
    var connectionString = @"
        Server=host.docker.internal,1433; 
        Initial Catalog=PhoneBook;
        User Id=SA;Password=Password01;
        MultipleActiveResultSets=True; 
        TrustServerCertificate=true;";

    var query = @"
        SELECT TOP 10 a.FullName, SUM(c.TotalCalls + r.TotalCalls) as TotalCalls 
        FROM [dbo].[Abonents] as a 
        LEFT JOIN ( 
            SELECT CallerId as Abonent, COUNT(Id) as TotalCalls 
            FROM Calls 
            GROUP BY CallerId
        ) as c ON a.Id = c.Abonent 
        LEFT JOIN ( 
            SELECT ReceiverId as Abonent, COUNT(Id) as TotalCalls 
            FROM Calls 
            GROUP BY ReceiverId 
        ) as r ON a.Id = r.Abonent 
        GROUP BY a.FullName 
        ORDER BY TotalCalls DESC";

    #region Вариант через UNION ALL
    //var query = @"
    //    SELECT TOP 10 FullName, SUM(TotalCalls) as TotalCalls
    //    FROM
    //    (
    //        SELECT a.FullName, COUNT(c.Id) as TotalCalls
    //        FROM [dbo].[Abonents] as a
    //        LEFT JOIN Calls as c on a.Id = c.CallerId
    //        GROUP BY a.FullName, c.CallerId

    //        UNION ALL

    //        SELECT a.FullName, COUNT(c.Id) as TotalCalls
    //        FROM [dbo].[Abonents] as a
    //        LEFT JOIN Calls as c on a.Id = c.ReceiverId
    //        GROUP BY a.FullName, c.ReceiverId
    //    ) as CombinedCalls
    //    GROUP BY FullName
    //    ORDER BY TotalCalls DESC";
    #endregion

    var results = new List<Abonent>();

    using (var connection = new SqlConnection(connectionString))
    {
        using var command = new SqlCommand(query, connection);
        
        connection.Open();
        
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var fullName = reader["FullName"] as string ?? string.Empty;
            var totalCalls = reader["TotalCalls"] as int? ?? 0;
            var abonent = new Abonent(fullName, totalCalls);
            results.Add(abonent);
        }
    }

    return results;
};


var abonents  = GetAbonents();

foreach (var abonent in abonents)
{
    Console.WriteLine(abonent);
}

Console.ReadKey();

record Abonent(string FullName, int TotalCalls);
