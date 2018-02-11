using System;
using System.Linq;
using System.Threading.Tasks;
using Autyan.NiChiJou.Model.Extension;
using Autyan.NiChiJou.Model.Identity;
using Autyan.NiChiJou.Repository.Dapper;
using Autyan.NiChiJou.Repository.Dapper.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass]
    public class DapperRepositoryTest
    {
        public DapperRepositoryTest()
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true)
            //    .Build();
            //BaseDbConnectionFactory.SetConfigurationRoot(builder);
            DapperConfiguration.UseMssql();
            var serviceCollection = new ServiceCollection() as IServiceCollection;
            serviceCollection.AddNiChiJouDataModel();
            MetadataContext.Instance.Initilize(AppDomain.CurrentDomain.GetAssemblies());
        }

        [TestMethod]
        public void SelectTest()
        {
            var totalInsert = 500000;
            var random = new Random();
            for (var i = 0; i < totalInsert; i++)
            {
                var length = random.Next(4, 20);
                var user = new IdentityUser
                {
                    LoginName = RandomString(random, length),
                    PasswordHash = "userlogin",
                    SecuritySalt = "salt",
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false
                };
                var repo = new IdentityUserRepository();
                var queryUser = repo.FirstOrDefaultAsync(new {user.LoginName}).Result;
                if (queryUser != null) continue;
                var result = Task.Factory.StartNew(() => repo.InsertAsync(user)).Result.Result;
            }
        }

        public static string RandomString(Random random, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [TestMethod]
        public void UpdateTest()
        {
            var repo = new IdentityUserRepository();
            var result = Task.Factory.StartNew(() => repo.UpdateByConditionAsync(new {PasswordHash = "userlogin2"}, new {IdFrom = 10})).Result.Result;
            Console.WriteLine(result);
        }
    }
}
