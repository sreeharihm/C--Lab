using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASPNET_Core_COSMOS_DB.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace ASPNET_Core_COSMOS_DB.Repositories
{
    public interface IDbCollectionOperationsRepository<TModel, in TPk>
    {
        Task<IEnumerable<TModel>> GetItemsFromCollectionAsync();
        Task<TModel> GetItemFromCollectionAsync(TPk id);
        Task<TModel> AddDocumentIntoCollectionAsync(TModel item);
        Task<TModel> UpdateDocumentFromCollection(TPk id, TModel item);
        Task DeleteDocumentFromCollectionAsync(TPk id);
    }
    public class DbCollectionOperationsRepository: IDbCollectionOperationsRepository<PersonalInformationModel, string>
    {
        private static readonly string EndPoint = "https://cosmossreehari.documents.azure.com:443/";
        private static readonly string Key = "skHIFhMzevrzobNnYTlp2H3ARTGMyYIc79EDArinzAfginXMdJWvU3kFL9o1qFpPi8uGYHuhYH98OBm7yaEykg==";
        private static readonly string DatabaseId = "PersonalInformationDB";
        private static readonly string CollectionId = "PersonalInfoCollection";
        private static DocumentClient docClient;
        public DbCollectionOperationsRepository()
        {
            docClient = new DocumentClient(new Uri(EndPoint),Key );
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch(DocumentClientException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await docClient.CreateDatabaseAsync(new Database {Id = DatabaseId});
                }
                else
                {
                    throw;
                }
            }
        }
        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId,CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await docClient.CreateDocumentCollectionAsync( UriFactory.CreateDatabaseUri(DatabaseId), new DocumentCollection { Id=CollectionId}, new RequestOptions { OfferThroughput = 1000}
                        );
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<PersonalInformationModel>> GetItemsFromCollectionAsync()
        {
            var documents = docClient
                .CreateDocumentQuery<PersonalInformationModel>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions {MaxItemCount = -1}).AsDocumentQuery();
            List<PersonalInformationModel> persons = new List<PersonalInformationModel>();
            while (documents.HasMoreResults)
            {
                persons.AddRange(await documents.ExecuteNextAsync<PersonalInformationModel>());
            }
            return persons;
        }

        public async Task<PersonalInformationModel> GetItemFromCollectionAsync(string id)
        {
            try
            {
                var doc =
                    await docClient.ReadDocumentAsync<PersonalInformationModel>(
                        UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return doc.Document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {

                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<PersonalInformationModel> AddDocumentIntoCollectionAsync(PersonalInformationModel item)
        {
            try
            {
                var document =
                    await docClient.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
                var res = document.Resource;
                var person = JsonConvert.DeserializeObject<PersonalInformationModel>(res.ToString());
                return person;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PersonalInformationModel> UpdateDocumentFromCollection(string id, PersonalInformationModel item)
        {
            try
            {
                var document =
                    await docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id),
                        item);
                var data = document.Resource.ToString();
                var person = JsonConvert.DeserializeObject<PersonalInformationModel>(data);
                return person;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteDocumentFromCollectionAsync(string id)
        {
            await docClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId,id));
        }
    }
}
