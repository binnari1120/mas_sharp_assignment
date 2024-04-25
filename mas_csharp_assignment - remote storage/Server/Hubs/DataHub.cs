using CommonModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Channels;

namespace Server
{
    public class DataHub : Hub
    {
        private readonly object _lock = new object();

        private readonly Channel<string> _channel = null;
        private readonly CancellationTokenSource _cancellation_token_source = null;

        public DataHub()
        {
            Debug.WriteLine("===================== DataHub =====================");

            _channel = Channel.CreateUnbounded<string>();
            _cancellation_token_source = new CancellationTokenSource();
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

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

        public async Task RequestReset()
        {
            Logger.delete_file("active", "asset.txt");
            Logger.delete_file("active", "positions.txt");
            Logger.delete_file("active", "orders.txt");
            Logger.delete_file("records", "transactions.txt");
            Logger.delete_file("records", "orders.txt");

            await Clients.Caller.SendAsync("RequestReset", "Server cleared all records successfully!");
        }

        public async Task RequestAsset()
        {
            string message = Logger.read_file("active", "asset.txt");

            if (!string.IsNullOrEmpty(message)) await Clients.Caller.SendAsync("RequestAsset", message);
        }

        public async Task RequestPositions()
        {
            string message = Logger.read_file("active", "positions.txt");

            if (!string.IsNullOrEmpty(message)) await Clients.Caller.SendAsync("RequestPositions", message);
        }

        public async Task RequestOrders()
        {
            string message = Logger.read_file("active", "orders.txt");

            if (!string.IsNullOrEmpty(message)) await Clients.Caller.SendAsync("RequestOrders", message);
        }

        public async Task RequestTransactionRecords()
        {
            string message = Logger.read_file("records", "transactions.txt");

            if (!string.IsNullOrEmpty(message)) await Clients.Caller.SendAsync("RequestTransactionRecords", message);
            else await Clients.Caller.SendAsync("RequestTransactionRecords", string.Empty);
        }

        public async Task RequestOrderRecords()
        {
            string message = Logger.read_file("records", "orders.txt");

            if (!string.IsNullOrEmpty(message)) await Clients.Caller.SendAsync("RequestOrderRecords", message);
            else await Clients.Caller.SendAsync("RequestOrderRecords", string.Empty);
        }

        // updates
        public async Task UpdateAsset(string message)
        {
            Debug.WriteLine($" - {DateTime.Now} | UpdateAsset : {message}");
            lock (_lock) Logger.log_string("active", "asset.txt", message);

            await Clients.Caller.SendAsync("UpdateAsset", "Server updated asset successfully!");
        }

        public async Task UpdatePositions(string message)
        {
            Debug.WriteLine($" - {DateTime.Now} | UpdatePositions : {message}");
            lock (_lock) Logger.log_string("active", "positions.txt", message);

            await Clients.Caller.SendAsync("UpdatePositions", "Server updated positions successfully!");
        }

        public async Task UpdateOrders(string message)
        {
            Debug.WriteLine($" - {DateTime.Now} | UpdateOrders : {message}");
            lock (_lock) Logger.log_string("active", "orders.txt", message);

            await Clients.Caller.SendAsync("UpdateOrders", "Server updated orders successfully!");
        }

        public async Task UpdateTransactionRecord(string message)
        {
            Debug.WriteLine($" - {DateTime.Now} | UpdateTransactionRecord : {message}");
            Logger.log_string("records", "transactions.txt", message);

            await Clients.Caller.SendAsync("UpdateTransactionRecord", "Server updated transaction recrod successfully!");
        }

        public async Task UpdateOrderRecord(string message)
        {
            Debug.WriteLine($" - {DateTime.Now} | UpdateOrderRecord : {message}");
            Logger.log_string("records", "orders.txt", message);

            await Clients.Caller.SendAsync("UpdateOrderRecord", "Server updated order recrod successfully!");
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
