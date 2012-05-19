using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arbitrary.Test
{
    public static class TestHelpers
    {
        public static void AssertThrows<TException>(Action test) 
            where TException : Exception
        {
            bool didThrow = false;
            try
            {
                test();
            }
            catch (TException)
            {
                didThrow = true;
            }
            // Consume exceptions that aren't expected so that test fails correctly
            catch (Exception)
            {
            }
            finally
            {
                Assert.IsTrue(didThrow, "Didn't throw " + typeof(TException).FullName);
            }
        }
    }
}
