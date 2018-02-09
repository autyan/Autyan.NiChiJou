using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autyan.NiChiJou.Core.Repository.DbConnectionFactory;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Dapper;
using Autyan.NiChiJou.Repository.Dapper.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass]
    public class DapperRepositoryTest
    {
        [TestMethod]
        public void SelectTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            BaseDbConnectionFactory.SetConfigurationRoot(builder);
            DapperConfiguration.UseMssql();
            MetadataContext.Instance.Initilize(AppDomain.CurrentDomain.GetAssemblies());
            var totalInsert = 1;
            var random = new Random();
            for (var i = 0; i < totalInsert; i++)
            {
                var length = random.Next(4, 20);
                var user = new LoginUser
                {
                    LoginName = RandomString(random, length),
                    PasswordHash = "userlogin",
                    SecuritySalt = "salt",
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false
                };
                var repo = new IdentityUserRepository();
                var result = Task.Factory.StartNew(() => repo.InsertAsync(user)).Result.Result;
                Console.WriteLine(result);
            }
        }

        public static string RandomString(Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
