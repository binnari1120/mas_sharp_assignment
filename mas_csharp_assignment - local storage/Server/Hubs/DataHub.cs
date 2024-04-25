using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Channels;

namespace Server
{
    public class DataHub : Hub
    {
        private readonly Channel<string> _channel = null;
        private readonly CancellationTokenSource _cancellation_token_source = null;

        public DataHub()
        {
            _channel = Channel.CreateUnbounded<string>();
            _cancellation_token_source = new CancellationTokenSource();
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();

            Debug.WriteLine($"{DateTime.Now.ToLocalTime()}: connected");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _cancellation_token_source.Cancel();

            await base.OnDisconnectedAsync(ex);
        }

        // requests
        public async Task RequestAllSymbolConstrains()
        {
            await Clients.Caller.SendAsync("RequestAllSymbolConstrains", JsonConvert.SerializeObject(Models.all_symbol_constraints));

            Debug.WriteLine($" - {DateTime.Now} | RequestAllSymbolConstrains");
        }

        // streaming
        public ChannelReader<string> SubscribeOrderbooks(CancellationToken cancellation_token)
        {
            var writer = _channel.Writer;

            _ = _push_orderbooks(writer, cancellation_token);

            return _channel.Reader;
        }

        private async Task _push_orderbooks(ChannelWriter<string> writer, CancellationToken cancellation_token)
        {
            try
            {
                while (true)
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(Models.get_all_symbol_orderbooks()), cancellation_token);

                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally
            {
                writer.TryComplete();
            }
        }
    }
}
