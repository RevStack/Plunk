using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Collections.Generic;
using UnitTest.Service;
using UnitTest.Models;

namespace UnitTest
{
    [TestClass]
    public class CompanyTests
    {
        private readonly ICompanyService _service;

        public CompanyTests()
        {
            var container = UnityConfig.GetConfiguredContainer();
            _service = container.Resolve<ICompanyService>();
        }

        [TestMethod]
        public void Get()
        {
            IEnumerable<Company> entities = _service.Get();
            var results = entities.ToList();
            Assert.AreNotEqual(results.Count, 0);
        }

        [TestMethod]
        public void Find()
        {
            IQueryable<Company> entities = _service.Find(c => c.Name == "Test2");
            var results = entities.ToList();
            Assert.AreNotEqual(results.Count, 0);
        }

        [TestMethod]
        public void Distinct()
        {
            List<Company> entities = _service.Get().GroupBy(c => c.Name).Select(grp => grp.First()).ToList();
            Assert.AreNotEqual(entities.Count, 0);
        }

        [TestMethod]
        public void Add()
        {
            Company entity = new Company();
            entity.Name = "Test2";
            entity.IsActive = true;
            entity = _service.Add(entity);
            Assert.AreNotEqual(entity, null);
        }

        [TestMethod]
        public void Update()
        {
            Company entity = _service.Find(c => c.Name == "Test2").SingleOrDefault();
            entity.IsActive = false;
            entity = _service.Update(entity);
            Assert.AreNotEqual(entity.IsActive, true);
        }

        [TestMethod]
        public void Delete()
        {
            Company entity = _service.Find(c => c.Name == "Test2").SingleOrDefault();
            _service.Delete(entity);
            int count = _service.Find(c => c.Id == entity.Id).ToList().Count();
            Assert.AreEqual(count, 0);
        }
    }
}
