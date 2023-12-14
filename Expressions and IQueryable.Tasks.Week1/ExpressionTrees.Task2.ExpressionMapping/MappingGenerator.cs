using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>(Action<TSource, TDestination> mapping = null)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var sourceParam = Expression.Parameter(sourceType);
            var newExpression = Expression.New(destinationType.GetConstructor(Type.EmptyTypes));

            List<MemberBinding> bindings = new List<MemberBinding>();

            foreach (var field in sourceType.GetFields())
            {
                try
                {
                    var destinationField = destinationType.GetField(field.Name);
                    if (destinationField == null)
                    {
                        continue;
                    }

                    var pma = Expression.MakeMemberAccess(sourceParam, field);
                    var binding = Expression.Bind(destinationField, pma);

                    bindings.Add(binding);
                }
                catch (ArgumentException ex)
                {
                    continue;
                }
            }

            var body = CreateMappingExpression(sourceParam, Expression.MemberInit(newExpression, bindings), mapping);

            return new Mapper<TSource, TDestination>(Expression.Lambda<Func<TSource, TDestination>>(body, false, sourceParam).Compile());
        }

        private Expression CreateMappingExpression<TSource, TDestination>(ParameterExpression sourceParam, Expression body, Action<TSource, TDestination> mapping)
        {
            if(mapping != null)
            {
                var targetVariable = Expression.Variable(typeof(TDestination), "target");
                var assignExpr = Expression.Assign(targetVariable, body);
                var customMappingExpr = Expression.Invoke(Expression.Constant(mapping), sourceParam, targetVariable);

                return Expression.Block(new[] { targetVariable }, assignExpr, customMappingExpr, targetVariable);
            }

            return body;
        }
    }
}
