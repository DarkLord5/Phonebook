using FakeItEasy;
using Phonebook.Controllers;
using Phonebook.Models;
using Phonebook.Services;
using Phonebook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests
{
    public class UserControllerTests
    {

        private readonly RegistrationViewModel newUser = new()
        {
            Name = "Tad",
            Surname = "Mozby",
            Email = "tadmozby@gmail.com",
            Password = "Ab1Esdadwa"
        };

        private readonly RegistrationViewModel badNewUser = new()
        {
            Name = "Tad",
            Surname = "Mozby",
            Email = "tadmozby@gmail.com"
        };

        private readonly User? badResponse = null;

        private readonly User userExpected = new()
        {
            FirstName = "Tad",
            SecondName = "Mozby",
            Email = "tadmozby@gmail.com"
        };

        private readonly LoginViewModel logUser = new()
        {
            Email = "tadmozby@gmail.com",
            Password = "Ab1Esdadwa"
        };

        private readonly LoginViewModel badLogUser = new()
        {
            Email = "tadmozby@gmail.com",
            Password = "121212312"
        };
        
        [Fact]
        public async Task Success_RegistrationAsync()
        {
            //Arrange
            var mok = A.Fake<IUserService>();

            A.CallTo(() => mok.Registration(newUser)).Returns(userExpected);

            var controller = new UserController(mok);

            //Act
            var result = await controller.Registration(newUser);

            //Assert
            Assert.Equal(userExpected, result.Value);
        }


        [Fact]
        public async Task Failed_Registration_Not_Enough_Required_DataAsync()
        {
            //Arrange

            var mok = A.Fake<IUserService>();

            A.CallTo(() => mok.Registration(badNewUser)).Returns(badResponse);

            var controller = new UserController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.Registration(badNewUser));
        }


        [Fact]
        public async Task Success_AuthorizationAsync()
        {
            //Arrange
            var mok = A.Fake<IUserService>();

            A.CallTo(() => mok.Login(logUser)).Returns(true);

            var controller = new UserController(mok);

            //Act
            var result = await controller.Login(logUser);

            //Assert
            Assert.True(result.Value);
        }


        [Fact]
        public async Task Failed_Authorization_Invalid_DataAsync()
        {
            //Arrange
            var mok = A.Fake<IUserService>();

            A.CallTo(() => mok.Login(badLogUser)).Returns(false);

            var controller = new UserController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.Login(badLogUser));
        }
    }
}
