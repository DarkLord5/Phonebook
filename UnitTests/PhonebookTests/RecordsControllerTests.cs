using Xunit;
using Phonebook.Services;
using FakeItEasy;
using Phonebook.ViewModels;
using Phonebook.Models;
using System.Collections.Generic;
using Phonebook.Controllers;
using System.Threading.Tasks;
using Phonebook.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System;

namespace PhonebookTests
{
    public class RecordsControllerTests
    {

        private readonly List<RecordViewModel> expectRes = new()
        {
            new RecordViewModel() { Id = 1, Name = "Petr" },
            new RecordViewModel() { Id = 2, Name = "Jhon" }
        };

        private readonly Phonebook.Models.Record newUser = new()
        {
            Id = 3,
            Name = "Maksim",
            Surname = "Basharov",
            FatherName = "Ivanovich",
            Position = "Secretary",
            SubdivisionID = 1,
            PersonalNumber = new List<string> { "+375295676767" },
            WorkMobileNumber = new List<string> { "+375295676768" },
            WorkNumber = new List<string> { "+375295676768" }
        };

        private readonly Phonebook.Models.Record updateUser = new()
        {
            Id = 3,
            Name = "Andrey",
            Surname = "Basharov",
            FatherName = "Ivanovich",
            Position = "Secretary",
            SubdivisionID = 1,
            PersonalNumber = new List<string> { "+375295676767" },
            WorkMobileNumber = new List<string> { "+375295676768" },
            WorkNumber = new List<string> { "+375295676768" }
        };

        private readonly Phonebook.Models.Record badNewUser = new()
        {
            Id = 4,
            Name = "Ivan",
            Surname = "Basharov",
            FatherName = "Ivanovich",
            Position = "Director",
            SubdivisionID = 1
        };

        private readonly List<RecordViewModel>? badResponse = null;

        private readonly List<int> deleteList = new() { 1, 3 };

        [Fact]
        public async Task Success_Get_All_RecordsAsync()
        {
            //Arrange


            var mok = A.Fake<IRecordService>();

            A.CallTo(() => mok.SearchAndGetRecordsAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored,
                A<string>.Ignored, A<string>.Ignored)).Returns(expectRes);

            var controller = new RecordsController(mok);

            //Act
            var result = await controller.GetFilteredRecords("", "", "", "", "");

            //Assert
            Assert.Equal(expectRes, result.Value);
        }


        [Fact]
        public async Task Success_Create_RecordsAsync()
        {
            //Arrange
            var mok = A.Fake<IRecordService>();

            expectRes.Add(new RecordViewModel { Id = 3, Name = "Maksim" });

            A.CallTo(() => mok.CreateRecordAsync(newUser)).Returns(expectRes);

            var controller = new RecordsController(mok);

            //Act
            var result = await controller.CreateRecord(newUser);

            //Assert
            Assert.Equal(expectRes, result.Value);
        }


        [Fact]
        public async Task Failed_Create_Records_Not_Enough_Required_DataAsync()
        {
            //Arrange

            var mok = A.Fake<IRecordService>();

            A.CallTo(() => mok.CreateRecordAsync(badNewUser)).Returns(badResponse);

            var controller = new RecordsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.CreateRecord(badNewUser));
        }


        [Fact]
        public async Task Success_Update_RecordsAsync()
        {
            //Arrange
            var mok = A.Fake<IRecordService>();

            expectRes.Add(new RecordViewModel { Id = 3, Name = "Andrey" });

            A.CallTo(() => mok.UpdateRecordAsync(updateUser, 3)).Returns(expectRes);

            var controller = new RecordsController(mok);

            //Act
            var result = await controller.UpdateRecord(newUser.Id, updateUser);

            //Assert
            Assert.Equal(expectRes, result.Value);
        }

        [Fact]
        public async Task Failed_Update_Records_Not_Enough_Required_DataAsync()
        {
            //Arrange

            var mok = A.Fake<IRecordService>();

            A.CallTo(() => mok.UpdateRecordAsync(badNewUser, 3)).Returns(badResponse);

            var controller = new RecordsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.UpdateRecord(newUser.Id, badNewUser));
        }


        [Fact]
        public async Task Success_Delete_Records_ListAsync()
        {
            //Arrange
            var mok = A.Fake<IRecordService>();

            expectRes.Remove(new RecordViewModel { Id = 1, Name = "Petr" });

            A.CallTo(() => mok.DeleteRecordsAsync(deleteList)).Returns(expectRes);

            var controller = new RecordsController(mok);

            //Act
            var result = await controller.DeleteRecord(deleteList);

            //Assert
            Assert.Equal(expectRes, result.Value);
        }


        [Fact]
        public async Task Failed_Delete_Records_EmptyListAsync()
        {
            //Arrange

            var mok = A.Fake<IRecordService>();

            deleteList.Clear();

            A.CallTo(() => mok.DeleteRecordsAsync(deleteList)).Returns(badResponse);

            var controller = new RecordsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.DeleteRecord(deleteList));
        }

        [Fact]
        public async Task Success_Find_Records_By_SubdivisionAsync()
        {
            //Arrange
            var mok = A.Fake<IRecordService>();

            A.CallTo(() => mok.FindBySubdivisionAsync(1)).Returns(expectRes);

            var controller = new RecordsController(mok);

            //Act
            var result = await controller.GetSubdivisionRecords(newUser.SubdivisionID);

            //Assert
            Assert.Equal(expectRes, result.Value);
        }

        [Fact]
        public async Task Failed_Find_Records_By_Subdivision_Does_Not_ExistAsync()
        {
            //Arrange

            var mok = A.Fake<IRecordService>();

            deleteList.Clear();

            A.CallTo(() => mok.FindBySubdivisionAsync(0)).Returns(badResponse);

            var controller = new RecordsController(mok);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.GetSubdivisionRecords(0));
        }


    }
}