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
            var targetVariable = Expression.Variable(destinationType, "target");
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

            var body = Expression.MemberInit(newExpression, bindings);
            var assignExpr = Expression.Assign(targetVariable, body);

            if (mapping != null)
            {
                var customMappingExpr = Expression.Invoke(Expression.Constant(mapping), sourceParam, targetVariable);

                var blockExpr = Expression.Block(new[] { targetVariable }, assignExpr, customMappingExpr, targetVariable);

                return new Mapper<TSource, TDestination>(Expression.Lambda<Func<TSource, TDestination>>(blockExpr, sourceParam).Compile());
            }

            return new Mapper<TSource, TDestination>(Expression.Lambda<Func<TSource, TDestination>>(body, false, sourceParam).Compile());
        }
    }
}
