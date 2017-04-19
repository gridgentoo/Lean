using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QuantConnect.Brokerages;
using QuantConnect.Data;
using QuantConnect.Interfaces;
using QuantConnect.Orders;
using QuantConnect.Securities;

namespace QuantConnect.Tests.Brokerages
{
    [TestFixture]
    public class CompositeBrokerageTests
    {
        [Test]
        public void ConnectsToEachBrokerage()
        {
            var b1 = CreateBrokerage1();
            b1.Setup(b => b.Connect()).Callback(async () => await Task.Delay(Time.OneSecond));

            var b2 = CreateBrokerage2();
            b2.Setup(b => b.Connect()).Callback(async () => await Task.Delay(Time.OneSecond));

            var router = new Mock<IBrokerageRouter>();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            compositeBrokerage.Connect();

            Mock.VerifyAll(router, b1, b2);
        }

        [Test]
        public void DisconnectsFromEachBrokerage()
        {
            var b1 = CreateBrokerage1();
            b1.Setup(b => b.Disconnect()).Callback(async () => await Task.Delay(Time.OneSecond));

            var b2 = CreateBrokerage2();
            b2.Setup(b => b.Disconnect()).Callback(async () => await Task.Delay(Time.OneSecond));

            var router = new Mock<IBrokerageRouter>();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            compositeBrokerage.Disconnect();

            Mock.VerifyAll(router, b1, b2);
        }

        [Test]
        public void GetOpenOrdersReturnsGetOpenOrdersFromChildBrokeragesInOrder()
        {
            var b1 = CreateBrokerage1();
            var b1Orders = new List<Order>
            {
                new MarketOrder {Symbol = Symbols.SPY},
                new StopMarketOrder {Symbol = Symbols.EURGBP}
            };
            b1.Setup(b => b.GetOpenOrders()).Returns(b1Orders);

            var b2 = CreateBrokerage2();
            var b2Orders = new List<Order>
            {
                new MarketOrder {Symbol = Symbols.SPY},
                new StopMarketOrder {Symbol = Symbols.EURUSD}
            };
            b2.Setup(b => b.GetOpenOrders()).Returns(b2Orders);

            var router = new Mock<IBrokerageRouter>();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var actualOrders = compositeBrokerage.GetOpenOrders();
            var expectedOrders = b1Orders.Concat(b2Orders);

            Assert.That(actualOrders, Is.EqualTo(expectedOrders));
        }

        [Test]
        public void GetAccountHoldingsReturnsAveragedPrices()
        {
            var b1 = CreateBrokerage1();
            var b1Holdings = new List<Holding>
            {
                new Holding{Symbol = Symbols.EURUSD, Quantity = 1, AveragePrice = 2, ConversionRate = 3, MarketPrice = 4, MarketValue = 5, Type = SecurityType.Forex },
                new Holding{Symbol = Symbols.SPY,    Quantity = 2, AveragePrice = 3, ConversionRate = 4, MarketPrice = 5, MarketValue = 6, Type = SecurityType.Equity}
            };
            b1.Setup(b => b.GetAccountHoldings()).Returns(b1Holdings);

            var b2 = CreateBrokerage2();
            var b2Holdings = new List<Holding>
            {
                new Holding{Symbol = Symbols.EURUSD, Quantity = 3, AveragePrice = 4, ConversionRate = 5, MarketPrice = 6, MarketValue = 7, Type = SecurityType.Forex },
                new Holding{Symbol = Symbols.FXE,    Quantity = 4, AveragePrice = 5, ConversionRate = 6, MarketPrice = 7, MarketValue = 8, Type = SecurityType.Equity}
            };
            b2.Setup(b => b.GetAccountHoldings()).Returns(b2Holdings);

            var router = new Mock<IBrokerageRouter>();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var holdings = compositeBrokerage.GetAccountHoldings();

            Assert.AreEqual(3, holdings.Count);

            var eurusd = holdings.Single(h => h.Symbol == Symbols.EURUSD);
            var b1EurUsd = b1Holdings[0];
            var b2EurUsd = b2Holdings[0];
            var quantity = b1EurUsd.Quantity + b2EurUsd.Quantity;
            var marketValue = b1EurUsd.MarketValue + b2EurUsd.MarketValue;
            var avgP = WeightedAverage(new[] {1m, b1EurUsd.AveragePrice   }, new[] {3m, b2EurUsd.AveragePrice   });
            var conv = WeightedAverage(new[] {1m, b1EurUsd.ConversionRate }, new[] {3m, b2EurUsd.ConversionRate });
            var marP = WeightedAverage(new[] {1m, b1EurUsd.MarketPrice    }, new[] {3m, b2EurUsd.MarketPrice    });
            AssertHolding(eurusd, quantity, avgP, conv, marP, marketValue);

            var spy = holdings.Single(h => h.Symbol == Symbols.SPY);
            var b1Spy = b1Holdings[1];
            AssertHolding(spy, b1Spy.Quantity, b1Spy.AveragePrice, b1Spy.ConversionRate, b1Spy.MarketPrice, b1Spy.MarketValue);

            var fxe = holdings.Single(h => h.Symbol == Symbols.FXE);
            var b2Fxe = b2Holdings[1];
            AssertHolding(fxe, b2Fxe.Quantity, b2Fxe.AveragePrice, b2Fxe.ConversionRate, b2Fxe.MarketPrice, b2Fxe.MarketValue);
        }

