using FakeItEasy;
using Phonebook.Controllers;
using Phonebook.Models;
using Phonebook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests
{
    public class SubdivisionsControllerTests
    {
        private readonly List<Subdivision> expectRes = new()
        {
            new Subdivision { Id = 1, Name = "Main Company", ParentId = null },
            new Subdivision { Id = 2, Name = "Financial Department", ParentId = 1 }
        };

        private readonly Subdivision newSubdivision = new()
        { 
            Name = "Accounting", 
            ParentId = 2 
        };

        private readonly Subdivision badSubdivision = new()
        {
            ParentId = 2
        };

        private readonly Subdivision? badResponse = null;

        private readonly List<Subdivision>? badResponseList = null;

        [Fact]
        public async Task Success_Get_All_SubdivisionAsync()
        {
            //Arrange
            var mok = A.Fake<ISubdivisionService>();

            A.CallTo(() => mok.GetAllSubdivisionsAsync()).Returns(expectRes);

            var controller = new SubdivisionsController(mok);

            //Act
            var result = await controller.GetSubdivisions();

            //Assert
            Assert.Equal(expectRes, result.Value);
        }



        [Fact]
        public async Task Success_Create_SubdivisionAsync()
        {
            //Arrange
            var mok = A.Fake<ISubdivisionService>();

            expectRes.Add(newSubdivision);

            A.CallTo(() => mok.CreateSubdivisionAsync(newSubdivision)).Returns(newSubdivision);

            var controller = new SubdivisionsController(mok);

            //Act
            var result = await controller.CreateSubdivision(newSubdivision);

            //Assert
            Assert.Equal(newSubdivision, result.Value);
        }


        [Fact]
        public async Task Failed_Create_Subdivision_Not_Enough_Required_DataAsync()
        {
            //Arrange

            var mok = A.Fake<ISubdivisionService>();

            A.CallTo(() => mok.CreateSubdivisionAsync(badSubdivision)).Returns(badResponse);

            var controller = new SubdivisionsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.CreateSubdivision(badSubdivision));
        }


        [Fact]
        public async Task Success_Update_SubdivisionAsync()
        {
            //Arrange
            var mok = A.Fake<ISubdivisionService>();

            A.CallTo(() => mok.UpdateSubdivisionAsync(1, newSubdivision)).Returns(newSubdivision);

            var controller = new SubdivisionsController(mok);

            //Act
            var result = await controller.UpdateSubdivision(1, newSubdivision);

            //Assert
            Assert.Equal(newSubdivision, result.Value);
        }


        [Fact]
        public async Task Failed_Update_Subdivision_Not_Enough_Required_DataAsync()
        {
            //Arrange

            var mok = A.Fake<ISubdivisionService>();

            A.CallTo(() => mok.UpdateSubdivisionAsync(1, badSubdivision)).Returns(badResponse);

            var controller = new SubdivisionsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.UpdateSubdivision(1, badSubdivision));
        }

        [Fact]
        public async Task Success_Delete_SubdivisionAsync()
        {
            //Arrange
            var mok = A.Fake<ISubdivisionService>();

            expectRes.Remove(new Subdivision { Id = 2, Name = "Financial Department", ParentId = 1 });

            A.CallTo(() => mok.DeleteSubdivisionAsync(2)).Returns(expectRes);

            var controller = new SubdivisionsController(mok);

            //Act
            var result = await controller.DeleteSubdivision(2);

            //Assert
            Assert.Equal(expectRes, result.Value);
        }


        [Fact]
        public async Task Failed_Delete_Subdivision_Does_Not_ExistAsync()
        {
            //Arrange

            var mok = A.Fake<ISubdivisionService>();

            A.CallTo(() => mok.DeleteSubdivisionAsync(0)).Returns(badResponseList);

            var controller = new SubdivisionsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.DeleteSubdivision(0));
        }
    }
}
