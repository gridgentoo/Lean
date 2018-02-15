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
        private readonly CancellationTokenSource _ctsRestartGateway = new CancellationTokenSource();
        private readonly AutoResetEvent _resetEventRestartGateway = new AutoResetEvent(false);
        private readonly AutoResetEvent _resetEventRestartGatewayComplete = new AutoResetEvent(false);
        private Exception _lastRestartError;

        /// <summary>
        /// Returns true if the restart was completed with no errors, false otherwise
        /// </summary>
        public bool WasLastRestartSuccessful => _lastRestartError == null;

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
                            _lastRestartError = null;

                            // start the restart sequence
                            _brokerage.ResetGatewayConnection();
                        }
                        catch (Exception exception)
                        {
                            Log.Error($"InteractiveBrokersRestartManager.RestartHandler(): Error in ResetGatewayConnection: {exception}");

                            _lastRestartError = exception;
                        }

                        Log.Trace("InteractiveBrokersRestartManager.RestartHandler(): Restart sequence end.");

                        // send restart complete signal to caller, allowing caller to proceed
                        _resetEventRestartGatewayComplete.Set();
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
            Log.Trace("InteractiveBrokersRestartManager.Restart(): Signaling restart.");

            // send restart signal to restart handler thread
            _resetEventRestartGateway.Set();

            // wait until restart complete
            _resetEventRestartGatewayComplete.WaitOne();

            Log.Trace($"InteractiveBrokersRestartManager.Restart(): Restart complete, success: {WasLastRestartSuccessful}");
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