        [Test]
        public void GetCashBalanceReturnsCombinedBalances()
        {
            var b1 = CreateBrokerage1();
            var b1Balance = new List<Cash>
            {
                new Cash("USD", 1, 2),
                new Cash("EUR", 2, 3)
            };
            b1.Setup(b => b.GetCashBalance()).Returns(b1Balance);

            var b2 = CreateBrokerage2();
            var b2Balance = new List<Cash>
            {
                new Cash("USD", 3, 4),
                new Cash("JPY", 4, 5)
            };
            b2.Setup(b => b.GetCashBalance()).Returns(b2Balance);

            var router = new Mock<IBrokerageRouter>();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var balance = compositeBrokerage.GetCashBalance();

            Assert.AreEqual(3, balance.Count);

            var usd = balance.Single(c => c.Symbol == "USD");
            var b1Usd = b1Balance[0];
            var b2Usd = b2Balance[0];
            var amount = b1Usd.Amount + b2Usd.Amount;
            var conversionRate = WeightedAverage(new[] {b1Usd.Amount, b1Usd.ConversionRate}, new[] {b2Usd.Amount, b2Usd.ConversionRate});
            Assert.AreEqual(amount, usd.Amount);
            Assert.AreEqual(conversionRate, usd.ConversionRate);

            var eur = balance.Single(c => c.Symbol == "EUR");
            var b1Eur = b1Balance[1];
            Assert.AreEqual(b1Eur.Amount, eur.Amount);
            Assert.AreEqual(b1Eur.ConversionRate, eur.ConversionRate);

            var jpy = balance.Single(c => c.Symbol == "JPY");
            var b2Jpy = b2Balance[1];
            Assert.AreEqual(b2Jpy.Amount, jpy.Amount);
            Assert.AreEqual(b2Jpy.ConversionRate, jpy.ConversionRate);
        }

