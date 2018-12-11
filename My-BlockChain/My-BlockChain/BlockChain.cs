using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace My_BlockChain
{
    public class BlockChain
    {
        public int Difficulty { set; get; } = 2;
        public List<Block> Chain { get; set; }
        List<Transaction> _pendingTransactions = new List<Transaction>();

        public int Reward = 1; //1 cryptocurrency
        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, _pendingTransactions);
            AddBlock(block);

            _pendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }
        public void InitailizeChain()
        {
            Chain = new List<Block>();
        }
        public void CreateTransaction(Transaction transaction)
        {
            _pendingTransactions.Add(transaction);
        }
        public Block CreaetGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, _pendingTransactions);
            block.Mine(Difficulty);
            _pendingTransactions = new List<Transaction>();
            return block;
        }
        public void AddGensisBlock()
        {
            Chain.Add(CreaetGenesisBlock());
        }
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        public BlockChain()
        {
            InitailizeChain();
            AddGensisBlock();
        }

        public void AddBlock(Block block)
        {
            Block latest = GetLatestBlock();
            block.Index = latest.Index + 1;
            block.PreviousHash = latest.Hash;
            block.Hash = block.CalculateHash();
            block.Mine(this.Difficulty);
            Chain.Add(block);
        }
        public int GetBalance(string address)
        {
            int balance = 0;

            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];

                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }
        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
