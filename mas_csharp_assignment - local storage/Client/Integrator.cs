using CommonModels;
using System.Collections.Concurrent;

namespace Client
{
    internal class Integrator
    {
        private Exchange _exchange;

        private Execution _execution = null;
        private Streams _streams = null;

        private List<SymbolConstraint> _symbol_constraints = null;
        private ConcurrentDictionary<string, Orderbook> _orderbooks = null;

        public Integrator(Exchange exchange, int port)
        {
            _exchange = exchange;

            _execution = new Execution(exchange, port);
            _streams = new Streams(_execution);

            _symbol_constraints = _execution.get_symbol_constraints();
            _orderbooks = _streams.get_orderbooks();
        }

        public Exchange get_exchange()
        {
            return _exchange;
        }

        public List<SymbolConstraint> get_symbol_constraints()
        {
            return _symbol_constraints;
        }

        public ConcurrentDictionary<string, Orderbook> get_orderbooks()
        {
            return _orderbooks;
        }

        public Asset get_asset()
        {
            decimal pnl = 0;
            decimal total_position_size = 0;
            decimal total_order_size = 0;

            foreach (var position in _execution.get_positions())
            {
                if (_orderbooks.ContainsKey(position.Value.symbol) && _orderbooks[position.Value.symbol].bids.Count > 0) pnl += (_orderbooks[position.Value.symbol].bids.FirstOrDefault().price - position.Value.average_price) * position.Value.quantity;

                total_position_size += position.Value.size;
            }

            foreach (var order in _execution.get_orders())
            {
                total_order_size += order.Value.size;
            }

            Asset asset = _execution.get_asset();

            asset.total_position_size = total_position_size;
            asset.total_order_size = total_order_size;
            asset.available_balance = asset.initial_balance - asset.total_position_size - asset.total_order_size;
            asset.pnl = pnl;
            asset.equity = asset.initial_balance + asset.pnl;

            return asset;
        }

        public ConcurrentDictionary<string, Position> get_positions()
        {
            return _execution.get_positions();
        }

        public ConcurrentDictionary<string, Order> get_orders()
        {
            return _execution.get_orders();
        }

        public List<TransactionRecord> get_transaction_records()
        {
            return _execution.get_transaction_records();
        }

        public List<OrderRecord> get_order_records()
        {
            return _execution.get_order_records();
        }

        public bool on_connected()
        {
            return _execution.on_connected();
        }

        public bool on_all_symbol_constraints_loaded()
        {
            while (true)
            {
                if (_symbol_constraints.Count > 0) break;
            }

            return true;
        }

        public void start_streams()
        {
            Task.Run(async () =>
            {
                await _streams.start();
            });
        }

        public OrderApplication send_order(OrderApplication order_application)
        {
            return _execution.send_order(order_application, _orderbooks);
        }

        public void reset(bool clear_all_records = false)
        {
            _execution.reset(clear_all_records);
        }
    }
}
