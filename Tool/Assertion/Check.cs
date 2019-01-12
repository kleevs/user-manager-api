using System;

namespace Tool.Assertion
{
    public class Check
    {
        public Check Assert(Func<bool> predicat)
        {
            return this;
        }

        public void IsValid()
        {
            try
            {
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
