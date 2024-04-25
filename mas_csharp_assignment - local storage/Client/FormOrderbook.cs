using CommonModels;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;

namespace Client
{
    internal partial class FormOrderbook : Form
    {
        private readonly object _lock = new object();

        private static int _number_of_rows = 2000;
        private static int _first_displayed_scrolling_row_index = _number_of_rows / 2 - 12;
        private static int _ask_row_start_index = _number_of_rows / 2 - 1;
        private static int _bid_row_start_index = _number_of_rows / 2;

        private SymbolConstraint _symbol_constraint;
        private Integrator _integrator;

        private BackgroundWorker _data_updating_background_worker = null;

        private Size _this_form_size_before_collapse;
        private Point _status_tab_location_before_collapse;
        private Point _expand_collapse_button_location_before_collapse;
        private Size _this_form_current_size;
        private Point _status_tab_original_current_location;
        private Point _expand_collapse_button_current_location;
        private bool _is_expand_collapse_button_clicked = false;

        public FormOrderbook(SymbolConstraint symbol_constraint, Integrator integrator)
        {
            InitializeComponent();

            _symbol_constraint = symbol_constraint;
            _integrator = integrator;

            _initialize_ui();

            _data_updating_background_worker.RunWorkerAsync();
        }

        public void _initialize_ui()
        {
            _this_form_size_before_collapse = this.Size;
            _status_tab_location_before_collapse = form_orderbook_tab_control_status.Location;
            _expand_collapse_button_location_before_collapse = form_orderbook_button_expand_or_collapse.Location;

            _this_form_current_size = this.Size;
            _status_tab_original_current_location = form_orderbook_tab_control_status.Location;
            _expand_collapse_button_current_location = form_orderbook_button_expand_or_collapse.Location;

            // order setting
            DataGridView target_data_grid_view = form_orderbook_data_grid_view_order_setting;
            target_data_grid_view.Rows.Add(new DataGridViewRow());

            if (target_data_grid_view.RowCount == 1 && target_data_grid_view.ColumnCount == 4)
            {
                target_data_grid_view.Rows[0].Cells[0].Value = _symbol_constraint.price_step;
                target_data_grid_view.Rows[0].Cells[1].Value = _symbol_constraint.quantity_step;

                target_data_grid_view.Rows[0].Cells[0].ReadOnly = true;
                target_data_grid_view.Rows[0].Cells[1].ReadOnly = true;
            }

            // orderbook
            target_data_grid_view = form_orderbook_data_grid_view_orderbook;

            for (int i = 0; i < _number_of_rows; i++)
            {
                target_data_grid_view.Rows.Add(new DataGridViewRow());
            }

            // background worker
            _initialize_background_workers();
        }

        private void _initialize_background_workers()
        {
            _data_updating_background_worker = new BackgroundWorker();
            _data_updating_background_worker.DoWork += _on_data_updating_background_worker_started_working;
            _data_updating_background_worker.ProgressChanged += _on_data_updating_background_worker_updated_process;
            _data_updating_background_worker.WorkerReportsProgress = true;
            _data_updating_background_worker.WorkerReportsProgress = true;
        }

        private void _on_data_updating_background_worker_started_working(object sender, DoWorkEventArgs event_args)
        {
            if (_data_updating_background_worker != null)
            {
                while (true)
                {
                    _data_updating_background_worker.ReportProgress(0);

                    Thread.Sleep(200);
                }
            }
        }

        private void _clear_orderbook()
        {
            DataGridView target_data_grid_view = form_orderbook_data_grid_view_orderbook;

            foreach (DataGridViewRow target_data_grid_view_row in target_data_grid_view.Rows)
            {
                foreach (DataGridViewCell target_data_grid_view_cell in target_data_grid_view_row.Cells)
                {
                    target_data_grid_view_cell.Value = string.Empty;
                }
            }
        }

