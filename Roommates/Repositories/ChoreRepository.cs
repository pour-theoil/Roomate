using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString)
        {
        }

        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select c.Id, c.Name from  chore c left join RoommateChore r on c.Id = r.ChoreId where r.id is null;";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };
                        chores.Add(chore);
                    }
                    reader.Close();
                    return chores;

                }
            }
        }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Id, Name FROM Chore";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> chores = new List<Chore>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPostition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPostition);

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        chores.Add(chore);

                    }

                    reader.Close();

                    return chores;
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select Name from Chore where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                    }

                    reader.Close();

                    return chore;
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Insert into Chore (Name)
                                        output inserted.Id
                                        Values (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public void AssignChore(int chore, int roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"Insert into RoommateChore (RoommateId, ChoreId) output inserted.id values(@roommateid, @choreid)";
                    cmd.Parameters.AddWithValue("@roommateid", roommate);
                    cmd.Parameters.AddWithValue("@choreid", chore);
                    cmd.ExecuteScalar();

                    int success = (int)cmd.ExecuteScalar();

                    Console.WriteLine($"You have successfully added another chore!");
                }
            }

        }

        public List<RoommateChoreCount> GetChoreCount()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select r.FirstName, count(r.firstname) as Count
                                        from RoommateChore rc join Roommate r on rc.RoommateId = r.id
                                        group by r.FirstName;";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<RoommateChoreCount> counts = new List<RoommateChoreCount>();

                    while (reader.Read())
                    {


                        int fnameColumn = reader.GetOrdinal("FirstName");
                        string firstname = reader.GetString(fnameColumn);

                        int countColumn = reader.GetOrdinal("Count");
                        int chorecount = reader.GetInt32(countColumn);

                        RoommateChoreCount count = new RoommateChoreCount
                        {
                            FirstName = firstname,
                            NumberChores = chorecount
                        };

                        counts.Add(count);

                    }
                    reader.Close();
                    return counts;
                }
            }
        }

        ///Add a chore
        ///

        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Update chore
                                        set Name = @name
                                        where Id = @id";
                    cmd.Parameters.AddWithValue("@id", chore.Id);
                    cmd.Parameters.AddWithValue("@name", chore.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Delete from chore where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
