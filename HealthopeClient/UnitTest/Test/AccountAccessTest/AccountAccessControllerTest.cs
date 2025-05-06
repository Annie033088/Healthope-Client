using System.Web.Http;
using ApiLayer.Controllers.api;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestMemberDto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTest.utils;

namespace UnitTest.Test.MemberTest
{
    [TestClass]
    public class AccountAccessControllerTest
    {
        private AccountAccessController accountAccessController;
        private Mock<IAccountAccessService> accountAccessServiceMock;

        [TestInitialize]
        public void Setup()
        {
            accountAccessServiceMock = new Mock<IAccountAccessService>();
            accountAccessController = new AccountAccessController(accountAccessServiceMock.Object);
        }

        [TestMethod]
        public void 新增_成功_回傳成功()
        {
            // Arrange
            RequestAddMemberDto addMemberDto = new RequestAddMemberDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "qwe@ieu.tt",
                Phone = "0912345678",
            };

            // Mock 設定
            ErrorCodeDefine errorCode = ErrorCodeDefine.Success;
            accountAccessServiceMock.Setup(s => s.AddMember(addMemberDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = accountAccessController.AddMember(addMemberDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.Success)) return;

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 新增_失敗_回傳格式無效()
        {
            // Arrange
            RequestAddMemberDto addMemberDto = new RequestAddMemberDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "qwe@ieu.tt",
                Phone = "9123456789",
            };

            // Mock 設定

            // Act
            IHttpActionResult result = accountAccessController.AddMember(addMemberDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.InvalidFormatOrEntry)) return;

            // Assert
            Assert.Fail("測試出錯");
        }

        [TestMethod]
        public void 新增_失敗_回傳電話重複()
        {
            // Arrange
            RequestAddMemberDto addMemberDto = new RequestAddMemberDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "qwe@ieu.tt",
                Phone = "0912345567",
            };

            // Mock 設定
            ErrorCodeDefine errorCode = ErrorCodeDefine.DuplicateAccount;
            accountAccessServiceMock.Setup(s => s.AddMember(addMemberDto)).Returns(errorCode);

            // Act
            IHttpActionResult result = accountAccessController.AddMember(addMemberDto);

            ResponseErrorCodeIsEqual errorCodeIsEqual = new ResponseErrorCodeIsEqual();
            if (errorCodeIsEqual.ErrorCodeIsEqual(result, ErrorCodeDefine.DuplicateAccount)) return;

            // Assert
            Assert.Fail("測試出錯");
        }
    }
}
