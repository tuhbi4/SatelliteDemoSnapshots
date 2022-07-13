using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using SatelliteDemoSnapshots.DemoSnapshots.BL.API.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Tests.Integration
{
    public class DemoSnapshotControllerTestsIntegration

    {
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            var application = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
        });
            this.client = application.CreateClient();
        }

        [Test]
        public async Task Get_GetAllAsync_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            string Uri = "/DemoSnapshot";

            // Act
            var response = await client.GetAsync(Uri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Get_GetByIdAsync_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            string Uri = "/DemoSnapshot/1";
            string expectedJsonString = "{\"id\":1,\"satellite\":\"Kanopus\",\"shootingDate\":\"10.01.2022 00:00:00\",\"cloudiness\":50.00,\"turn\":2," +
                "\"coordinates\":\"POLYGON ((3.6718750000000044 25.73989230448949, 14.921875000000004 25.73989230448949," +
                " 14.921875000000004 18.916011030403887, 3.6718750000000044 18.916011030403887, 3.6718750000000044 25.73989230448949))\"}";

            // Act
            var response = await client.GetAsync(Uri);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expectedJsonString, result);
        }

        [Test]
        public async Task Post_PostAsync_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            string Uri = "/DemoSnapshot/";

            DemoSnapshotModel expectedDemoSnapshotModel = new DemoSnapshotModel()
            {
                Id = 0,
                Satellite = "Kanopus",
                ShootingDate = "10.01.2022 00:00:00",
                Cloudiness = 50.00M,
                Turn = 2,
                Coordinates = "POLYGON ((3.6718750000000044 25.73989230448949, 14.921875000000004 25.73989230448949, " +
                    "14.921875000000004 18.916011030403887, 3.6718750000000044 18.916011030403887, 3.6718750000000044 25.73989230448949))"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(expectedDemoSnapshotModel), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(Uri, stringContent);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("1", result);
        }

        [Test]
        public async Task Put_PutAsync_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            string Uri = "/DemoSnapshot/";

            DemoSnapshotModel baseDemoSnapshotModel = new DemoSnapshotModel()
            {
                Id = 0,
                Satellite = "Kanopus",
                ShootingDate = "10.01.2022 00:00:00",
                Cloudiness = 50.00M,
                Turn = 2,
                Coordinates = "POLYGON ((1 1, 3 1, 3 3, 1 3, 1 1))"
            };

            var baseModelStringContent = new StringContent(JsonConvert.SerializeObject(baseDemoSnapshotModel), Encoding.UTF8, "application/json");
            await client.PostAsync(Uri, baseModelStringContent);
            var baseGetAllResponce = await client.GetAsync(Uri);
            List<DemoSnapshotModel> basePresentation = JsonConvert.DeserializeObject<List<DemoSnapshotModel>>(baseGetAllResponce.Content.ReadAsStringAsync().Result);

            var expectedObjectsCount = basePresentation.Count;
            var addedObjectId = basePresentation[expectedObjectsCount - 1].Id;

            DemoSnapshotModel expectedDemoSnapshotModel = new DemoSnapshotModel()
            {
                Id = addedObjectId,
                Satellite = "KOMPSAT",
                ShootingDate = "22.02.2000 00:00:00",
                Cloudiness = 22.22M,
                Turn = 22,
                Coordinates = "POLYGON ((1 1, 2 1, 3 1, 3 3, 1 3, 1 1))"
            };
            var expectedModelStringContent = new StringContent(JsonConvert.SerializeObject(expectedDemoSnapshotModel), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(Uri + $"{addedObjectId}", expectedModelStringContent);
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("1", result);

            var finalGetAllResponce = await client.GetAsync(Uri);
            List<DemoSnapshotModel> finalPresentation = JsonConvert.DeserializeObject<List<DemoSnapshotModel>>(finalGetAllResponce.Content.ReadAsStringAsync().Result);
            var actualObjectsCount = finalPresentation.Count;
            Assert.AreEqual(expectedObjectsCount, actualObjectsCount);

            DemoSnapshotModel actualObject = finalPresentation[actualObjectsCount - 1];
            Assert.AreEqual(addedObjectId, actualObject.Id);
            Assert.AreEqual(expectedDemoSnapshotModel.Satellite, actualObject.Satellite);
            Assert.AreEqual(expectedDemoSnapshotModel.ShootingDate, actualObject.ShootingDate);
            Assert.AreEqual(expectedDemoSnapshotModel.Cloudiness, actualObject.Cloudiness);
            Assert.AreEqual(expectedDemoSnapshotModel.Turn, actualObject.Turn);
            Assert.AreEqual(expectedDemoSnapshotModel.Coordinates, actualObject.Coordinates);
        }

        [Test]
        public async Task Delete_DeleteAsync_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            string Uri = "/DemoSnapshot/";

            DemoSnapshotModel baseDemoSnapshotModel = new DemoSnapshotModel()
            {
                Id = 0,
                Satellite = "Kanopus",
                ShootingDate = "10.01.2022 00:00:00",
                Cloudiness = 50.00M,
                Turn = 2,
                Coordinates = "POLYGON ((1 1, 3 1, 3 3, 1 3, 1 1))"
            };

            var baseModelStringContent = new StringContent(JsonConvert.SerializeObject(baseDemoSnapshotModel), Encoding.UTF8, "application/json");
            await client.PostAsync(Uri, baseModelStringContent);
            var baseGetAllResponce = await client.GetAsync(Uri);
            List<DemoSnapshotModel> basePresentation = JsonConvert.DeserializeObject<List<DemoSnapshotModel>>(baseGetAllResponce.Content.ReadAsStringAsync().Result);

            var expectedObjectsCount = basePresentation.Count;
            var addedObjectId = basePresentation[expectedObjectsCount - 1].Id;

            // Act
            var response = await client.DeleteAsync(Uri + $"{addedObjectId}");
            var result = response.Content.ReadAsStringAsync().Result;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("1", result);

            var finalGetAllResponce = await client.GetAsync(Uri);
            List<DemoSnapshotModel> finalPresentation = JsonConvert.DeserializeObject<List<DemoSnapshotModel>>(finalGetAllResponce.Content.ReadAsStringAsync().Result);
            var actualObjectsCount = finalPresentation.Count;
            Assert.AreEqual(expectedObjectsCount - 1, actualObjectsCount);
        }
    }
}