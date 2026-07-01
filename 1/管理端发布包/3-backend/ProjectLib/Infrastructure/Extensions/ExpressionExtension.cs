using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Infrastructure.Extensions;

/// <summary>
/// 表达式扩展
/// </summary>
public static class ExpressionExtension
{
    /// <summary>
    /// 谓词表达式 OrElse 连接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        return left.Compose(right, Expression.OrElse);
    }

    /// <summary>
    /// 谓词表达式 AndAlso 连接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        return left.Compose(right, Expression.AndAlso);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="merge"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Compose<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second, Func<Expression, Expression, Expression> merge)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));

        ReplaceExpressionVisitor leftVisitor = new(first.Parameters[0], parameter);
        Expression? left = leftVisitor.Visit(first.Body);

        ReplaceExpressionVisitor rightVisitor = new(second.Parameters[0], parameter);
        Expression? right = rightVisitor.Visit(second.Body);

        return Expression.Lambda<Func<T, bool>>(merge(left, right), parameter);
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            if (node == _oldValue) { return _newValue; }
            return base.Visit(node);
        }
    }
}