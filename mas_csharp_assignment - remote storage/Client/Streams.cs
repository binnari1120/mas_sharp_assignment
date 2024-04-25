using CommonModels;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Client
{
    internal class Streams
    {
        private Execution _execution = null;
        private HubConnection _connection = null;
        private ConcurrentDictionary<string, Orderbook> _orderbooks = null;

        public Streams(Execution execution)
        {
            _execution = execution;
            _connection = _execution.get_connection();
            _orderbooks = new ConcurrentDictionary<string, Orderbook>();
        }

        public ConcurrentDictionary<string, Orderbook> get_orderbooks()
        {
            return _orderbooks;
        }

        public async Task start()
        {
            CancellationTokenSource cancellation_token_source = new CancellationTokenSource();

            var channel = await _connection.StreamAsChannelAsync<string>("SubscribeOrderbooks", cancellation_token_source.Token);

            try
            {
                while (await channel.WaitToReadAsync())
                {
                    while (channel.TryRead(out var message))
                    {
                        var all_symbol_orderbooks = JsonConvert.DeserializeObject<JArray>(message);

                        if (all_symbol_orderbooks != null)
                        {
                            foreach (var symbol_orderbook in all_symbol_orderbooks)
                            {
                                _orderbooks[symbol_orderbook["symbol"].ToString()] = new Orderbook
                                {
                                    symbol = symbol_orderbook["symbol"].ToString(),
                                    code = symbol_orderbook["code"].ToString(),
                                    asks = JsonConvert.DeserializeObject<List<Depth>>(symbol_orderbook["asks"].ToString()),
                                    bids = JsonConvert.DeserializeObject<List<Depth>>(symbol_orderbook["bids"].ToString()),
                                    last_price = Convert.ToDecimal(symbol_orderbook["last_price"].ToString())
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