        private void _on_data_updating_background_worker_updated_process(object sender, ProgressChangedEventArgs event_args)
        {
            if (_integrator != null)
            {
                int i = 0;

                // orderbook
                ConcurrentDictionary<string, Orderbook> orderbooks = _integrator.get_orderbooks();
                DataGridView target_data_grid_view = form_orderbook_data_grid_view_orderbook;

                if (orderbooks.ContainsKey(_symbol_constraint.symbol))
                {
                    Orderbook orderbook = orderbooks[_symbol_constraint.symbol];

                    if (!orderbook.Equals(default(Orderbook)))
                    {
                        this.Text = $"종목명: {_symbol_constraint.symbol} (코드: {_symbol_constraint.code}) - 체결가: {orderbook.last_price}";

                        _clear_orderbook();

                        try
                        {
                            // asks
                            for (i = _ask_row_start_index; i >= 0; i--)
                            {
                                if (_ask_row_start_index - i < orderbook.asks.Count)
                                {
                                    target_data_grid_view.Rows[i].Cells[0].Value = $"Ask {_ask_row_start_index - i + 1}";
                                    target_data_grid_view.Rows[i].Cells[1].Value = orderbook.asks[_ask_row_start_index - i].price.ToString();
                                    target_data_grid_view.Rows[i].Cells[2].Value = orderbook.asks[_ask_row_start_index - i].quantity;
                                }
                                else break;
                            }

                            // bids
                            for (i = _bid_row_start_index; i < target_data_grid_view.Rows.Count; i++)
                            {
                                if (i - _bid_row_start_index < orderbook.bids.Count)
                                {
                                    target_data_grid_view.Rows[i].Cells[0].Value = $"Bid {i - _bid_row_start_index + 1}";
                                    target_data_grid_view.Rows[i].Cells[1].Value = orderbook.bids[i - _bid_row_start_index].price;
                                    target_data_grid_view.Rows[i].Cells[2].Value = orderbook.bids[i - _bid_row_start_index].quantity;
                                }
                                else break;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        // last price coloring
                        foreach (DataGridViewRow target_data_grid_view_row in target_data_grid_view.Rows)
                        {
                            if (!string.IsNullOrEmpty(target_data_grid_view_row.Cells[1].Value.ToString()) && decimal.TryParse(target_data_grid_view_row.Cells[1].Value.ToString(), out decimal ask_or_bid_price))
                            {
                                if (ask_or_bid_price == orderbook.last_price)
                                {
                                    target_data_grid_view_row.DefaultCellStyle.BackColor = Color.Yellow;
                                }
                                else target_data_grid_view_row.DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                }

                // positions
                target_data_grid_view = form_orderbook_tab_control_status_data_grid_view_positions;
                ConcurrentDictionary<string, Position> positions = _integrator.get_positions();

                int number_of_positions = positions.Keys.Count;

                if (number_of_positions >= target_data_grid_view.RowCount)
                {
                    for (i = 0; i < number_of_positions - target_data_grid_view.RowCount; i++)
                    {
                        target_data_grid_view.Rows.Add(new DataGridViewRow());

                        DataGridViewButtonCell data_grid_view_buitton_close_position = new DataGridViewButtonCell();
                        data_grid_view_buitton_close_position.Value = "➖";
                        target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[target_data_grid_view.Columns.Count - 1] = data_grid_view_buitton_close_position;
                    }
                }
                else if (number_of_positions < target_data_grid_view.RowCount)
                {
                    for (i = target_data_grid_view.RowCount - number_of_positions - 1; i >= 0; i--)
                    {
                        target_data_grid_view.Rows.RemoveAt(i);
                    }
                }

                i = 0;

                if (number_of_positions == target_data_grid_view.RowCount)
                {
                    foreach (var position in positions)
                    {
                        target_data_grid_view.Rows[i].Cells[0].Value = position.Value.symbol;
                        target_data_grid_view.Rows[i].Cells[1].Value = position.Value.code;
                        target_data_grid_view.Rows[i].Cells[2].Value = position.Value.average_price.ToString("#,##0");
                        target_data_grid_view.Rows[i].Cells[3].Value = position.Value.quantity;
                        target_data_grid_view.Rows[i].Cells[4].Value = (position.Value.average_price * position.Value.quantity).ToString("#,##0");
                        target_data_grid_view.Rows[i].Cells[5].Value = orderbooks.ContainsKey(position.Value.symbol) && orderbooks[position.Value.symbol].bids.Count > 0 ? ((orderbooks[position.Value.symbol].bids.FirstOrDefault().price - position.Value.average_price) * position.Value.quantity).ToString("#,##0") : 0;

                        i++;
                    }
                }

                // orders
                target_data_grid_view = form_orderbook_tab_control_status_data_grid_view_orders;
                ConcurrentDictionary<string, Order> orders = _integrator.get_orders();

                int number_of_orders = orders.Keys.Count;

                if (number_of_orders >= target_data_grid_view.RowCount)
                {
                    for (i = 0; i < number_of_orders - target_data_grid_view.RowCount; i++)
                    {
                        target_data_grid_view.Rows.Add(new DataGridViewRow());

                        DataGridViewButtonCell data_grid_view_buitton_delete_order = new DataGridViewButtonCell();
                        data_grid_view_buitton_delete_order.Value = "➖";
                        target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[target_data_grid_view.Columns.Count - 1] = data_grid_view_buitton_delete_order;
                    }
                }
                else if (number_of_orders < target_data_grid_view.RowCount)
                {
                    for (i = target_data_grid_view.RowCount - number_of_orders - 1; i >= 0; i--)
                    {
                        target_data_grid_view.Rows.RemoveAt(i);
                    }
                }

                i = 0;

                if (number_of_orders == target_data_grid_view.RowCount)
                {
                    foreach (var order in orders)
                    {
                        target_data_grid_view.Rows[i].Cells[0].Value = order.Value.id;
                        target_data_grid_view.Rows[i].Cells[1].Value = order.Value.symbol;
                        target_data_grid_view.Rows[i].Cells[2].Value = order.Value.code;
                        target_data_grid_view.Rows[i].Cells[3].Value = order.Value.side == Side.BUY ? "매수" : "매도";
                        target_data_grid_view.Rows[i].Cells[4].Value = order.Value.price.ToString("#,##0");
                        target_data_grid_view.Rows[i].Cells[5].Value = order.Value.quantity;
                        target_data_grid_view.Rows[i].Cells[6].Value = order.Value.size.ToString("#,##0");

                        i++;
                    }
                }

                lock (_lock)
                {
                    // transaction records
                    target_data_grid_view = form_orderbook_tab_control_status_data_grid_view_transaction_records;
                    List<TransactionRecord> transaction_records = _integrator.get_transaction_records();

                    int number_of_transaction_records = transaction_records.Count;

                    if (number_of_transaction_records >= target_data_grid_view.RowCount)
                    {
                        for (i = 0; i < number_of_transaction_records - target_data_grid_view.RowCount; i++)
                        {
                            target_data_grid_view.Rows.Add(new DataGridViewRow());
                        }
                    }
                    else if (number_of_transaction_records < target_data_grid_view.RowCount)
                    {
                        for (i = target_data_grid_view.RowCount - number_of_transaction_records - 1; i >= 0; i--)
                        {
                            target_data_grid_view.Rows.RemoveAt(i);
                        }
                    }

                    i = 0;

                    if (number_of_transaction_records == target_data_grid_view.RowCount)
                    {
                        foreach (TransactionRecord transaction_record in transaction_records.OrderByDescending(x => x.time))
                        {
                            target_data_grid_view.Rows[i].Cells[0].Value = transaction_record.time;
                            target_data_grid_view.Rows[i].Cells[1].Value = transaction_record.symbol;
                            target_data_grid_view.Rows[i].Cells[2].Value = transaction_record.code;
                            target_data_grid_view.Rows[i].Cells[3].Value = transaction_record.executed_price.ToString("#,##0");
                            target_data_grid_view.Rows[i].Cells[4].Value = transaction_record.executed_quantity;
                            target_data_grid_view.Rows[i].Cells[5].Value = transaction_record.executed_size.ToString("#,##0");
                            target_data_grid_view.Rows[i].Cells[6].Value = transaction_record.realized_profit.ToString("#,##0");

                            i++;
                        }
                    }

                    // order records
                    target_data_grid_view = form_orderbook_tab_control_status_data_grid_view_order_records;
                    List<OrderRecord> order_records = _integrator.get_order_records();

                    int number_of_order_records = order_records.Count;

                    if (number_of_order_records >= target_data_grid_view.RowCount)
                    {
                        for (i = 0; i < number_of_order_records - target_data_grid_view.RowCount; i++)
                        {
                            target_data_grid_view.Rows.Add(new DataGridViewRow());
                        }
                    }
                    else if (number_of_order_records < target_data_grid_view.RowCount)
                    {
                        for (i = target_data_grid_view.RowCount - number_of_order_records - 1; i >= 0; i--)
                        {
                            target_data_grid_view.Rows.RemoveAt(i);
                        }
                    }

                    i = 0;

                    if (number_of_order_records == target_data_grid_view.RowCount)
                    {
                        foreach (OrderRecord order_record in order_records.OrderByDescending(x => x.time))
                        {
                            target_data_grid_view.Rows[i].Cells[0].Value = order_record.time;
                            target_data_grid_view.Rows[i].Cells[1].Value = order_record.symbol;
                            target_data_grid_view.Rows[i].Cells[2].Value = order_record.code;
                            target_data_grid_view.Rows[i].Cells[3].Value = order_record.side == Side.BUY ? "매수" : "매도";
                            target_data_grid_view.Rows[i].Cells[4].Value = order_record.price.ToString("#,##0");
                            target_data_grid_view.Rows[i].Cells[5].Value = order_record.quantity;
                            target_data_grid_view.Rows[i].Cells[6].Value = order_record.size.ToString("#,##0");
                            target_data_grid_view.Rows[i].Cells[7].Value = order_record.result;

                            i++;
                        }
                    }
                }
            }
        }

        private void _on_form_orderbook_loaded(object sender, EventArgs event_args)
        {
            form_orderbook_data_grid_view_orderbook.FirstDisplayedScrollingRowIndex = _first_displayed_scrolling_row_index;
        }

        private void _on_form_orderbook_size_changed(object sender, EventArgs e)
        {
            if (!_is_expand_collapse_button_clicked)
            {
                _this_form_size_before_collapse = this.Size;
                _status_tab_location_before_collapse = form_orderbook_tab_control_status.Location;
                _expand_collapse_button_location_before_collapse = form_orderbook_button_expand_or_collapse.Location;
            }

            _this_form_current_size = this.Size;
            _status_tab_original_current_location = form_orderbook_tab_control_status.Location;
            _expand_collapse_button_current_location = form_orderbook_button_expand_or_collapse.Location;
        }

        private void _on_form_main_button_expand_or_collapse_clicked(object sender, EventArgs e)
        {
            _is_expand_collapse_button_clicked = true;

            form_orderbook_tab_control_status.Visible = !form_orderbook_tab_control_status.Visible;

            if (form_orderbook_tab_control_status.Visible)
            {
                form_orderbook_button_expand_or_collapse.Text = "▲";

                form_orderbook_data_grid_view_orderbook.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form_orderbook_tab_control_status.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form_orderbook_button_expand_or_collapse.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                this.Size = _this_form_size_before_collapse;
                form_orderbook_tab_control_status.Location = _status_tab_location_before_collapse;
                form_orderbook_button_expand_or_collapse.Location = _expand_collapse_button_current_location;

                form_orderbook_data_grid_view_orderbook.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form_orderbook_button_expand_or_collapse.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
            else
            {
                form_orderbook_button_expand_or_collapse.Text = "▼";

                form_orderbook_data_grid_view_orderbook.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                form_orderbook_button_expand_or_collapse.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                this.Size = new Size(this.Size.Width, this.Size.Height - form_orderbook_tab_control_status.Height);
                form_orderbook_tab_control_status.Location = new Point(0, 0);

                form_orderbook_data_grid_view_orderbook.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                form_orderbook_button_expand_or_collapse.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        private void _on_data_grid_view_data_error_generated(object sender, DataGridViewDataErrorEventArgs event_args)
        {

        }

        private void _on_data_grid_view_selection_changed(object sender, EventArgs event_args)
        {
            if (sender is DataGridView target_data_grid_view)
            {
                target_data_grid_view.ClearSelection();
            }
        }

        private void _on_form_orderbook_closing(object sender, FormClosingEventArgs event_args)
        {
            if (event_args.CloseReason == CloseReason.UserClosing)
            {
                event_args.Cancel = true;

                this.Hide();
            }
        }

        private void _on_form_orderbook_table_layout_panel_buttons_button_buy_clicked(object sender, EventArgs event_args)
        {
            if (_integrator != null)
            {
                DataGridView target_data_grid_view = form_orderbook_data_grid_view_order_setting;

                if (target_data_grid_view.RowCount == 1 && target_data_grid_view.ColumnCount == 4)
                {
                    if (target_data_grid_view.Rows[0].Cells[2].Value != null && decimal.TryParse(target_data_grid_view.Rows[0].Cells[2].Value.ToString(), out decimal order_price) && order_price >= 0)
                    {
                        if (target_data_grid_view.Rows[0].Cells[3].Value != null && int.TryParse(target_data_grid_view.Rows[0].Cells[3].Value.ToString(), out int order_quantity) && order_quantity > 0)
                        {
                            try
                            {
                                OrderApplication order_application = new OrderApplication
                                {
                                    exchange = _integrator.get_exchange(),
                                    symbol = _symbol_constraint.symbol,
                                    code = _symbol_constraint.code,
                                    price = order_price,
                                    quantity = order_quantity,
                                    side = Side.BUY
                                };

                                OrderApplication resulted_order_application = _integrator.send_order(order_application);

                                Debug.WriteLine(resulted_order_application);

                                if (resulted_order_application.result == OrderExecutionResult.FAILED)
                                {
                                    MessageBox.Show($"주문 실패: {resulted_order_application.error_message}", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex);
                            }
                        }
                        else
                        {
                            MessageBox.Show("입력값 오류: 주문수량(주)", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("입력값 오류: 주문가격(원)", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return;
                    }
                }
            }
        }

        private void _on_form_orderbook_table_layout_panel_buttons_button_sell_clicked(object sender, EventArgs event_args)
        {
            if (_integrator != null)
            {
                DataGridView target_data_grid_view = form_orderbook_data_grid_view_order_setting;

                if (target_data_grid_view.RowCount == 1 && target_data_grid_view.ColumnCount == 4)
                {
                    if (target_data_grid_view.Rows[0].Cells[2].Value != null && decimal.TryParse(target_data_grid_view.Rows[0].Cells[2].Value.ToString(), out decimal order_price) && order_price >= 0)
                    {
                        if (target_data_grid_view.Rows[0].Cells[3].Value != null && int.TryParse(target_data_grid_view.Rows[0].Cells[3].Value.ToString(), out int order_quantity) && order_quantity > 0)
                        {
                            try
                            {
                                OrderApplication order_application = new OrderApplication
                                {
                                    exchange = _integrator.get_exchange(),
                                    symbol = _symbol_constraint.symbol,
                                    code = _symbol_constraint.code,
                                    price = order_price,
                                    quantity = order_quantity,
                                    side = Side.SELL
                                };

                                OrderApplication resulted_order_application = _integrator.send_order(order_application);

                                Debug.WriteLine(resulted_order_application);

                                if (resulted_order_application.result == OrderExecutionResult.FAILED)
                                {
                                    MessageBox.Show($"주문 실패: {resulted_order_application.error_message}", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex);
                            }
                        }
                        else
                        {
                            MessageBox.Show("입력값 오류: 주문수량(주)", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("입력값 오류: 주문가격(원)", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return;
                    }
                }
            }
        }

        private void _on_form_orderbook_tab_control_status_data_grid_view_positions_cell_clicked(object sender, DataGridViewCellEventArgs event_args)
        {
            if (_integrator != null)
            {
                DataGridView target_data_grid_view = form_orderbook_tab_control_status_data_grid_view_positions;

                if (event_args.RowIndex >= 0)
                {
                    if (event_args.ColumnIndex == target_data_grid_view.ColumnCount - 1)
                    {
                        try
                        {
                            string symbol = target_data_grid_view.Rows[event_args.RowIndex].Cells[0].Value.ToString();
                            string code = target_data_grid_view.Rows[event_args.RowIndex].Cells[1].Value.ToString();
                            decimal average_price = Convert.ToDecimal(target_data_grid_view.Rows[event_args.RowIndex].Cells[2].Value.ToString());
                            int quantity = Convert.ToInt32(target_data_grid_view.Rows[event_args.RowIndex].Cells[3].Value.ToString());

                            OrderApplication order_application = new OrderApplication
                            {
                                exchange = _integrator.get_exchange(),
                                symbol = symbol,
                                code = code,
                                price = 0,
                                quantity = quantity,
                                side = Side.SELL
                            };

                            OrderApplication resulted_order_application = _integrator.send_order(order_application);

                            Debug.WriteLine(resulted_order_application);

                            if (resulted_order_application.result == OrderExecutionResult.FAILED)
                            {
                                MessageBox.Show($"주문 실패: {resulted_order_application.error_message}", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
            }
        }

        private void _on_form_orderbook_tab_control_status_data_grid_view_orders_cell_clicked(object sender, DataGridViewCellEventArgs event_args)
        {
            if (_integrator != null)
            {
                DataGridView target_data_grid_view = form_orderbook_tab_control_status_data_grid_view_orders;

                if (event_args.RowIndex >= 0)
                {
                    if (event_args.ColumnIndex == target_data_grid_view.ColumnCount - 1)
                    {
                        try
                        {
                            string order_id = target_data_grid_view.Rows[event_args.RowIndex].Cells[0].Value.ToString();
                            string symbol = target_data_grid_view.Rows[event_args.RowIndex].Cells[1].Value.ToString();
                            string code = target_data_grid_view.Rows[event_args.RowIndex].Cells[2].Value.ToString();
                            Side side = Converter.get_side(target_data_grid_view.Rows[event_args.RowIndex].Cells[3].Value.ToString());
                            decimal price = Convert.ToDecimal(target_data_grid_view.Rows[event_args.RowIndex].Cells[4].Value.ToString());
                            int quantity = Convert.ToInt32(target_data_grid_view.Rows[event_args.RowIndex].Cells[5].Value.ToString());

                            OrderApplication order_application = new OrderApplication
                            {
                                exchange = _integrator.get_exchange(),
                                id = order_id,
                                symbol = symbol,
                                code = code,
                                side = side == Side.BUY ? Side.SELL : Side.BUY,
                                price = price,
                                quantity = quantity,
                                result = OrderExecutionResult.CANCELED
                            };

                            OrderApplication resulted_order_application = _integrator.send_order(order_application);

                            Debug.WriteLine(resulted_order_application);

                            if (resulted_order_application.result == OrderExecutionResult.FAILED)
                            {
                                MessageBox.Show($"주문 실패: {resulted_order_application.error_message}", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                }
            }
        }
    }
}
