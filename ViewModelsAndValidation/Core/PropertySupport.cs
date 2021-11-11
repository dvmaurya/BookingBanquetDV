using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ViewModelsAndValidation.Core
{
    public static class PropertySupport
    {
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var lambda = (LambdaExpression)expression;

            return GetPropertyName(lambda);
        }

        public static string GetPropertyName(LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression", "Invalid lambda");
            }

            MemberExpression memberExpression;
            if (expression.Body is UnaryExpression)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Invalid expression", "expression");
            }

            if (!(memberExpression.Member is PropertyInfo propertyInfo))
            {
                throw new ArgumentException("Expression is not a property", "expression");
            }

            return propertyInfo.Name;
        }
    }
}
