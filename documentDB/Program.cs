using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using documentDb.Model;
using documentDB.Model;

namespace documentDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to run");
            Console.ReadLine();

            Run();

            Console.ReadLine();
        }

        private static async void Run()
        {
            DocumentClient documentClient = new DocumentClient(new Uri("https://schooltest.documents.azure.com:443/"),
                "Of414DnEibhmqeSTB0pKShdQtR6dxxfXP8uT93pggxoumKthJGzuECCUJZ27Pqf846VUkFfOrraMTQ74PZaywA==");

            Database database = await CreateDatabase(documentClient);

            DocumentCollection documentCollection = await CreateDocumentCollection(documentClient, database);

            //await CreateCourse(documentClient, documentCollection);

            //await UpdateCourse(new Guid("2f3ffd36-6011-4e35-9d94-3955b8d4d960"), documentCollection);

            await DeleteCourse(new Guid("2f3ffd36-6011-4e35-9d94-3955b8d4d960"), documentCollection);

            Console.WriteLine("database created successfully!");

            Console.ReadLine();
        }

        private static async Task DeleteCourse(Guid guid, DocumentCollection documentCollection)
        {
            DocumentClient documentClient = new DocumentClient(new Uri("https://schooltest.documents.azure.com:443/"),
                "Of414DnEibhmqeSTB0pKShdQtR6dxxfXP8uT93pggxoumKthJGzuECCUJZ27Pqf846VUkFfOrraMTQ74PZaywA==");

            Course course = documentClient.CreateDocumentQuery<Course>(documentCollection.DocumentsLink,
                new SqlQuerySpec(string.Format("SELECT * FROM c WHERE c.CourseId = '{0}'", guid))).AsEnumerable().FirstOrDefault();

            if (course == null)
                return;

            await documentClient.DeleteDocumentAsync(course.SelfLink);
        }

        private static async Task UpdateCourse(Guid guid, DocumentCollection documentCollection)
        {
            DocumentClient documentClient = new DocumentClient(new Uri("https://schooltest.documents.azure.com:443/"),
                "Of414DnEibhmqeSTB0pKShdQtR6dxxfXP8uT93pggxoumKthJGzuECCUJZ27Pqf846VUkFfOrraMTQ74PZaywA==");

            Course course = documentClient.CreateDocumentQuery<Course>(documentCollection.DocumentsLink, 
                new SqlQuerySpec(string.Format("SELECT * FROM c WHERE c.CourseId = '{0}'", guid))).AsEnumerable().FirstOrDefault();

            if (course == null)
                return;

            course.Name = "THIS IS THE PERFECT NAME...2..";

            await documentClient.ReplaceDocumentAsync(course);
        }

        private static async Task CreateCourse(DocumentClient documentClient, DocumentCollection documentCollection)
        {
            Course course = new Course()
            {
                CourseId = Guid.NewGuid(),
                Name = "En",
                Teacher = new Teacher()
                {
                    TeacherId = Guid.NewGuid(),
                    FullName = "Adel Adel",
                    Age = 44
                },
                Students = new List<Model.Student>()
                {
                    new Student(){
                         FullName = "Adel Mohamed",
                         StudentId = Guid.NewGuid()
                    }
                }
                ,
                Sessions = new List<Session>(){
                    new Session(){
                        SessionId = Guid.NewGuid(),
                        Name = "Intro",
                        MaterialsCount = 10
                    },
                    new Session(){
                        SessionId = Guid.NewGuid(),
                        Name = "Ch1",
                        MaterialsCount = 3
                    }
                }
            };

            Document document = await documentClient.CreateDocumentAsync(documentCollection.DocumentsLink, course);
        }

        private static async Task<DocumentCollection> CreateDocumentCollection(DocumentClient documentClient, Database database)
        {
            DocumentCollection documentCollection = documentClient.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "courseDocumentCollection").AsEnumerable().FirstOrDefault();

            if (documentCollection == null)
            {
                documentCollection = await documentClient.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection()
                {
                    Id = "courseDocumentCollection"
                });
            }

            return documentCollection;
        }

        private static async Task<Database> CreateDatabase(DocumentClient documentClient)
        {
            Database database = documentClient.CreateDatabaseQuery().Where(c => c.Id == "courseDatabase").AsEnumerable().FirstOrDefault();
            if (database == null)
            {
                database = await documentClient.CreateDatabaseAsync(new Database()
                {
                    Id = "courseDatabase"
                });
            }
            return database;
        }
    }
}
