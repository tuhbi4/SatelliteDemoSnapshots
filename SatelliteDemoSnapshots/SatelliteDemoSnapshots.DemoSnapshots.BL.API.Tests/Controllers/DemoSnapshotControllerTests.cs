using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Types;
using Moq;
using NUnit.Framework;
using SatelliteDemoSnapshots.DemoSnapshots.BL.API.Controllers;
using SatelliteDemoSnapshots.DemoSnapshots.BL.API.Helpers;
using SatelliteDemoSnapshots.DemoSnapshots.BL.API.Models;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;
using SatelliteDemoSnapshots.DemoSnapshots.DL.DAO.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace SatelliteDemoSnapshots.DemoSnapshots.BL.API.Tests.Controllers
{
    [TestFixture]
    public class DemoSnapshotControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<DemoSnapshotController>> _mockLogger;
        private Mock<IRepository<DemoSnapshot>> _mockRepository;
        private IMapper _mapper;

        private List<DemoSnapshot> _demoSnapshotsList;
        private List<DemoSnapshotModel> _demoSnapshotModelsList;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            _mockLogger = mockRepository.Create<ILogger<DemoSnapshotController>>();
            _mockRepository = mockRepository.Create<IRepository<DemoSnapshot>>();

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }));

            SetUpDemoSnapshotRepository();
            SetUpDemoSnapshotsList();
            SetUpDemoSnapshotModelsList();
        }

        [Test]
        public async Task GetAsync_GetAllDemoSnapshotsTest()
        {
            // Arrange
            var demoSnapshotController = CreateDemoSnapshotController();
            string query = "";

            // Act
            IActionResult result = await demoSnapshotController.GetAsync(query);
            var resultObject = (result as OkObjectResult).Value;

            // Assert
            Assert.IsNotNull(resultObject);
            var presentations = resultObject as IEnumerable<DemoSnapshotModel>;
            Assert.IsNotNull(presentations);
            Assert.AreEqual(presentations.Select(x => x.Id).Intersect(_demoSnapshotModelsList.Select(x => x.Id)).Count(),
                    presentations.Count());
        }

        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public async Task<int> GetAsync_GetDemoSnapshotByIdTest(int id)
        {
            // Arrange
            var demoSnapshotController = CreateDemoSnapshotController();
            var actual = _demoSnapshotModelsList.Find(x => x.Id.Equals(id));

            // Act

            IActionResult result = await demoSnapshotController.GetAsync(id);
            var resultObject = (result as OkObjectResult).Value;

            // Assert
            Assert.IsNotNull(resultObject);
            var presentation = resultObject as DemoSnapshotModel;
            Assert.IsNotNull(presentation);
            Assert.AreEqual(presentation.Id, actual.Id);
            Assert.AreEqual(presentation.Satellite, actual.Satellite);
            Assert.AreEqual(presentation.ShootingDate, actual.ShootingDate);
            Assert.AreEqual(presentation.Cloudiness, actual.Cloudiness);
            Assert.AreEqual(presentation.Turn, actual.Turn);
            Assert.AreEqual(presentation.Coordinates, actual.Coordinates);

            return await Task.FromResult(presentation.Id);
        }

        [Test]
        public async Task PostAsync_CreateDemoSnapshotTest()
        {
            // Arrange
            var demoSnapshotController = CreateDemoSnapshotController();

            DemoSnapshot actualDemoSnapshot = new DemoSnapshot()
            {
                Id = 3,
                Satellite = Satellites.KOMPSAT,
                ShootingDate = new System.DateTime(2000, 02, 22),
                Cloudiness = 22,
                Turn = 4,
                Coordinates = SqlGeography.Parse(new SqlString("POLYGON((3.6718750000000044 25.73989230448949,14.921875000000004 25.73989230448949," +
                "14.921875000000004 18.916011030403887,3.6718750000000044 18.916011030403887,3.6718750000000044 25.73989230448949))"))
            };

            DemoSnapshotModel demoSnapshotModel = new DemoSnapshotModel()
            {
                Id = 3,
                Satellite = "KOMPSAT",
                ShootingDate = "22.02.2000 00:00:00",
                Cloudiness = 22.00M,
                Turn = 4,
                Coordinates = "POLYGON ((3.6718750000000044 25.73989230448949,14.921875000000004 25.73989230448949," +
                "14.921875000000004 18.916011030403887,3.6718750000000044 18.916011030403887,3.6718750000000044 25.73989230448949))"
            };

            // Act
            IActionResult result = await demoSnapshotController.PostAsync(demoSnapshotModel);
            var resultObject = (result as OkObjectResult).Value;
            Int32.TryParse(resultObject.ToString(), out var newId);

            // Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(newId, actualDemoSnapshot.Id);
            var newDemoSnapshot = _demoSnapshotsList.Find(x => x.Id.Equals(newId));
            Assert.IsNotNull(newDemoSnapshot);
            Assert.AreEqual(newDemoSnapshot.Id, actualDemoSnapshot.Id);
            Assert.AreEqual(newDemoSnapshot.Satellite, actualDemoSnapshot.Satellite);
            Assert.AreEqual(newDemoSnapshot.ShootingDate, actualDemoSnapshot.ShootingDate);
            Assert.AreEqual(newDemoSnapshot.Cloudiness, actualDemoSnapshot.Cloudiness);
            Assert.AreEqual(newDemoSnapshot.Turn, actualDemoSnapshot.Turn);
            StringAssert.AreEqualIgnoringCase(newDemoSnapshot.Coordinates.ToString(), actualDemoSnapshot.Coordinates.ToString());
            StringAssert.Contains(newDemoSnapshot.Coordinates.ToString(), actualDemoSnapshot.Coordinates.ToString());
        }

        [Test]
        public async Task PutAsync_UpdateDemoSnapshotTest()
        {
            // Arrange
            var demoSnapshotController = CreateDemoSnapshotController();
            int id = 2;

            DemoSnapshot actualDemoSnapshot = new DemoSnapshot()
            {
                Id = 2,
                Satellite = Satellites.KOMPSAT,
                ShootingDate = new System.DateTime(2000, 02, 22),
                Cloudiness = 22,
                Turn = 4,
                Coordinates = SqlGeography.Parse(new SqlString("POLYGON((3.6718750000000044 25.73989230448949,14.921875000000004 25.73989230448949," +
                "14.921875000000004 18.916011030403887,3.6718750000000044 18.916011030403887,3.6718750000000044 25.73989230448949))"))
            };

            DemoSnapshotModel demoSnapshotModel = new DemoSnapshotModel()
            {
                Id = 2,
                Satellite = "KOMPSAT",
                ShootingDate = "22.02.2000 00:00:00",
                Cloudiness = 22.00M,
                Turn = 4,
                Coordinates = "POLYGON ((3.6718750000000044 25.73989230448949,14.921875000000004 25.73989230448949," +
                "14.921875000000004 18.916011030403887,3.6718750000000044 18.916011030403887,3.6718750000000044 25.73989230448949))"
            };

            // Act
            IActionResult result = await demoSnapshotController.PutAsync(id, demoSnapshotModel);
            var resultObject = (result as OkObjectResult).Value;
            Int32.TryParse(resultObject.ToString(), out var newId);

            // Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(newId, id);
            var newDemoSnapshot = _demoSnapshotsList.Find(x => x.Id.Equals(newId));
            Assert.IsNotNull(newDemoSnapshot);
            Assert.AreEqual(newDemoSnapshot.Id, actualDemoSnapshot.Id);
            Assert.AreEqual(newDemoSnapshot.Satellite, actualDemoSnapshot.Satellite);
            Assert.AreEqual(newDemoSnapshot.ShootingDate, actualDemoSnapshot.ShootingDate);
            Assert.AreEqual(newDemoSnapshot.Cloudiness, actualDemoSnapshot.Cloudiness);
            Assert.AreEqual(newDemoSnapshot.Turn, actualDemoSnapshot.Turn);
            StringAssert.AreEqualIgnoringCase(newDemoSnapshot.Coordinates.ToString(), actualDemoSnapshot.Coordinates.ToString());
            StringAssert.Contains(newDemoSnapshot.Coordinates.ToString(), actualDemoSnapshot.Coordinates.ToString());
        }

        [Test]
        public async Task DeleteAsync_DeleteDemoSnapshotTest()
        {
            // Arrange
            var demoSnapshotController = CreateDemoSnapshotController();
            int id = 1;

            // Act
            IActionResult result = await demoSnapshotController.DeleteAsync(id);
            var resultObject = (result as OkObjectResult).Value;
            Int32.TryParse(resultObject.ToString(), out var deletedId);

            // Assert
            Assert.IsNotNull(resultObject);
            Assert.AreEqual(deletedId, id);
            var newDemoSnapshot = _demoSnapshotsList.Find(x => x.Id.Equals(deletedId));
            Assert.IsNull(newDemoSnapshot);
        }

        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        public async Task<int> GetAsync_GetDemoSnapshotsByQueryTestAsync(int id)
        {
            // Arrange
            var demoSnapshotController = CreateDemoSnapshotController();
            string query = _demoSnapshotsList.Find(x => x.Id.Equals(id)).Coordinates.ToString();

            // Act
            IActionResult result = await demoSnapshotController.GetAsync(query);
            var resultObject = (result as OkObjectResult).Value;

            // Assert
            Assert.IsNotNull(resultObject);
            var presentations = resultObject as IEnumerable<DemoSnapshotModel>;
            Assert.IsNotNull(presentations);

            return await Task.FromResult(presentations.Count());
        }

        private DemoSnapshotController CreateDemoSnapshotController()
        {
            return new DemoSnapshotController(
                _mockLogger.Object,
                _mockRepository.Object,
                _mapper);
        }

        private void SetUpDemoSnapshotsList()
        {
            _demoSnapshotsList = new List<DemoSnapshot>
            {
                new DemoSnapshot()
                {
                    Id = 1,
                    Satellite = Satellites.Kanopus,
                    ShootingDate = new System.DateTime(2022, 01, 10),
                    Cloudiness = 50,
                    Turn = 2,
                    Coordinates = SqlGeography.Parse(new SqlString("POLYGON((3.6718750000000044 25.73989230448949,14.921875000000004 25.73989230448949," +
                "14.921875000000004 18.916011030403887,3.6718750000000044 18.916011030403887,3.6718750000000044 25.73989230448949))"))
                },
                new DemoSnapshot()
                {
                    Id = 2,
                    Satellite = Satellites.BS,
                    ShootingDate = new System.DateTime(2021, 02, 11),
                    Cloudiness = 10,
                    Turn = 5,
                    Coordinates = SqlGeography.Parse(new SqlString("POLYGON((35.23238462584629 57.60906073517954,35.54000181334629 56.16893557803185," +
                "37.03414243834629 55.972705249882125,37.95699400084629 57.30173453950533,35.23238462584629 57.60906073517954))"))
                }
            };
        }

        private void SetUpDemoSnapshotModelsList()
        {
            _demoSnapshotModelsList = new List<DemoSnapshotModel>
            {
                new DemoSnapshotModel()
                {
                    Id = 1,
                    Satellite = "Kanopus",
                    ShootingDate = "10.01.2022 00:00:00",
                    Cloudiness = 50.00M,
                    Turn = 2,
                    Coordinates = "POLYGON ((3.6718750000000044 25.73989230448949, 14.921875000000004 25.73989230448949, " +
                    "14.921875000000004 18.916011030403887, 3.6718750000000044 18.916011030403887, 3.6718750000000044 25.73989230448949))"
                },
                new DemoSnapshotModel()
                {
                    Id = 2,
                    Satellite = "BS",
                    ShootingDate = "11.02.2021 00:00:00",
                    Cloudiness = 10,
                    Turn = 5,
                    Coordinates = "POLYGON ((35.23238462584629 57.60906073517954, 35.54000181334629 56.16893557803185, " +
                    "37.03414243834629 55.972705249882125, 37.95699400084629 57.30173453950533, 35.23238462584629 57.60906073517954))"
                }
            };
        }

        private void SetUpDemoSnapshotRepository()
        {
            //_demoSnapshotsList.Where<DemoSnapshot>(item => item.Coordinates.STContains(SqlGeography.Parse(new SqlString(query))))
            _mockRepository.Setup(x => x.GetAllAsync(It.IsAny<string>()))
                .ReturnsAsync(new Func<string, List<DemoSnapshot>>(query =>
                {
                    if (String.IsNullOrEmpty(query))
                    {
                        return _demoSnapshotsList;
                    }
                    else
                    {
                        SqlGeography sqlGeography = SqlGeography.Parse(new SqlString(query));

                        return _demoSnapshotsList.Where<DemoSnapshot>(item => (bool)item.Coordinates.STContains(sqlGeography)).ToList();
                    }
                }));

            _mockRepository.Setup(x => x.ReadAsync(It.IsAny<int>()))
                .ReturnsAsync(new Func<int, DemoSnapshot>(id => _demoSnapshotsList.Find(x => x.Id.Equals(id))));

            _mockRepository.Setup(x => x.CreateAsync(It.IsAny<DemoSnapshot>()))
                .ReturnsAsync(new Func<DemoSnapshot, int>(item =>
                {
                    _demoSnapshotsList.Add(item);

                    return item.Id;
                }));

            _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<DemoSnapshot>()))
                .ReturnsAsync(new Func<DemoSnapshot, int>(item =>
                {
                    if (_demoSnapshotsList.Exists(x => x.Id == item.Id))
                    {
                        _demoSnapshotsList.Insert(_demoSnapshotsList.FindIndex(x => x.Id.Equals(item.Id)), item);

                        return item.Id;
                    }

                    throw new ArgumentException("Item not found", nameof(item));
                }));

            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(new Func<int, int>(id =>
                {
                    if (_demoSnapshotsList.Exists(x => x.Id == id))
                    {
                        _demoSnapshotsList.RemoveAt(_demoSnapshotsList.FindIndex(x => x.Id.Equals(id)));

                        return id;
                    }

                    throw new ArgumentException("Item not found", nameof(id));
                }));
        }
    }
}