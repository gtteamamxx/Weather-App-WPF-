using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models.Application;

namespace WeatherAppTests
{
    [TestFixture]
    class MVVMMessagerSerivceTests
    {
        [Test]
        public void message_should_be_received_and_parameters_given()
        {
            int expectedValue1 = 5;
            double expectedValue2 = 25.5;

            MVVMMessagerService.RegisterReceiver<int, double>(typeof(MVVMMessagerService),
                (value1, value2) =>
                {
                    Assert.AreEqual(expectedValue1, value1);
                    Assert.AreEqual(expectedValue2, value2);
                });

            MVVMMessagerService.SendMessage(typeof(MVVMMessagerService), expectedValue1, expectedValue2);
            MVVMMessagerService.UnregisterReceiver(typeof(MVVMMessagerService));
        }

        [Test]
        public void receiver_should_exist()
        {
            MVVMMessagerService.RegisterReceiver(typeof(MVVMMessagerService), () => { });
            Assert.AreEqual(true, MVVMMessagerService.ReceiverExist(typeof(MVVMMessagerService)));
            MVVMMessagerService.UnregisterReceiver(typeof(MVVMMessagerService));
        }

        [Test]
        public void unregistering_unexisted_receiver_should_throw_error()
        {
            Assert.Throws<Exception>(unregister_unexist_receiver);

            void unregister_unexist_receiver()
            {
                MVVMMessagerService.UnregisterReceiver(typeof(MVVMMessagerService));
            }
        }

        [Test]
        public void registered_receivers_should_be_one() 
            // cant register more than 1 receivers for one type
        {
            int exceptedValue = 1;
            MVVMMessagerService.RegisterReceiver(typeof(MVVMMessagerService), () => { });
            MVVMMessagerService.RegisterReceiver(typeof(MVVMMessagerService), () => { });
            MVVMMessagerService.RegisterReceiver(typeof(MVVMMessagerService), () => { });

            Assert.AreEqual(exceptedValue, MVVMMessagerService.GetReceiversNum(typeof(MVVMMessagerService)));
        }

        [Test]
        public void unregistering_receivers_should_work()
        {
            int excpetedReceiversCount = 0;

            MVVMMessagerService.RegisterReceiver(typeof(MVVMMessagerService), () => { });
            MVVMMessagerService.UnregisterReceiver(typeof(MVVMMessagerService));

            Assert.AreEqual(excpetedReceiversCount, MVVMMessagerService.GetReceiversNum(typeof(MVVMMessagerService)));
        }
    }
}
