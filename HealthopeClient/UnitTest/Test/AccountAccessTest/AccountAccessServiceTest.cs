using ApiLayer.Models;
using ApiLayer.Models.AccountAccess.RequestMemberDto;
using ApiLayer.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace UnitTest.Test.MemberTest
{
    [TestClass]
    public class AccountAccessServiceTest
    {
        private Mock<IMemberRepository> memberRepositoryMock;
        private AccountAccessService accountAccessService;

        [TestInitialize]
        public void Setup()
        {
            memberRepositoryMock = new Mock<IMemberRepository>();
            accountAccessService = new AccountAccessService(accountAccessService.Object);
        }

        [TestMethod]
        public void 新增_成功_回傳成功()
        {
            // Arrange
            RequestAddMemberDto addAdminDto = new RequestAddMemberDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "qwe@ieu.tt",
                Phone = "0912345678",
            };

            // Mock 設定
            memberRepositoryMock.Setup(s => s.AddMember(It.IsAny<AddMemberDto>())).Returns((int)ErrorCodeDefine.Success);

            // Act
            ErrorCodeDefine errorCode = accountAccessService.AddMember(addAdminDto);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.Success);
        }

        [TestMethod]
        public void 新增_失敗_回傳帳號重複()
        {
            // Arrange
            RequestAddMemberDto addAdminDto = new RequestAddMemberDto()
            {
                Account = "eqweqw123",
                Pwd = "g4556fgerger",
                Email = "qwe@ieu.tt",
                Phone = "0912345678",
            };

            // Mock 設定
            memberRepositoryMock.Setup(s => s.AddMember(It.IsAny<AddMemberDto>())).Returns((int)ErrorCodeDefine.DuplicateAccount);

            // Act
            ErrorCodeDefine errorCode = accountAccessService.AddMember(addAdminDto);

            // Assert
            Assert.IsTrue(errorCode == ErrorCodeDefine.DuplicateAccount);
        }
    }
}
