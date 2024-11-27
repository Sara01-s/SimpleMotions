using System;
using System.Linq.Expressions;

namespace SimpleMotions {

	public static class ExpressionUtils {

		public static string GetProperty<T>(Expression<Func<T, object>> selector) {
			if (selector.Body is MemberExpression member) {
				return member.Member.Name;
			}

			// Implicit cast (e.g. value -> object)
        	if (selector.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand) {
            	return memberOperand.Member.Name;
        	}

        	throw new ArgumentException("Expression is not a valid property.");
    	}

	}
}