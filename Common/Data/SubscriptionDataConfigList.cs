/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace QuantConnect.Data
{
    /// <summary>
    /// Provides convenient methods for holding a unique set of <see cref="SubscriptionDataConfig"/>
    /// </summary>
    public class SubscriptionDataConfigList : IEnumerable<SubscriptionDataConfig>
    {
        private readonly ConcurrentDictionary<SubscriptionDataConfig, SubscriptionDataConfig> _configs
            = new ConcurrentDictionary<SubscriptionDataConfig, SubscriptionDataConfig>();

        /// <summary>
        /// <see cref="Symbol"/> for which this class holds <see cref="SubscriptionDataConfig"/>
        /// </summary>
        public Symbol Symbol { get; private set; }

        /// <summary>
        /// Assume that the InternalDataFeed is the same for both <see cref="SubscriptionDataConfig"/>
        /// </summary>
        public bool IsInternalFeed
        {
            get { return !_configs.IsEmpty && this.All(sdc => sdc.IsInternalFeed); }
        }

        /// <summary>
        /// Default constructor that specifies the <see cref="Symbol"/> that the <see cref="SubscriptionDataConfig"/> represent
        /// </summary>
        /// <param name="symbol"></param>
        public SubscriptionDataConfigList(Symbol symbol)
        {
            Symbol = symbol;
        }

        /// <summary>
        /// Sets the <see cref="DataNormalizationMode"/> for all <see cref="SubscriptionDataConfig"/> contained in the list
        /// </summary>
        /// <param name="normalizationMode"></param>
        public void SetDataNormalizationMode(DataNormalizationMode normalizationMode)
        {
            if (Symbol.SecurityType == SecurityType.Option && normalizationMode != DataNormalizationMode.Raw)
            {
                throw new ArgumentException("DataNormalizationMode.Raw must be used with options");
            }

            foreach (var config in this)
            {
                config.DataNormalizationMode = normalizationMode;
            }
        }

        /// <summary>
        /// Adds the configuration to this collection
        /// </summary>
        /// <param name="config">The subscription data config to add</param>
        /// <returns>True if the configuration was added, false if it already existed</returns>
        public bool Add(SubscriptionDataConfig config)
        {
            return _configs.TryAdd(config, config);
        }

        /// <summary>
        /// Adds the configuration object to this collection
        /// </summary>
        /// <param name="configs">The subscription data configs to add</param>
        public void AddRange(IEnumerable<SubscriptionDataConfig> configs)
        {
            foreach (var config in configs)
            {
                Add(config);
            }
        }

        /// <summary>
        /// Removes the configuration to this collection
        /// </summary>
        /// <param name="config">The subscription data config to remove</param>
        /// <returns>True if the configuration was removed, false if it didn't exist</returns>
        public bool Remove(SubscriptionDataConfig config)
        {
            return _configs.TryRemove(config, out config);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<SubscriptionDataConfig> GetEnumerator()
        {
            return _configs.Select(kvp => kvp.Value).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
