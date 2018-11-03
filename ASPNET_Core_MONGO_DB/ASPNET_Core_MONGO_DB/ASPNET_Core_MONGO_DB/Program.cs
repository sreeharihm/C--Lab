using System;

namespace ASPNET_Core_MONGO_DB
{
    class Program
    {
        static void Main(string[] args)
        {
            DbCollectionRepository db = new DbCollectionRepository();
            db.AddItem();
            db.GetAll();             
            db.GetById();
            db.UpdateItem();
            db.DeleteItem();
            db.GetAll();
            Console.ReadLine();
        }
    }
}
