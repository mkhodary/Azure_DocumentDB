using azure.console.documentDb.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azure.console.documentDb
{
    class Program
    {
        private const string CONST_EndPointUrl = "https://edutestdocument.documents.azure.com:443/";
        private const string CONST_AuthorizationKey = "XsG34MQlf5RnbM/Kt8WXnkEaQ8FLHY2IjzLNOGJ/FH7r9tOvTFWXmy0tr8+MSbHQ/URlcX3wMXIbJcJ3E7PYfg==";

        static void Main(string[] args)
        {
            //CreateDatabase();

            RunQuery();

            Console.ReadLine();
        }

        private static async void CreateDatabase()
        {
            DocumentClient client = new DocumentClient(new Uri(CONST_EndPointUrl), CONST_AuthorizationKey);

            Database database = await GetDatabase(client);

            DocumentCollection documentCollection = await GetDocumentCollection(client, database);

            Course course = new Course()
            {
                Id = "1",
                Name = "English",
                CreationDate = DateTime.Now.ToUniversalTime(),
                Teacher = new Teacher() { Age = 40, FullName = "Adel Mohamed" },
                Sessions = new List<Session>(){
                    new Session(){Id=3, Name="Introduction", MaterialsCount=10},
                    new Session(){Id=4, Name="First Lesson", MaterialsCount = 3}
                }
            };

            Document document = await client.CreateDocumentAsync(documentCollection.DocumentsLink, course);

        }

        private static async void RunQuery()
        {
            DocumentClient client = new DocumentClient(new Uri(CONST_EndPointUrl), CONST_AuthorizationKey);

            Database database = await GetDatabase(client);

            DocumentCollection documentCollection = await GetDocumentCollection(client, database);

            List<Course> coursesList = client.CreateDocumentQuery<Course>(documentCollection.DocumentsLink).Where(c => c.Name == "English").ToList();

            if (coursesList != null && coursesList.Count > 0)
            {
                foreach (var item in coursesList)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }

        //SELECT C.id AS CourseId, C.Name AS CourseName
        //from CourseCollection AS C
        //Join Session IN C.Sessions
        //WHERE Session.Name = "Introduction"

        #region Helpers
        private static async Task<DocumentCollection> GetDocumentCollection(DocumentClient client, Database database)
        {
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "CourseCollection").AsEnumerable().FirstOrDefault();
            if (documentCollection == null)
            {
                documentCollection = await client.CreateDocumentCollectionAsync(database.CollectionsLink, new DocumentCollection() { Id = "CourseCollection" });
            }
            return documentCollection;
        }

        private static async Task<Database> GetDatabase(DocumentClient client)
        {
            Database database = client.CreateDatabaseQuery().Where(c => c.Id == "SchoolRegistry").AsEnumerable().FirstOrDefault();
            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database() { Id = "SchoolRegistry" });
            }
            return database;
        }
        #endregion
    }
}
