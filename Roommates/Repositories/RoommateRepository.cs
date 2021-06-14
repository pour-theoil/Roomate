using Roommates.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString)
        {

        }

        public List<Roommate> GetAll()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select * from Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int fnameColumn = reader.GetOrdinal("FirstName");
                        string firstname = reader.GetString(fnameColumn);

                        int lnameColumn = reader.GetOrdinal("LastName");
                        string lastname = reader.GetString(lnameColumn);

                        int rentColumn = reader.GetOrdinal("RentPortion");
                        int rent = reader.GetInt32(rentColumn);

                        int roomColmn = reader.GetOrdinal("RoomId");
                        int room = reader.GetInt32(roomColmn);


                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstname,
                            LastName = lastname,
                            RentPortion = rent,
                        };

                        roommates.Add(roommate);

                    }

                    reader.Close();
                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select m.FirstName, m.RentPortion, r.Name from Roommate m join Room r on m.RoomId = r.Id where m.id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    Room room = null;

                    if (reader.Read())
                    {
                        room = new Room
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = room
                        };
                    }

                    reader.Close();
                    return roommate;

                }
            }
        }
    }
}
