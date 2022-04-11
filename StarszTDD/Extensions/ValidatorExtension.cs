using System;
using System.Collections.Generic;

namespace StarszTDD
{
    public static class ValidatorExtension
    {
        public static IEnumerable<T> Validate<T>(this IEnumerable<T> list, Func<IEnumerable<T>, bool> validationRule)
            => validationRule(list) ? list : throw new TestletDataException();

    }

    public class TestletDataException : Exception
    {

    }

}
