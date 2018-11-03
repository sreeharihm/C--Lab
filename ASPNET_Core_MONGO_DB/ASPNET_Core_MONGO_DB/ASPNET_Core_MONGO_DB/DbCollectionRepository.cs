using System;
using System.Linq;
using ASPNET_Core_MONGO_DB.DbModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ASPNET_Core_MONGO_DB
{
    public class DbCollectionRepository
    {
        private readonly IMongoCollection<Student> _collection;
        public DbCollectionRepository()
        {
            IMongoClient client = new MongoClient();
            var database = client.GetDatabase("School");
            _collection = database.GetCollection<Student>("StudentDetails");
        }

        public void GetAll()
        {
            var results = _collection.Find(new BsonDocument()).ToList();
            foreach (Student t in results)
            {
                Console.WriteLine(t.Id + " -" + t.LastName);
            }

        }
        public void GetById()
        {
            Console.WriteLine("Please Enter the ObjectId : ");
            string id = Console.ReadLine();
            var result = _collection.Find(Builders<Student>.Filter.Eq("Id", ObjectId.Parse(id))).ToList().FirstOrDefault();
            if (result != null) Console.WriteLine(result.Id + " -" + result.LastName);
        }

        public int AddItem()
        {
            Console.Write("Enter the First Name");
            string fName = Console.ReadLine();
            Console.Write("Enter the Last Name");
            string lName = Console.ReadLine();
            Console.Write("Enter the Age");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the City");
            string city = Console.ReadLine();
            Student tModel = new Student { FirstName = fName, LastName = lName, Age = age, City = city };
            _collection.InsertOne(tModel);
            return 0;
        }

        public int UpdateItem()
        {
            Console.Write("Enter the First Name");
            string fName = Console.ReadLine();
            Console.Write("Enter the Last Name");
            string lName = Console.ReadLine();
            Console.WriteLine("Please Enter the ObjectId :  ");
            string ss = Console.ReadLine();
            _collection.FindOneAndUpdate<Student>
            (Builders<Student>.Filter.Eq("Id", ObjectId.Parse(ss)),
                Builders<Student>.Update.Set("LastName", lName).Set("FirstName", fName));
            return 0;
        }

        public int DeleteItem()
        {
            Console.WriteLine("Please Enter the ObjectId : ");
            string ss = Console.ReadLine();
            _collection.DeleteOne(s => s.Id == ObjectId.Parse(ss));
            return 0;
        }
    }
}
