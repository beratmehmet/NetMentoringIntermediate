using System;
using System.Linq;
using System.Linq.Expressions;
using Expressions.Task3.E3SQueryProvider.Models.Entities;
using Xunit;

namespace Expressions.Task3.E3SQueryProvider.Test
{
    public class FtsRequestTranslatorTests
    {
        #region SubTask 1 : operands order
        private readonly ExpressionToFtsRequestTranslator _translator;
        public FtsRequestTranslatorTests()
        {
            _translator = new ExpressionToFtsRequestTranslator();
        }

        [Fact]
        public void TestBinaryBackOrder()
        {
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => "EPRUIZHW006" == employee.Workstation;

            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated);
        }

        #endregion

        #region SubTask 2: inclusion operations

        [Fact]
        public void TestBinaryEqualsQueryable()
        {
            Expression<Func<IQueryable<EmployeeEntity>, IQueryable<EmployeeEntity>>> expression
                = query => query.Where(e => e.Workstation == "EPRUIZHW006");

            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated);
        }

        [Fact]
        public void TestBinaryEquals()
        {
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation == "EPRUIZHW006";

            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated);
        }

        [Fact]
        public void TestMethodEquals()
        {
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.Equals("EPRUIZHW006");

            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(EPRUIZHW006)", translated);
        }

        [Fact]
        public void TestStartsWith()
        {
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.StartsWith("EPRUIZHW006");
            
            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(EPRUIZHW006*)", translated);
        }

        [Fact]
        public void TestEndsWith()
        {
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.EndsWith("IZHW0060");

            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(*IZHW0060)", translated);
        }

        [Fact]
        public void TestContains()
        {
            Expression<Func<EmployeeEntity, bool>> expression
                = employee => employee.Workstation.Contains("IZHW006");

            string translated = _translator.Translate(expression);
            Assert.Equal("Workstation:(*IZHW006*)", translated);
        }

        #endregion
    }
}
