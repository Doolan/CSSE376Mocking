
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedia;
using System.Reflection;

namespace ExpediaTest
{
    [TestClass]
    public class ServiceLocatorTest
    {
        [TestMethod]
        public void TestThatServiceLocatorInitializes()
        {
            Assert.IsNotNull(new ServiceLocator());
        }

        [TestMethod]
        public void TestThatServiceLocatorAvailableDiscountsReturnsExpectedDiscounts()
        {
            var firstDiscount = new Discount(0.05, 1000);
            var secondDisctount = new Discount(0.10, 1750);
            var locator = new ServiceLocator();

            locator.AddDiscount(firstDiscount);
            locator.AddDiscount(secondDisctount);

            Assert.AreSame(firstDiscount, locator.AvailableDiscounts[0]);
            Assert.AreSame(secondDisctount, locator.AvailableDiscounts[1]);
            Assert.AreEqual(2, locator.AvailableDiscounts.Count);
        }

        [TestMethod]
        public void TestThatServiceLocatorAvailableFlightsReturnsExpectedFlights()
        {
            var firstFlight = new Flight(DateTime.Today, DateTime.Today.AddDays(1), 500);
            var secondFlight = new Flight(DateTime.Today, DateTime.Today.AddDays(3), 200);
            var locator = new ServiceLocator();

            locator.AddFlight(firstFlight);
            locator.AddFlight(secondFlight);

            Assert.AreSame(firstFlight, locator.AvailableFlights[0]);
            Assert.AreSame(secondFlight, locator.AvailableFlights[1]);
            Assert.AreEqual(2, locator.AvailableFlights.Count);
        }

        [TestMethod]
        public void TestThatServiceLocatorAvailableCarsReturnsExpectedCars()
        {
            var firstCar = new Car(5);
            var secondCar = new Car(4);
            var locator = new ServiceLocator();

            locator.AddCar(firstCar);
            locator.AddCar(secondCar);

            Assert.AreSame(firstCar, locator.AvailableCars[0]);
            Assert.AreSame(secondCar, locator.AvailableCars[1]);
            Assert.AreEqual(2, locator.AvailableCars.Count);
        }

        [TestMethod()]
        public void TestThatUserDoesRemoveCarFromServiceLocatorWhenBooked()
        {
            ServiceLocator serviceLocator = new ServiceLocator();
            var carToBook = new Car(5);
            var remainingCar = new Car(7);
            serviceLocator.AddCar(carToBook);

            serviceLocator.AddCar(remainingCar);

            typeof(ServiceLocator).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic)
            .SetValue(serviceLocator, serviceLocator);
            
            var target = new User("Bob");
            target.book(carToBook);
            Assert.AreEqual(1, ServiceLocator.Instance.AvailableCars.Count);
            Assert.AreSame(remainingCar, ServiceLocator.Instance.AvailableCars[0]);
        }

        [TestMethod()]
        public void TestThatUserDoesRemoveFlightFromServiceLocatorWhenBooked()
        {
            ServiceLocator serviceLocator = new ServiceLocator();
            var flightToBook = new Flight(new DateTime(2000,8,15),new DateTime(2000,9,15),5);
            var remainingflight = new Flight(new DateTime(2012, 8, 25), new DateTime(2012, 9, 1), 5);
            serviceLocator.AddFlight(flightToBook);

            serviceLocator.AddFlight(remainingflight);

            typeof(ServiceLocator).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic)
            .SetValue(serviceLocator, serviceLocator);

            var target = new User("Bob");
            target.book(flightToBook);
            Assert.AreEqual(1, ServiceLocator.Instance.AvailableFlights.Count);
            Assert.AreSame(remainingflight, ServiceLocator.Instance.AvailableFlights[0]);
        }


    }
}
