using System;
using System.Collections.Generic;
using System.Linq;

namespace UserManager.Implementation.Exception
{
    public class ArrayException : BusinessException
    {
        public override object Content => new { Code, Message, Errors = Errors.Select(_ => _.Content).ToList() };
        public IEnumerable<BusinessException> Errors { get; private set; }

        private ArrayException(int code, string message, IEnumerable<BusinessException> errors) : base(code, message)
        {
            Errors = errors;
        }

        public static void Assert<T>(int code, IEnumerable<Action> predicates) where T : BusinessException => Assert<T>(code, null, predicates);
        public static void Assert<T>(int code, string message, IEnumerable<Action> predicates) where T: BusinessException
        {
            var errors = predicates.Select(predicate => 
            {
                try
                {
                    predicate();
                } catch (T e)
                {
                    return e;
                }

                return null;
            }).Where( _ => _ != null).ToList();
            if (errors.Count() > 0) throw new ArrayException(code, message, errors);
        }
    }
}
