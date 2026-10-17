using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Public.Frameworks.Cache
{

    public class Cache<T,U,O> : ICache<T,U, O>
        where T:notnull
        where U:notnull
        where O:CacheOptions
    {
        public O Options { get; }
        private readonly ConcurrentDictionary<T, SemaphoreSlim> semaphores = new ();
        private readonly ConcurrentDictionary<T, U> items = new ();
        private readonly ConcurrentQueue<T> fifoKeys = new();


        public Cache(IOptions<O> options)
        {
            this.Options = options.Value;
            this.semaphores = new();
            this.items = new();
        }

        public int Count => this.items.Count;

        public bool Contains(T key) => this.items.ContainsKey(key);

        public async Task<U> GetOrAdd(T key, Func<T, Task<U>> factory, int semaphoreWaitItemOutMilli)
        {
            if (this.items.TryGetValue(key, out U? value))
            {
                return value;
            }
            var thisThreadSemaphore = new SemaphoreSlim(1);
            var actualSemaphore = this.semaphores.GetOrAdd(key, thisThreadSemaphore);
            try
            {
                if (thisThreadSemaphore != actualSemaphore)
                {
                    thisThreadSemaphore.Dispose();
                }

                var locked = await actualSemaphore.WaitAsync(semaphoreWaitItemOutMilli);

                if (!locked)
                {
                    throw new TimeoutException($"Timeout while waiting to lock key {key}");
                }

                if (this.items.TryGetValue(key, out value))
                {
                    return value;
                }

                value = await factory(key);

                this.items[key] = value;
                this.fifoKeys.Enqueue(key);

            }
            finally
            {
                actualSemaphore.Release();
            }
            /*
             * TODO: Constituent facts: tangible book, capex, free cashflow, ebitda, adjusted ebitda, operating margin, ebitda margin, net margin, 
             * TODO: pie of contituents foreach year, multi line timeseries of constituents
             * TODO: for total facts show time series of their constituents. pie foreach year of their constituents
             * TODO: data quality check. Check if their are duplicates data points, same fact, year, period, and ascension, for constituents. We assume ONE of each fact per report
             * TODO: data quality check. Gaps
             * TODO: data quality check. check against some other data
             * TODO: change yoy and sequential
             * TODO: check for consecutive same number. at least one cashflow fact looked like it had same number consecutively
             * TODO: Clean up the statement tree json so that all the xrbl technical things are mapped away
             * TODO: for any data point in the UI you should be able to see all the meta data, or the actual fact and/or link to that filing on edgar
             * TODO: make sure to escape anything passed into json query
             * TODO: UI ticker look ahead
             * TODO: Trim the statement tree down
             * TODO: UI default view when ticker selected
             * TODO: Select 10-K or 10-Q
             * TODO: single exe packaged with UI app
             */

            int countToRemove = fifoKeys.Count - this.Options.ItemCount;
            for (int i = 0; i < countToRemove; i++)
            {
                if (!fifoKeys.TryDequeue(out T? removeKey))
                {
                    //some other thread dequeued did maintainence on this key already
                    continue;
                }
                this.items.Remove(removeKey, out U? removed);
                this.semaphores.Remove(removeKey, out SemaphoreSlim? removedSemaphore);
                removedSemaphore!.Dispose();
            }
            return value;

        }
    }

}
