using CommonModels;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Client
{
    internal class Execution
    {
        private Exchange _exchange;
        private int _port;

        private HubConnection _connection = null;

        private List<SymbolConstraint> _symbol_constraints = null;
        private Asset _asset;
        private ConcurrentDictionary<string, Position> _positions = null;
        private ConcurrentDictionary<string, Order> _orders = null;
        private List<TransactionRecord> _transaction_records = null;
        private List<OrderRecord> _order_records = null;

        public Execution(Exchange exchange, int port)
        {
            _exchange = exchange;
            _port = port;

            _connection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:{_port}/dataHub")
                .WithAutomaticReconnect(new TimeSpan[]
                {
                    TimeSpan.Zero,
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                })
                .Build();

            _symbol_constraints = new List<SymbolConstraint>();
            _asset = new Asset
            {
                exchange = _exchange,
                initial_balance = 1000000000,
                available_balance = 1000000000
            };
            _positions = new ConcurrentDictionary<string, Position>();
            _orders = new ConcurrentDictionary<string, Order>();
            _transaction_records = new List<TransactionRecord>();
            _order_records = new List<OrderRecord>();
        }

        public HubConnection get_connection()
        {
            return _connection;
        }

        public bool on_connected()
        {
            bool result = false;

            try
            {
                Task.Run(async () =>
                {
                    await _connection.StartAsync();
                }).Wait();

                Debug.WriteLine($" - {DateTime.Now} | Connected to SignalR DataHub");

                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (_connection.State != HubConnectionState.Connected) Debug.WriteLine("Not connected");
            else
            {
                Debug.WriteLine($" - {DateTime.Now} | Connected");

                _register_events();
                _request_symbol_constraints();

                reset();

                result = true;
            }

            return result;
        }

        public List<SymbolConstraint> get_symbol_constraints()
        {
            return _symbol_constraints;
        }

        public Asset get_asset()
        {
            return _asset;
        }

        public ConcurrentDictionary<string, Position> get_positions()
        {
            return _positions;
        }

        public ConcurrentDictionary<string, Order> get_orders()
        {
            return _orders;
        }

        public List<TransactionRecord> get_transaction_records()
        {
            return _transaction_records;
        }

        public List<OrderRecord> get_order_records()
        {
            return _order_records;
        }

        private void _register_events()
        {
            // requests
            _connection.On<string>("RequestAllSymbolConstrains", (message) =>
            {
                try
                {
                    var all_symbol_constraints = JsonConvert.DeserializeObject<JArray>(message);

                    if (all_symbol_constraints != null)
                    {
                        foreach (var symbol_constraint in all_symbol_constraints)
                        {
                            _symbol_constraints.Add(new SymbolConstraint
                            {
                                code = symbol_constraint["code"].ToString(),
                                symbol = symbol_constraint["symbol"].ToString(),
                                price_step = Convert.ToDecimal(symbol_constraint["price_step"].ToString()),
                                quantity_step = Convert.ToInt32(symbol_constraint["quantity_step"].ToString()),
                            });
                        }

                        Debug.WriteLine($" - {DateTime.Now} | All symbol constraints ready");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            });

            _connection.On<string>("RequestReset", (message) =>
            {
                _asset = new Asset
                {
                    exchange = _exchange,
                    initial_balance = 1000000000,
                    available_balance = 1000000000
                };

                _positions = new ConcurrentDictionary<string, Position>();
                _orders = new ConcurrentDictionary<string, Order>();
                _transaction_records = new List<TransactionRecord>();
                _order_records = new List<OrderRecord>();
            });

            _connection.On<string>("RequestAsset", (message) =>
            {
                try
                {
                    var asset = JsonConvert.DeserializeObject<JObject>(message);

                    if (asset != null)
                    {
                        _asset.exchange = Converter.get_exchange(asset["exchange"].ToString());
                        _asset.initial_balance = Convert.ToDecimal(asset["initial_balance"].ToString());
                        _asset.total_position_size = Convert.ToDecimal(asset["total_position_size"].ToString());
                        _asset.total_order_size = Convert.ToDecimal(asset["total_order_size"].ToString());
                        _asset.available_balance = Convert.ToDecimal(asset["available_balance"].ToString());
                        _asset.pnl = Convert.ToDecimal(asset["pnl"].ToString());
                        _asset.equity = Convert.ToDecimal(asset["equity"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

            _connection.On<string>("RequestPositions", (message) =>
            {
                try
                {
                    var positions = JsonConvert.DeserializeObject<JArray>(message);

                    if (positions != null)
                    {
                        foreach (var position in positions)
                        {
                            _positions[position["Key"].ToString()] = new Position
                            {
                                exchange = Converter.get_exchange(position["Value"]["exchange"].ToString()),
                                symbol = position["Value"]["symbol"].ToString(),
                                code = position["Value"]["code"].ToString(),
                                average_price = Convert.ToDecimal(position["Value"]["average_price"].ToString()),
                                quantity = Convert.ToInt32(position["Value"]["quantity"].ToString()),
                                size = Convert.ToDecimal(position["Value"]["size"].ToString()),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

            _connection.On<string>("RequestOrders", (message) =>
            {
                try
                {
                    var orders = JsonConvert.DeserializeObject<JArray>(message);

                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            _orders[order["Key"].ToString()] = new Order
                            {
                                exchange = Converter.get_exchange(order["Value"]["exchange"].ToString()),
                                id = order["Key"].ToString(),
                                symbol = order["Value"]["symbol"].ToString(),
                                code = order["Value"]["code"].ToString(),
                                side = Converter.get_side(order["Value"]["side"].ToString()),
                                price = Convert.ToDecimal(order["Value"]["price"].ToString()),
                                quantity = Convert.ToInt32(order["Value"]["quantity"].ToString()),
                                size = Convert.ToDecimal(order["Value"]["size"].ToString())
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

            _connection.On<string>("RequestTransactionRecords", (message) =>
            {
                try
                {
                    var transaction_records = JsonConvert.DeserializeObject<JArray>(message);

                    if (transaction_records != null)
                    {
                        foreach (var transaction_record in transaction_records)
                        {
                            _transaction_records.Add(new TransactionRecord
                            {
                                exchange = Converter.get_exchange(transaction_record["exchange"].ToString()),
                                time = DateTimeOffset.Parse(transaction_record["time"].ToString()).DateTime,
                                symbol = transaction_record["symbol"].ToString(),
                                code = transaction_record["code"].ToString(),
                                executed_price = Convert.ToDecimal(transaction_record["executed_price"].ToString()),
                                executed_quantity = Convert.ToInt32(transaction_record["executed_quantity"].ToString()),
                                executed_size = Convert.ToDecimal(transaction_record["executed_size"].ToString()),
                                realized_profit = Convert.ToDecimal(transaction_record["realized_profit"].ToString()),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

            _connection.On<string>("RequestOrderRecords", (message) =>
            {
                try
                {
                    var order_records = JsonConvert.DeserializeObject<JArray>(message);

                    if (order_records != null)
                    {
                        foreach (var order_record in order_records)
                        {
                            _order_records.Add(new OrderRecord
                            {
                                exchange = Converter.get_exchange(order_record["exchange"].ToString()),
                                time = DateTimeOffset.Parse(order_record["time"].ToString()).DateTime,
                                symbol = order_record["symbol"].ToString(),
                                code = order_record["code"].ToString(),
                                side = Converter.get_side(order_record["side"].ToString()),
                                price = Convert.ToDecimal(order_record["price"].ToString()),
                                quantity = Convert.ToInt32(order_record["quantity"].ToString()),
                                size = Convert.ToDecimal(order_record["size"].ToString()),
                                result = Converter.get_order_execution_result(order_record["result"].ToString()),
                                error_message = order_record["error_message"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

            // updates
            _connection.On<string>("UpdateAsset", (message) =>
            {
                Debug.WriteLine(message);
            });

            _connection.On<string>("UpdatePositions", (message) =>
            {
                Debug.WriteLine(message);
            });

            _connection.On<string>("UpdateOrders", (message) =>
            {
                Debug.WriteLine(message);
            });

            _connection.On<string>("UpdateTransactionRecord", (message) =>
            {
                Debug.WriteLine(message);
            });

            _connection.On<string>("UpdateOrderRecord", (message) =>
            {
                Debug.WriteLine(message);
            });

            // deconstructor
            _connection.Closed += async (exception) =>
            {
                Debug.WriteLine($"Closed: {exception?.Message}");
            };
        }

        private void _request_symbol_constraints()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestAllSymbolConstrains");
            }).Wait();
        }

        public void reset(bool clear_all_records = false)
        {
            if (clear_all_records) _clear_all_records();

            _load_asset();
            _load_positions();
            _load_orders();

            _read_transaction_records();
            _read_order_records();
        }

        private void _clear_all_records()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestReset");
            }).Wait();
        }

        private void _load_asset()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestAsset");
            }).Wait();
        }

        private void _load_positions()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestPositions");
            }).Wait();
        }

        private void _load_orders()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestOrders");
            }).Wait();
        }

        private void _read_transaction_records()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestTransactionRecords");
            }).Wait();
        }

        private void _read_order_records()
        {
            Task.Run(async () =>
            {
                await _connection.InvokeAsync("RequestOrderRecords");
            }).Wait();
        }

        public void update_position(OrderApplication order_application)
        {
            if (_positions.ContainsKey(order_application.symbol))
            {
                Position older_position = _positions[order_application.symbol];

                if (order_application.side == Side.BUY)
                {
                    Position position = new Position
                    {
                        exchange = order_application.exchange,
                        symbol = order_application.symbol,
                        code = order_application.code,
                        average_price = ((order_application.price * order_application.quantity) + (older_position.average_price * older_position.quantity)) / (order_application.quantity + older_position.quantity),
                        quantity = order_application.quantity + older_position.quantity,
                        size = (((order_application.price * order_application.quantity) + (older_position.average_price * older_position.quantity)) / (order_application.quantity + older_position.quantity)) * (order_application.quantity + older_position.quantity)
                    };

                    _positions[order_application.symbol] = position;

                    try
                    {
                        Task.Run(async () =>
                        {
                            await _connection.InvokeAsync("UpdatePositions", JsonConvert.SerializeObject(_positions.ToList()));
                        }).Wait();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.StackTrace);
                    }
                }
                else
                {
                    Position position = new Position
                    {
                        exchange = order_application.exchange,
                        symbol = order_application.symbol,
                        code = order_application.code,
                        average_price = older_position.average_price,
                        quantity = (order_application.quantity * -1) + older_position.quantity,
                        size = older_position.average_price * ((order_application.quantity * -1) + older_position.quantity)
                    };

                    _positions[order_application.symbol] = position;

                    if (order_application.quantity == older_position.quantity) _positions.TryRemove(order_application.symbol, out _);

                    try
                    {
                        Task.Run(async () =>
                        {
                            await _connection.InvokeAsync("UpdatePositions", JsonConvert.SerializeObject(_positions.ToList()));
                        }).Wait();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.StackTrace);
                    }
                }
            }
            else
            {
                Position position = new Position
                {
                    exchange = order_application.exchange,
                    symbol = order_application.symbol,
                    code = order_application.code,
                    average_price = order_application.price,
                    quantity = order_application.quantity,
                    size = order_application.price * order_application.quantity
                };

                _positions[order_application.symbol] = position;

                try
                {
                    Task.Run(async () =>
                    {
                        await _connection.InvokeAsync("UpdatePositions", JsonConvert.SerializeObject(_positions.ToList()));
                    }).Wait();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        public void update_order(OrderApplication order_application)
        {
            Order order = new Order
            {
                exchange = order_application.exchange,
                id = order_application.id,
                symbol = order_application.symbol,
                code = order_application.code,
                side = order_application.side,
                price = order_application.price,
                quantity = order_application.quantity,
                size = order_application.price * order_application.quantity
            };

            _orders[order_application.id] = order;

            try
            {
                Task.Run(async () =>
                {
                    await _connection.InvokeAsync("UpdateOrders", JsonConvert.SerializeObject(_orders.ToList()));
                }).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public void update_transaction_record(OrderApplication order_application, ConcurrentDictionary<string, Orderbook> orderbooks)
        {
            TransactionRecord transaction_record = new TransactionRecord
            {
                exchange = _exchange,
                time = DateTime.Now.ToLocalTime(),
                symbol = order_application.symbol,
                code = order_application.code,
                executed_price = order_application.price,
                executed_quantity = order_application.quantity,
                executed_size = order_application.price * order_application.quantity,
                realized_profit = order_application.side == Side.SELL ? (orderbooks.ContainsKey(order_application.symbol) && orderbooks[order_application.symbol].bids.Count > 0 && _positions.ContainsKey(order_application.symbol) ? (orderbooks[order_application.symbol].bids.FirstOrDefault().price - _positions[order_application.symbol].average_price) * order_application.quantity : 0) : 0,
            };

            _transaction_records.Add(transaction_record);

            try
            {
                Task.Run(async () =>
                {
                    await _connection.InvokeAsync("UpdateTransactionRecord", JsonConvert.SerializeObject(_transaction_records));
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // updating initial balance
            Asset asset = _asset;
            asset.initial_balance += transaction_record.realized_profit;
            _asset = asset;

            try
            {
                Task.Run(async () =>
                {
                    await _connection.InvokeAsync("UpdateAsset", JsonConvert.SerializeObject(_asset));
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void update_order_record(OrderApplication order_application)
        {
            OrderRecord order_record = new OrderRecord
            {
                exchange = _exchange,
                time = DateTime.Now.ToLocalTime(),
                symbol = order_application.symbol,
                code = order_application.code,
                side = order_application.side,
                price = order_application.price,
                quantity = order_application.quantity,
                size = order_application.price * order_application.quantity,
                result = order_application.result,
                error_message = order_application.error_message
            };

            _order_records.Add(order_record);

            try
            {
                Task.Run(async () =>
                {
                    await _connection.InvokeAsync("UpdateOrderRecord", JsonConvert.SerializeObject(_order_records));
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public OrderApplication send_order(OrderApplication order_application, ConcurrentDictionary<string, Orderbook> orderbooks)
        {
            // special cases
            if (order_application.result == OrderExecutionResult.CANCELED)
            {
                if (_orders.TryRemove(order_application.id, out _))
                {
                    update_order_record(order_application);

                    return order_application;
                }
            }
            else
            {
                if (order_application.side == Side.BUY)
                {
                    if (order_application.price * order_application.quantity >= _asset.available_balance)
                    {
                        order_application.result = OrderExecutionResult.FAILED;
                        order_application.error_message = "예수금 초과";

                        update_order_record(order_application);

                        return order_application;
                    }
                }
                else if (order_application.side == Side.SELL)
                {
                    if (!_positions.ContainsKey(order_application.symbol))
                    {
                        order_application.result = OrderExecutionResult.FAILED;
                        order_application.error_message = "공매도 불가";

                        update_order_record(order_application);

                        return order_application;
                    }
                    else if (_positions[order_application.symbol].quantity < order_application.quantity)
                    {
                        order_application.result = OrderExecutionResult.FAILED;
                        order_application.error_message = "보유수량 초과";

                        update_order_record(order_application);

                        return order_application;
                    }
                }
            }

            // normal cases
            decimal filled_price = 0;

            if (orderbooks != null)
            {
                if (orderbooks.ContainsKey(order_application.symbol))
                {
                    Orderbook orderbook = orderbooks[order_application.symbol];

                    if (!order_application.Equals(default(Orderbook)))
                    {
                        // BUY
                        if (order_application.side == Side.BUY && orderbook.asks.Count > 0)
                        {
                            decimal lowest_ask = orderbook.asks.FirstOrDefault().price;

                            // market
                            if (order_application.price == 0 || order_application.price >= lowest_ask)
                            {
                                order_application.result = OrderExecutionResult.FILLED;
                                order_application.id = Guid.NewGuid().ToString();

                                filled_price = lowest_ask;
                            }
                            // limit
                            else
                            {
                                order_application.result = OrderExecutionResult.CREATED;
                                order_application.id = Guid.NewGuid().ToString();
                            }
                        }
                        // SELL
                        else if (order_application.side == Side.SELL && orderbook.bids.Count > 0)
                        {
                            decimal highest_bid = orderbook.bids.FirstOrDefault().price;

                            // market
                            if (order_application.price == 0 || order_application.price <= highest_bid)
                            {
                                order_application.result = OrderExecutionResult.FILLED;
                                order_application.id = Guid.NewGuid().ToString();

                                filled_price = highest_bid;
                            }
                            // limit
                            else
                            {
                                order_application.result = OrderExecutionResult.CREATED;
                                order_application.id = Guid.NewGuid().ToString();
                            }
                        }
                    }
                }
            }

            if (order_application.result == OrderExecutionResult.FILLED)
            {
                order_application.price = filled_price;

                update_transaction_record(order_application, orderbooks);
                update_position(order_application);
            }
            else if (order_application.result == OrderExecutionResult.CREATED) update_order(order_application);

            update_order_record(order_application);

            return order_application;
        }
    }
}
