using Roommates.Models;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString)
        {

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
