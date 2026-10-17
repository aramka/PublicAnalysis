using AwesomeAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.Cache
{
    record TestCacheItem(string Id, long TimeStamp);

    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public async Task CacheMiss()
        {
            /*
             * Lock on the key
             * Get the item
             * update cache
             * update fifo
             */
            CacheOptions cacheOptions = new CacheOptions { ItemCount = 1 };
            Mock<IOptions<CacheOptions>> optionsMoq = new Mock<IOptions<CacheOptions>>();
            optionsMoq.Setup(a => a.Value).Returns(cacheOptions);
            Cache<string,TestCacheItem, CacheOptions> underTest = new Cache<string,TestCacheItem, CacheOptions>(optionsMoq.Object);
            int id = 0;
            var factory = async (string key) =>
            {
                Interlocked.Increment(ref id);
                return await Task.FromResult(new TestCacheItem(id.ToString(), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
            };
            string key = "theKey";

            var tasks = Enumerable.Range(1, 5)
                .Select( async (i) => {

                    var item = await underTest.GetOrAdd(key, factory, 1000);
                    return item;
                
                }).AsParallel();
            var groups = (await Task.WhenAll(tasks))
                .GroupBy(item => item);

            groups.Count().Should().Be(1);

            var group = groups.Single();

            group.Count().Should().Be(5);

            group.All(item => item.Id == 1.ToString());

        }
        [TestMethod]
        public async Task CacheMissAndFull()
        {
            /*
             * lock on the key
             * get the item
             * update cache
             * remove first from fifo
             * 
             * Fill the cache to the limit + 1 the first should be removed so tryget should fail for that key and next GetOrAdd should call factory
             */

            CacheOptions cacheOptions = new CacheOptions { ItemCount = 1 };
            Mock<IOptions<CacheOptions>> optionsMoq = new Mock<IOptions<CacheOptions>>();
            optionsMoq.Setup(a => a.Value).Returns(cacheOptions);
            Cache<string, TestCacheItem, CacheOptions> underTest = new Cache<string, TestCacheItem, CacheOptions>(optionsMoq.Object);
            int count = 0;
            var factory = async (string key) =>
            {
                Interlocked.Increment(ref count);
                return await Task.FromResult(new TestCacheItem(key, DateTimeOffset.UnixEpoch.ToUnixTimeMilliseconds()));
            };

            var tasks = Enumerable.Range(1, 5)
                .Select((i) => {
                    return Task.Run(async () =>
                    {
                        var item = await underTest.GetOrAdd($"Key_{i}", factory, 1000);
                        return item;
                    }
                    );
                });

            var items = await Task.WhenAll(tasks);
            count.Should().Be(5);
            underTest.Count.Should().Be(1);
        }
    }
}
