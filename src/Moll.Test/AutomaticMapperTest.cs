using NUnit.Framework;
using System.Globalization;

namespace Moll.Test
{
    [TestFixture]
    public class AutomaticMapperTest
    {
        private class TestSrcClass1
        {
            public int Prop1 { get; set; }

            public string Prop2 { get; set; }
        }

        private class TestDestClass1
        {
            public int Prop1 { get; set; }

            public string Prop2 { get; set; }
        }

        private class TestDestClass2
        {
            public int Prop1 { get; set; }
        }

        private class TestDestClass3
        {
            public int Prop1 { get; set; }

            public string Prop2 { get; set; }

            public long Prop3 { get; set; }
        }

        private class TestDestClass4
        {
            public string Prop1 { get; set; }

            public string Prop2 { get; set; }
        }

        private class CustomSrc1ToDest4Mapper : AutomaticMapper<TestSrcClass1, TestDestClass4>
        {
            public override void AdditionalCustomMapping(TestSrcClass1 src, TestDestClass4 dest)
            {
                dest.Prop1 = src.Prop1.ToString(CultureInfo.InvariantCulture);
            }
        }

        [Test]
        public void MapNullObject()
        {
            var mapper = new AutomaticMapper<TestSrcClass1, TestDestClass1>();

            var dest = mapper.Map(null);

            Assert.IsNull(dest);
        }

        [Test]
        public void MapMatchingClasses()
        {
            var mapper = new AutomaticMapper<TestSrcClass1, TestDestClass1>();

            var src = new TestSrcClass1 {Prop1 = 1, Prop2 = "Test"};

            var dest = mapper.Map(src);

            Assert.IsNotNull(dest);
            Assert.AreEqual(src.Prop1, dest.Prop1);
            Assert.AreEqual(src.Prop2, dest.Prop2);
        }

        [Test]
        public void MapToDestWithMissingProperty()
        {
            var mapper = new AutomaticMapper<TestSrcClass1, TestDestClass2>();

            var src = new TestSrcClass1 {Prop1 = 1, Prop2 = "Test"};

            var dest = mapper.Map(src);

            Assert.IsNotNull(dest);
            Assert.AreEqual(src.Prop1, dest.Prop1);
        }

        [Test]
        public void MapToDestWithExtraProperty()
        {
            var mapper = new AutomaticMapper<TestSrcClass1, TestDestClass3>();

            var src = new TestSrcClass1 {Prop1 = 1, Prop2 = "Test"};

            var dest = mapper.Map(src);

            Assert.IsNotNull(dest);
            Assert.AreEqual(src.Prop1, dest.Prop1);
            Assert.AreEqual(src.Prop2, dest.Prop2);
            Assert.AreEqual(default(long), dest.Prop3);
        }

        [Test]
        public void MapToDestWithPropertyWithDifferentType()
        {
            var mapper = new AutomaticMapper<TestSrcClass1, TestDestClass4>();

            var src = new TestSrcClass1 {Prop1 = 1, Prop2 = "Test"};

            var dest = mapper.Map(src);

            Assert.IsNotNull(dest);
            Assert.AreEqual(default(string), dest.Prop1);
            Assert.AreEqual(src.Prop2, dest.Prop2);
        }

        [Test]
        public void MapToDestWithPropertyWithDifferentTypeUsingCustomMapper()
        {
            var mapper = new CustomSrc1ToDest4Mapper();

            var src = new TestSrcClass1 {Prop1 = 1, Prop2 = "Test"};

            var dest = mapper.Map(src);

            Assert.IsNotNull(dest);
            Assert.AreEqual(src.Prop1.ToString(CultureInfo.InvariantCulture), dest.Prop1);
            Assert.AreEqual(src.Prop2, dest.Prop2);
        }
    }
}