        [Test]
        public void PlaceOrderDelegatesToRoutedBrokerage()
        {
            var order = new MarketOrder();
            var b1 = CreateBrokerage1();
            b1.Setup(b => b.PlaceOrder(order)).Returns(true).Verifiable();

            var b2 = CreateBrokerage2();

            var router = new Mock<IBrokerageRouter>();
            var specifiers = new[] { b1.Object.BrokerageSpecifier, b2.Object.BrokerageSpecifier };
            router.Setup(r => r.RouteOrder(specifiers, order)).Returns(b1.Object.BrokerageSpecifier).Verifiable();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var result = compositeBrokerage.PlaceOrder(order);

            Mock.Verify(router, b1, b2);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void UpdateOrderDelegatesToRoutedBrokerage()
        {
            var order = new MarketOrder();
            var b1 = CreateBrokerage1();

            var b2 = CreateBrokerage2();
            b2.Setup(b => b.UpdateOrder(order)).Returns(true).Verifiable();

            var router = new Mock<IBrokerageRouter>();
            var specifiers = new[] { b1.Object.BrokerageSpecifier, b2.Object.BrokerageSpecifier };
            router.Setup(r => r.RouteOrder(specifiers, order)).Returns(b2.Object.BrokerageSpecifier).Verifiable();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var result = compositeBrokerage.UpdateOrder(order);

            Mock.Verify(router, b1, b2);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void CancelOrderDelegatesToRoutedBrokerage()
        {
            var order = new MarketOrder();
            var b1 = CreateBrokerage1();
            b1.Setup(b => b.CancelOrder(order)).Returns(true).Verifiable();

            var b2 = CreateBrokerage2();

            var router = new Mock<IBrokerageRouter>();
            var specifiers = new[] { b1.Object.BrokerageSpecifier, b2.Object.BrokerageSpecifier };
            router.Setup(r => r.RouteOrder(specifiers, order)).Returns(b1.Object.BrokerageSpecifier).Verifiable();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var result = compositeBrokerage.CancelOrder(order);

            Mock.Verify(router, b1, b2);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void GetHistoryDelegatesToRoutedBrokerage()
        {
            var request = new HistoryRequest();
            var history = new List<BaseData>();
            var b1 = CreateBrokerage1();
            b1.Setup(b => b.GetHistory(request)).Returns(history).Verifiable();

            var b2 = CreateBrokerage2();

            var router = new Mock<IBrokerageRouter>();
            var specifiers = new[] { b1.Object.BrokerageSpecifier, b2.Object.BrokerageSpecifier };
            router.Setup(r => r.RouteHistoryRequest(specifiers, request)).Returns(b1.Object.BrokerageSpecifier).Verifiable();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            var result = compositeBrokerage.GetHistory(request);

            Mock.Verify(router, b1, b2);
            Assert.AreEqual(history, result);
        }

        [Test]
        public void ChildBrokerageEventsAreRaised()
        {
            var b1 = CreateBrokerage1();
            var b2 = CreateBrokerage2();
            var router = new Mock<IBrokerageRouter>();
            var compositeBrokerage = new CompositeBrokerage(router.Object, b1.Object, b2.Object);

            BrokerageMessageEvent message = null;
            compositeBrokerage.Message += (sender, @event) => { message = @event; };
            b1.Raise(b => b.Message += null, null, new BrokerageMessageEvent(BrokerageSpecifier.Default, BrokerageMessageType.Error, 1, "2"));
            Assert.AreEqual("1", message.Code);
            Assert.AreEqual("2", message.Message);
            Assert.AreEqual(BrokerageMessageType.Error, message.Type);
            Assert.AreEqual(BrokerageSpecifier.Default, message.BrokerageSpecifier);

            AccountEvent accountEvent = null;
            compositeBrokerage.AccountChanged += (sender, @event) => { accountEvent = @event; };
            b2.Raise(b => b.AccountChanged += null, null, new AccountEvent("USD", 1));
            Assert.AreEqual("USD", accountEvent.CurrencySymbol);
            Assert.AreEqual(1, accountEvent.CashBalance);

            OrderEvent orderEvent = null;
            compositeBrokerage.OrderStatusChanged += (sender, @event) => { orderEvent = @event; };
            b2.Raise(b => b.OrderStatusChanged += null, null, new OrderEvent(1, Symbols.SPY, DateTime.UtcNow, OrderStatus.CancelPending, OrderDirection.Hold, 2, 3, 4));
            Assert.AreEqual(1, orderEvent.OrderId);
            Assert.AreEqual(Symbols.SPY, orderEvent.Symbol);
            Assert.AreEqual(OrderDirection.Hold, orderEvent.Direction);
            Assert.AreEqual(2, orderEvent.FillPrice);
            Assert.AreEqual(3, orderEvent.FillQuantity);
            Assert.AreEqual(4, orderEvent.OrderFee);

            OrderEvent optionPositionAssigned = null;
            compositeBrokerage.OptionPositionAssigned += (sender, @event) => { optionPositionAssigned = @event; };
            b1.Raise(b => b.OptionPositionAssigned += null, null, new OrderEvent(1, Symbols.SPY, DateTime.UtcNow, OrderStatus.CancelPending, OrderDirection.Hold, 2, 3, 4));
            Assert.AreEqual(1, optionPositionAssigned.OrderId);
            Assert.AreEqual(Symbols.SPY, optionPositionAssigned.Symbol);
            Assert.AreEqual(OrderDirection.Hold, optionPositionAssigned.Direction);
            Assert.AreEqual(2, optionPositionAssigned.FillPrice);
            Assert.AreEqual(3, optionPositionAssigned.FillQuantity);
            Assert.AreEqual(4, optionPositionAssigned.OrderFee);

        }

        private static decimal WeightedAverage(params decimal[][] args)
        {
            var sumOfWeights = args.Sum(arr => arr[0]);
            var sumOfWeightedValues = args.Sum(arr => arr[0] * arr[1]);
            return sumOfWeightedValues / sumOfWeights;
        }

        private static void AssertHolding(Holding actual, decimal quantity, decimal averagePrice, decimal conversionRate, decimal marketPrice, decimal marketValue)
        {
            Assert.AreEqual(quantity, actual.Quantity);
            Assert.AreEqual(averagePrice, actual.AveragePrice);
            Assert.AreEqual(conversionRate, actual.ConversionRate);
            Assert.AreEqual(marketPrice, actual.MarketPrice);
            Assert.AreEqual(marketValue, actual.MarketValue);
        }
        
        private Mock<IBrokerage> CreateBrokerage1()
        {
            return CreateMockBrokerage(BrokerageName.InteractiveBrokersBrokerage, "b1");
        }

        private Mock<IBrokerage> CreateBrokerage2()
        {
            return CreateMockBrokerage(BrokerageName.FxcmBrokerage, "b2");
        }

        private Mock<IBrokerage> CreateMockBrokerage(BrokerageName brokerage, string account)
        {
            var mockBrokerage = new Mock<IBrokerage>(MockBehavior.Strict);
            var specifier = new BrokerageSpecifier(brokerage, account);
            mockBrokerage.Setup(b => b.BrokerageSpecifier).Returns(specifier).Verifiable();
            return mockBrokerage;
        }
    }
}
