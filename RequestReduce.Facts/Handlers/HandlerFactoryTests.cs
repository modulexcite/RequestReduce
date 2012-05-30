﻿using System;
using System.Web;
using Xunit;
using Xunit.Extensions;

namespace RequestReduce.Facts.Handlers
{
    class HandlerFactoryTests
    {
        class TestableHandlerFactory : Testable<RequestReduce.Handlers.HandlerFactory>
        {
            public TestableHandlerFactory()
            {

            }
        }

        public class ResolveHandler
        {
            class FakeHandler : IHttpHandler
            {
                public void ProcessRequest(HttpContext context)
                {
                }

                public bool IsReusable
                {
                    get { return false; }
                }
            }

            [Fact]
            public void WillResolveAddedMap()
            {
                var testable = new TestableHandlerFactory();
                testable.ClassUnderTest.AddHandlerMap(x => x.AbsolutePath.EndsWith(".fake") ? new FakeHandler() : null);

                var result = testable.ClassUnderTest.ResolveHandler(new Uri("http://host.com/page.fake"));

                Assert.IsType<FakeHandler>(result);
            }

            [Fact]
            public void WillResolveToNullIfThereAreNoMatchingMaps()
            {
                var testable = new TestableHandlerFactory();
                testable.ClassUnderTest.AddHandlerMap(x => x.AbsolutePath.EndsWith(".fake") ? new FakeHandler() : null);

                var result = testable.ClassUnderTest.ResolveHandler(new Uri("http://host.com/page.notfake"));

                Assert.Null(result);
            }

            [Fact]
            public void WillResolveToNullIfThereAreNoMapsGiven()
            {
                var testable = new TestableHandlerFactory();

                var result = testable.ClassUnderTest.ResolveHandler(new Uri("http://host.com/page.fake"));

                Assert.Null(result);
            }

            [Theory]
            public void WillResolveDefaultMaps(string url, Type expectedHandler)
            {
                var testable = new TestableHandlerFactory();

                var result = testable.ClassUnderTest.ResolveHandler(new Uri(url));

                if(expectedHandler != null)
                    Assert.IsType<Type>(result);
                else
                    Assert.Null(result);
            }
        }
    }
}
