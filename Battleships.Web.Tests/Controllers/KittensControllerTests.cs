namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Services;
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class KittensControllerTests
    {
        private const string TestKittenName = "My Test Kitten";
        private const string NameField = "kittenName";
        private KittensController controller;
        private IKittensRepository fakeKittensRepo;
        private IKittenUploadService fakeKittenUploadService;
        private HttpPostedFileBase fakeImageFile;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakeKittensRepo = A.Fake<IKittensRepository>();
            fakeKittenUploadService = A.Fake<IKittenUploadService>();
            controller = new KittensController(fakeKittensRepo, fakeKittenUploadService) { ControllerContext = GetFakeControllerContext() };
        }

        [Test]
        public void Index_returns_index_view()
        {
            // When
            var view = controller.Index();

            // Then
            Assert.That(view, IsMVC.View(""));
        }

        [Test]
        public void Posting_to_index_redirects_to_index()
        {
            // When
            var result = controller.Index(new FormCollection { { NameField, TestKittenName } });

            // Then
            Assert.That(result, IsMVC.RedirectTo(MVC.Kittens.Index()));
        }

        [Test]
        public void Posting_image_file_delegates_to_kitten_upload_service()
        {
            // When
            controller.Index(new FormCollection { { NameField, TestKittenName } });

            // Then
            A.CallTo(() => fakeKittenUploadService.UploadAndGetKitten(TestKittenName, fakeImageFile, TestPlayerStore.Directory))
             .MustHaveHappened();
        }

        [Test]
        public void Posting_image_file_saves_new_kitten()
        {
            // Given
            var fakeKitten = A.Fake<Kitten>();
            A.CallTo(() => fakeKittenUploadService.UploadAndGetKitten(A<string>._, A<HttpPostedFileBase>._, A<string>._))
             .Returns(fakeKitten);

            // When
            controller.Index(new FormCollection { { NameField, TestKittenName } });

            // Then
            A.CallTo(() => fakeKittensRepo.Add(fakeKitten)).MustHaveHappened();
            A.CallTo(() => fakeKittensRepo.SaveContext()).MustHaveHappened();
        }

        private ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = A.Fake<HttpContextBase>();
            var fakeRequest = A.Fake<HttpRequestBase>();
            var fileCollection = A.Fake<HttpFileCollectionBase>();
            fakeImageFile = A.Fake<HttpPostedFileBase>();

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeRequest);
            A.CallTo(() => fakeRequest.Files).Returns(fileCollection);
            A.CallTo(() => fileCollection["kittenFile"]).Returns(fakeImageFile);

            return new ControllerContext(fakeHttpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}
