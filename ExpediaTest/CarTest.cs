using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedia;
using Rhino.Mocks;
using System.Collections.Generic;

namespace ExpediaTest
{
	[TestClass]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[TestInitialize]
		public void TestInitialize()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[TestMethod]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}


        [TestMethod]
        public void TestgetCarLocation()
        {
            IDatabase mockDB = mocks.DynamicMock<IDatabase>();

            String carLocal1 = "The Spot";
            String carLocal2 = "The Other Spot";
            using (mocks.Ordered())
            {
                Expect.Call(mockDB.getCarLocation(24)).Return(carLocal1);
                Expect.Call(mockDB.getCarLocation(1025)).Return(carLocal2);
                mockDB.Stub(x => x.getCarLocation(Arg<int>.Is.Anything)).Return("Uknown");
            }
            mocks.ReplayAll();

            Car target = new Car(10);
            target.Database = mockDB;

            String result;

            result = target.getCarLocation(24);
            Assert.AreEqual(carLocal1, result);
            result = target.getCarLocation(1025);
            Assert.AreEqual(carLocal2, result);
            result = target.getCarLocation(25);
            Assert.AreEqual("Uknown", result);

            mocks.VerifyAll();
        }

        [TestMethod]
        public void TestCarMileage()
        {
            IDatabase mockDB = mocks.StrictMock<IDatabase>();
            Int32 miles = 225;

            Expect.Call(mockDB.Miles).PropertyBehavior();
            mocks.ReplayAll();
            mockDB.Miles = miles;

            var target = new Car(12);
            target.Database = mockDB;

            Assert.AreEqual(target.Mileage, miles);
            mocks.VerifyAll();
        }
	}
}
