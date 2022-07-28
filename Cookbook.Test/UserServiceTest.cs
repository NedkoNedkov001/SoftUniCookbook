using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Core.Services;
using Cookbook.Infrastructure.Data.Models;
using Cookbook.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Test
{
    public class UserServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;


        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();

            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IRecipeService, RecipeService>()
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);

        }

        [Test]
        public async Task GetUsersAsyncValid()
        {
            var service = serviceProvider.GetService<IUserService>();

            var allUsers = await service.GetUsersAsync();

            Assert.AreEqual(2, allUsers.Count());
        }

        [Test]
        public async Task GetUsersAsyncInvalid()
        {
            var service = serviceProvider.GetService<IUserService>();

            var allUsers = await service.GetUsersAsync();

            Assert.AreNotEqual(allUsers.Count(), 1);
        }

        [Test]
        public void GetUserByUsernameAsyncInvalid()
        {
            var username = "Invalid";

            var service = serviceProvider.GetService<IUserService>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetUserByUsernameAsync(username));
        }

        [Test]
        public void GetUserByUsernameAsyncValid()
        {
            var username = "DummyOne";

            var service = serviceProvider.GetService<IUserService>();

            Assert.DoesNotThrowAsync(async () => await service.GetUserByUsernameAsync(username));
        }

        [Test]
        public async Task GetUserForViewByUsernameAsyncValid()
        {
            var username = "DummyOne";

            var service = serviceProvider.GetService<IUserService>();

            Assert.NotNull(await service.GetUserForViewByUsernameAsync(username));
        }

        [Test]
        public async Task GetUserForViewByUsernameAsyncInvalid()
        {
            var username = "Invalid";

            var service = serviceProvider.GetService<IUserService>();

            Assert.IsNull(await service.GetUserForViewByUsernameAsync(username));
        }

        [Test]
        public async Task GetHomeUserByUsernameAsyncValid()
        {
            var username = "DummyOne";

            var service = serviceProvider.GetService<IUserService>();

            Assert.NotNull(await service.GetHomeUserByUsernameAsync(username));
        }

        [Test]
        public async Task GetHomeUserByUsernameAsyncInvalid()
        {
            var username = "Invalid";

            var service = serviceProvider.GetService<IUserService>();

            Assert.IsNull(await service.GetHomeUserByUsernameAsync(username));
        }

        [Test]
        public async Task GetUserByIdAsyncValid()
        {
            var id = "00000000-0000-0000-0000-000000000001";

            var service = serviceProvider.GetService<IUserService>();

            Assert.DoesNotThrowAsync(async () => await service.GetUserByIdAsync(id));
        }

        [Test]
        public async Task GetUserByIdAsyncInvalid()
        {
            var id = "00000000-0000-0000-0000-000000000000";

            var service = serviceProvider.GetService<IUserService>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetUserByIdAsync(id));
        }

        [Test]
        public async Task GetUserForEditByIdAsyncValid()
        {
            var id = "00000000-0000-0000-0000-000000000001";

            var service = serviceProvider.GetService<IUserService>();

            Assert.NotNull(await service.GetUserForEditByIdAsync(id));
        }

        [Test]
        public async Task GetUserForEditByIdAsyncInvalid()
        {
            var id = "00000000-0000-0000-0000-000000000000";

            var service = serviceProvider.GetService<IUserService>();

            Assert.IsNull(await service.GetUserForEditByIdAsync(id));
        }

        [Test]
        public async Task GetUserByEmailAsyncValid()
        {
            var email = "dummyone@email.com";

            var service = serviceProvider.GetService<IUserService>();

            Assert.DoesNotThrowAsync(async () => await service.GetUserByEmailAsync(email));
        }

        [Test]
        public async Task GetUserByEmailAsyncInvalid()
        {
            var email = "invalid@email.com";

            var service = serviceProvider.GetService<IUserService>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetUserByEmailAsync(email));
        }

        [Test]
        public async Task GetUserForViewByEmailAsyncValid()
        {
            var email = "dummyone@email.com";

            var service = serviceProvider.GetService<IUserService>();

            Assert.NotNull(await service.GetUserForViewByEmailAsync(email));

        }

        [Test]
        public async Task GetUserForViewByEmailAsyncInvalid()
        {
            var email = "invalid@email.com";

            var service = serviceProvider.GetService<IUserService>();

            Assert.IsNull(await service.GetUserForViewByEmailAsync(email));

        }

        [Test]
        public async Task UpdateUserAsyncTakenUsername()
        {
            var editUser = new UserEditViewModel()
            {
                Id = "00000000-0000-0000-0000-000000000001",
                Username = "DummyTwo",
                Email = "dummyOne@email.com"
            };

            var service = serviceProvider.GetService<IUserService>();

            var errorsList = await service.UpdateUserAsync(editUser);

            Assert.That("Username is already taken.", Is.EqualTo(errorsList.ElementAt(0)));
        }


        [Test]
        public async Task UpdateUserAsyncTakenEmail()
        {
            var editUser = new UserEditViewModel()
            {
                Id = "00000000-0000-0000-0000-000000000001",
                Username = "DummyOne",
                Email = "dummyTwo@email.com"
            };

            var service = serviceProvider.GetService<IUserService>();

            var errorsList = await service.UpdateUserAsync(editUser);

            Assert.That("Email is already taken.", Is.EqualTo(errorsList.ElementAt(0)));
        }

        [Test]
        public async Task UpdateUserAsyncValid()
        {
            var editUser = new UserEditViewModel()
            {
                Id = "00000000-0000-0000-0000-000000000001",
                Username = "DummyOneNew",
                Email = "dummyOne@email.com"
            };

            var service = serviceProvider.GetService<IUserService>();

            await service.UpdateUserAsync(editUser);
            var user = await service.GetUserByIdAsync(editUser.Id);

            Assert.That("DummyOneNew", Is.EqualTo(user.UserName));
        }

        [Test]
        public async Task DeleteUserAsyncValid()
        {
            string id = "00000000-0000-0000-0000-000000000001";

            var service = serviceProvider.GetService<IUserService>();

            var allUsers = await service.GetUsersAsync();

            Assert.That(allUsers.Count(), Is.EqualTo(2));

            Assert.IsTrue(await service.DeleteUserAsync(id));

            allUsers = await service.GetUsersAsync();

            Assert.That(allUsers.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddFavoriteAsyncValid()
        {
            string userId = "00000000-0000-0000-0000-000000000001";
            string recipeId = "10000000-0000-0000-0000-000000000000";

            var service = serviceProvider.GetService<IUserService>();
            var user = await service.GetUserByIdAsync(userId);

            Assert.That(user.Favorites.Count(), Is.EqualTo(0));

            await service.AddFavoriteAsync(userId, recipeId);

            user = await service.GetUserByIdAsync(userId);

            Assert.That(user.Favorites.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserFavoritesAsync()
        {
            string userId = "00000000-0000-0000-0000-000000000001";
            var service = serviceProvider.GetService<IUserService>();

            var favorites = await service.GetUserFavoritesAsync(userId);

            Assert.That(favorites.Count(), Is.EqualTo(0));
        }




        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var userOne = Activator.CreateInstance<ApplicationUser>();
            userOne.Id = "00000000-0000-0000-0000-000000000001";
            userOne.UserName = "DummyOne";
            userOne.NormalizedUserName = "DUMMYONE";
            userOne.Email = "dummyOne@email.com";
            userOne.NormalizedEmail = "DUMMYONE@EMAIL.COM";
            userOne.Picture = ImageToByteArray("../../../../SoftUniCookbook/wwwroot/img/AdminLTELogo.png");


            var userTwo = Activator.CreateInstance<ApplicationUser>();
            userTwo.Id = "00000000-0000-0000-0000-000000000002";
            userTwo.UserName = "DummyTwo";
            userTwo.NormalizedUserName = "DUMMYTWO";
            userTwo.Email = "dummyTwo@email.com";
            userTwo.NormalizedEmail = "DUMMYTWO@EMAIL.COM";
            userTwo.Picture = ImageToByteArray("../../../../SoftUniCookbook/wwwroot/img/AdminLTELogo.png");

            var recipeOne = new Recipe()
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000000"),
                Name = "DummyRecipeOne",
                Picture = ImageToByteArray("../../../../SoftUniCookbook/wwwroot/img/AdminLTELogo.png"),
                Description = "Dummy recipe one description",
                Instructions = "Dummy recipe one instructions",
                AuthorId = "00000000-0000-0000-0000-000000000001",
                ServingSize = 6
            };

            await repo.AddAsync(userOne);
            await repo.AddAsync(userTwo);
            await repo.AddAsync(recipeOne);
            await repo.SaveChangesAsync();
        }

        private byte[] ImageToByteArray(string path)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(path,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(path).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }

    }
}