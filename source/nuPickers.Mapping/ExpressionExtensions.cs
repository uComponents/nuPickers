using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace nuPickers.Mapping
{
    internal static class ExpressionExtensions
    {

        /// <summary>
        /// Taken from <c>System.Data.Entity.Internal.DbHelpers</c>:
        /// 
        /// Called recursively to parse an expression tree representing a property path..
        /// This involves parsing simple property accesses like o =&gt; o.Products as well as calls to Select like
        /// o =&gt; o.Products.Select(p =&gt; p.OrderLines).
        /// </summary>
        /// <param name="expression"> The expression to parse. </param>
        /// <param name="path"> The expression parsed into an include path, or null if the expression did not match. </param>
        /// <returns> True if matching succeeded; false if the expression could not be parsed. </returns>
        public static bool TryParsePath(this Expression expression, out string path)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            path = null;
            var withoutConvert = expression.RemoveConvert(); // Removes boxing
            var memberExpression = withoutConvert as MemberExpression;
            var callExpression = withoutConvert as MethodCallExpression;

            if (memberExpression != null)
            {
                var thisPart = memberExpression.Member.Name;
                string parentPart;
                if (!TryParsePath(memberExpression.Expression, out parentPart))
                {
                    return false;
                }
                path = parentPart == null ? thisPart : (parentPart + "." + thisPart);
            }
            else if (callExpression != null)
            {
                if (callExpression.Method.Name == "Select"
                    && callExpression.Arguments.Count == 2)
                {
                    string parentPart;
                    if (!TryParsePath(callExpression.Arguments[0], out parentPart))
                    {
                        return false;
                    }
                    if (parentPart != null)
                    {
                        var subExpression = callExpression.Arguments[1] as LambdaExpression;
                        if (subExpression != null)
                        {
                            string thisPart;
                            if (!TryParsePath(subExpression.Body, out thisPart))
                            {
                                return false;
                            }
                            if (thisPart != null)
                            {
                                path = parentPart + "." + thisPart;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes boxing on the expression.
        /// 
        /// Taken from <c>System.Data.Entity.Utilities.ExpressionExtensions</c>.
        /// </summary>
        public static Expression RemoveConvert(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            while ((expression != null)
                   && (expression.NodeType == ExpressionType.Convert
                       || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }

        /// <summary>
        /// Gets property info from a lambda expression.
        /// </summary>
        /// <param name="expression">The expression indicating the property.</param>
        /// <returns>The property info.</returns>
        public static PropertyInfo GetPropertyInfo(this LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var withoutConvert = expression.Body.RemoveConvert();

            var memberExpression = withoutConvert as MemberExpression;

            if (memberExpression == null || memberExpression.Member as PropertyInfo == null)
            {
                throw new IndexOutOfRangeException("Property expression could not be parsed");
            }

            return memberExpression.Member as PropertyInfo;
        }
    }
}
