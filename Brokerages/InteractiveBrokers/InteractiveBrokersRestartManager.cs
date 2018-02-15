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
 *
*/

using System;
using System.Threading;
using QuantConnect.Logging;

namespace QuantConnect.Brokerages.InteractiveBrokers
{
    /// <summary>
    /// Handles the restart process for the IB Gateway
    /// </summary>
    public class InteractiveBrokersRestartManager : IDisposable
    {
        private readonly InteractiveBrokersBrokerage _brokerage;
        private readonly AutoResetEvent _resetEventRestartGateway = new AutoResetEvent(false);
        private readonly CancellationTokenSource _ctsRestartGateway = new CancellationTokenSource();

        /// <summary>
        /// Creates a new instance of the <see cref="InteractiveBrokersRestartManager"/> class
        /// </summary>
        public InteractiveBrokersRestartManager(InteractiveBrokersBrokerage brokerage)
        {
            _brokerage = brokerage;

            new Thread(RestartHandler) { IsBackground = true }.Start();
        }

        /// <summary>
        /// Handles requests to restart the IB gateway
        /// </summary>
        private void RestartHandler()
        {
            try
            {
                Log.Trace("InteractiveBrokersRestartManager.RestartHandler(): Thread started.");

                while (!_ctsRestartGateway.IsCancellationRequested)
                {
                    if (_resetEventRestartGateway.WaitOne(1000, _ctsRestartGateway.Token))
                    {
                        Log.Trace("InteractiveBrokersRestartManager.RestartHandler(): Restart sequence start.");

                        try
                        {
                            _brokerage.ResetGatewayConnection();
                        }
                        catch (Exception exception)
                        {
                            Log.Error($"InteractiveBrokersRestartManager.RestartHandler(): Error in ResetGatewayConnection: {exception}");
                        }

                        Log.Trace("InteractiveBrokersRestartManager.RestartHandler(): Restart sequence end.");
                    }
                }

                Log.Trace("InteractiveBrokersRestartManager.RestartHandler(): Thread ended.");
            }
            catch (Exception exception)
            {
                Log.Error($"InteractiveBrokersRestartManager.RestartHandler(): Error in restart handler thread: {exception}");
            }
        }

        /// <summary>
        /// Restarts the IB Gateway
        /// </summary>
        public void Restart()
        {
            _resetEventRestartGateway.Set();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _ctsRestartGateway.Cancel(false);
        }
    }
}
