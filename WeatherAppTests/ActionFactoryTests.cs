using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models.Factory;

namespace WeatherAppTests
{
    [TestFixture]
    class ActionFactoryTests
    {
        [Test]
        public async Task progress_changed_should_be_called_five_times()
        {
            int callNum = 0;
            int expectedCalls = 5;

            await new ActionFactory()
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ProgressChanged(
                progress =>
                {
                    callNum++;
                }).StartAsync();
            Assert.AreEqual(expectedCalls, callNum);

            async Task<bool> testMethod()
            {
                return false;
            }
        }

        [Test]
        public async Task progress_change_value_can_be_dividing_by_20()
        {
            int expectedValue = 20;

            await new ActionFactory()
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ProgressChanged(
                progress =>
                {
                    if(progress % expectedValue != 0)
                    {
                        Assert.Fail("should be divided by" + expectedValue);
                    }
                }).StartAsync();

            async Task<bool> testMethod()
            {
                return false;
            }
        }

        [Test]
        public async Task result_set_should_return_true_and_three()
        {
            int callsNum = 0;
            (bool, int) exceptedResultSet = (true, 3);

            (bool, int) resultSet = await new ActionFactory()
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .StartAsync();

            Assert.AreEqual(exceptedResultSet.Item1, resultSet.Item1);
            Assert.AreEqual(exceptedResultSet.Item2, resultSet.Item2);

            async Task<bool> testMethod()
            {
                if (++callsNum == 3)
                    return true;
                return false;
            }
        }

        [Test]
        public async Task result_set_should_return_false_and_3()
        {
            (bool, int) exceptedResultSet = (false, 3);

            (bool, int) resultSet = await new ActionFactory()
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .ContinueWith(testMethod)
                .StartAsync();

            Assert.AreEqual(exceptedResultSet.Item1, resultSet.Item1);
            Assert.AreEqual(exceptedResultSet.Item2, resultSet.Item2);

            async Task<bool> testMethod()
            {
                return false;
            }
        }
    }
}
