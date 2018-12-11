
using System;
using Newtonsoft.Json;

namespace My_BlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            BlockChain sreeCoin = new BlockChain();
            sreeCoin.CreateTransaction(new Transaction("Henry", "MaHesh", 10));
            sreeCoin.ProcessPendingTransactions("Bill");
            Console.WriteLine(JsonConvert.SerializeObject(sreeCoin, Formatting.Indented));

            sreeCoin.CreateTransaction(new Transaction("MaHesh", "Henry", 5));
            sreeCoin.CreateTransaction(new Transaction("MaHesh", "Henry", 5));
            sreeCoin.ProcessPendingTransactions("Bill");

            sreeCoin.CreateTransaction(new Transaction("Rahul", "Mahesh", 15));
            sreeCoin.CreateTransaction(new Transaction("Rahul", "Ram", 15));
            sreeCoin.ProcessPendingTransactions("John");
            var endTime = DateTime.Now;

            Console.WriteLine($"Duration: {endTime - startTime}");

            Console.WriteLine("=========================");
            Console.WriteLine($"Henry' balance: {sreeCoin.GetBalance("Henry")}");
            Console.WriteLine($"MaHesh' balance: {sreeCoin.GetBalance("MaHesh")}");
            Console.WriteLine($"Bill' balance: {sreeCoin.GetBalance("Bill")}");


            Console.WriteLine($"Rahul' balance: {sreeCoin.GetBalance("Rahul")}");
            Console.WriteLine($"Ram' balance: {sreeCoin.GetBalance("Ram")}");
            Console.WriteLine($"John' balance: {sreeCoin.GetBalance("John")}");

            Console.WriteLine("=========================");
            Console.WriteLine($"sreeCoin");
            Console.WriteLine(JsonConvert.SerializeObject(sreeCoin, Formatting.Indented));
            /* Without reward
            BlockChain sreeCoin = new BlockChain();
            sreeCoin.CreateTransaction(new Transaction("Henry", "MaHesh", 10));
            sreeCoin.ProcessPendingTransactions("Bill");
            Console.WriteLine(JsonConvert.SerializeObject(sreeCoin, Formatting.Indented));
            */
            /*-- Proof of work
            BlockChain sreeCoin = new BlockChain();
            var startTime = DateTime.Now;

            sreeCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            sreeCoin.AddBlock(new Block(DateTime.Now, null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            sreeCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));

            var endTime = DateTime.Now;

            Console.WriteLine($"Duration: {endTime - startTime}");
            Console.WriteLine(JsonConvert.SerializeObject(sreeCoin, Formatting.Indented));
            */

            /*--Blocking adding and tamparing
            sreeCoin.AddBlock(new Block(DateTime.Now,null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            sreeCoin.AddBlock(new Block(DateTime.Now, null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            sreeCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));

            Console.WriteLine(JsonConvert.SerializeObject(sreeCoin, Formatting.Indented));
            Console.WriteLine($"Is Chain Valid: {sreeCoin.IsValid()}");
            Console.WriteLine($"Update amount to 1000");
            sreeCoin.Chain[1].Data = "{sender:Henry,receiver:MaHesh,amount:1000}";
            sreeCoin.Chain[1].Hash = sreeCoin.Chain[1].CalculateHash();
            Console.WriteLine($"Update the entire chain");
            sreeCoin.Chain[2].PreviousHash = sreeCoin.Chain[1].Hash;
            sreeCoin.Chain[2].Hash = sreeCoin.Chain[2].CalculateHash();
            sreeCoin.Chain[3].PreviousHash = sreeCoin.Chain[2].Hash;
            sreeCoin.Chain[3].Hash = sreeCoin.Chain[3].CalculateHash();
            Console.WriteLine($"Is Chain Valid: {sreeCoin.IsValid()}");*/
            Console.ReadKey();
        }
    }
}
