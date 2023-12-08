using System.Linq.Expressions;
using System.Collections.Generic;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        public static Expression<TDelegate> TransformLambdaExpression<TDelegate>(
        Expression<TDelegate> sourceExpression,
        Dictionary<string, object> parameterReplacements)
        {
            var translator = new IncDecExpressionVisitor(parameterReplacements);
            Expression translatedBody = translator.Visit(sourceExpression.Body);

            return Expression.Lambda<TDelegate>(translatedBody, sourceExpression.Parameters); ;
        }

        private readonly IReadOnlyDictionary<string, object> _parameterReplacements;

        private IncDecExpressionVisitor(Dictionary<string, object> parameterReplacements)
        {
            _parameterReplacements = parameterReplacements;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterReplacements.TryGetValue(node.Name, out var replacementValue))
            {
                return Expression.Convert(Expression.Constant(replacementValue, node.Type), node.Type);
            }

            return base.VisitParameter(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Right is ConstantExpression constantExpression && (int)constantExpression.Value == 1)
            {
                if (node.NodeType == ExpressionType.Add || node.NodeType == ExpressionType.Subtract)
                {
                    if (node.Left is ParameterExpression)
                    {
                        var unaryNodeType = node.NodeType == ExpressionType.Add
                        ? ExpressionType.Increment
                        : ExpressionType.Decrement;

                        return Expression.MakeUnary(unaryNodeType, Visit(node.Left), node.Type);
                    }
                    else if (node.Left is BinaryExpression leftNode && (leftNode.NodeType == ExpressionType.Add || leftNode.NodeType == ExpressionType.Subtract))
                    {
                        Expression right;

                        right = Visit(leftNode.Right);

                        var unaryNodeType = node.NodeType == ExpressionType.Add
                        ? ExpressionType.Increment
                        : ExpressionType.Decrement;
                        right = Expression.MakeUnary(unaryNodeType, right, node.Type);

                        if (leftNode.NodeType == ExpressionType.Add)
                        {
                            return Expression.Add(Visit(leftNode.Left), right);
                        }
                        else
                        {
                            return Expression.Subtract(Visit(leftNode.Left), right);
                        }
                    }
                }
            }

            return base.VisitBinary(node);
        }
    }
}
