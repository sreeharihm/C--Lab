using MongoDB.Bson;

namespace ASPNET_Core_MONGO_DB.DbModel
{
    public class Student
    {
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }
}
