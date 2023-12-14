using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void MappingGenerator_MapsSourceToDestinationWithoutCustomMapping()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var foo = new Foo() { Id = 123, FirstName = "Berat", LastName = "Topuz", IsValid = true, numbers = new List<int> { 1, 2, 3 } };

            var bar = mapper.Map(foo);

            Assert.IsNull(bar.Id);
            Assert.IsNull(bar.FullName);
            Assert.AreEqual(foo.IsValid, bar.IsValid);
            CollectionAssert.AreEquivalent(foo.numbers, bar.numbers);
        }

        [TestMethod]
        public void MappingGenerator_MapsSourceToDestinationWithCustomMapping()
        {
            var mapGenerator = new MappingGenerator();
            Action<Foo, Bar> customMapping = (source, destination) =>
            {
                destination.FullName = source.FirstName + " " + source.LastName;
                destination.IsValid = !source.IsValid;
                destination.Id = source.Id.ToString();
                destination.numbers = source.numbers.Select(x => x*2).ToList();

            };
            var mapper = mapGenerator.Generate<Foo, Bar>(customMapping);
            var foo = new Foo() { Id = 123, FirstName = "Berat", LastName = "Topuz", IsValid = true, numbers = new List<int> { 1, 2, 3 } };
            var bar = mapper.Map(foo);

            Assert.AreEqual(foo.Id.ToString(), bar.Id);
            Assert.AreEqual(bar.FullName, foo.FirstName + " " + foo.LastName);
            Assert.AreEqual(foo.IsValid, !bar.IsValid);
            CollectionAssert.AreEquivalent(foo.numbers.Select(x => x*2).ToList(), bar.numbers);
        }
    }
}
