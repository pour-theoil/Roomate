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

        public List<Chore> GetAssignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select c.Name, c.Id
from RoommateChore rc 
join Chore c on rc.ChoreId = c.Id
group by c.name, c.Id;";
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

        public List<Roommate> GetRoommateAssignedChore(int choreid)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select r.FirstName, r.Id
from RoommateChore rc join Roommate r on rc.RoommateId = r.Id
where rc.ChoreId = @Id";
                    cmd.Parameters.AddWithValue("@Id", choreid);

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> choremate = new List<Roommate>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("FirstName");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Roommate mate = new Roommate
                        {
                            Id = idValue,
                            FirstName = nameValue
                        };
                        choremate.Add(mate);
                    }
                    reader.Close();
                    return choremate;
                }
            }
        }

        public void Reassign(int choreid, int mateid, int target)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Update RoommateChore
                                        set RoommateId = @target
                                        where RoommateId = @mateid
                                        and ChoreId = @choreid";
                    cmd.Parameters.AddWithValue("@choreid", choreid);
                    cmd.Parameters.AddWithValue("@mateid", mateid);
                    cmd.Parameters.AddWithValue("@target", target);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
