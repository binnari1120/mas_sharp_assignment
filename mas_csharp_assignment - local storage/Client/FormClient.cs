using CommonModels;
using System.ComponentModel;

namespace Client
{
    public partial class FormClient : Form
    {
        private Integrator _integrator = null;
        private Dictionary<string, Form> _orderbook_forms = null;

        private BackgroundWorker _data_updating_background_worker = null;

        public FormClient()
        {
            InitializeComponent();

            _initialize_ui();

            _orderbook_forms = new Dictionary<string, Form>();
        }

        private void _initialize_ui()
        {
            DataGridView target_data_grid_view = form_client_data_grid_view_account_information;

            target_data_grid_view.Rows.Add(new DataGridViewRow());

            if (target_data_grid_view.RowCount == 1 && target_data_grid_view.ColumnCount == 7)
            {
                target_data_grid_view.Rows[0].Cells[0].Value = "미래에셋";
                target_data_grid_view.Rows[0].Cells[1].Value = "5000";

                target_data_grid_view.Rows[0].Cells[0].ReadOnly = false;
                target_data_grid_view.Rows[0].Cells[1].ReadOnly = false;
                target_data_grid_view.Rows[0].Cells[2].ReadOnly = true;
                target_data_grid_view.Rows[0].Cells[3].ReadOnly = true;
                target_data_grid_view.Rows[0].Cells[4].ReadOnly = true;
                target_data_grid_view.Rows[0].Cells[5].ReadOnly = true;
            }

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

        private void _on_data_updating_background_worker_updated_process(object sender, ProgressChangedEventArgs event_args)
        {
            if (_integrator != null)
            {
                // asset
                Asset asset = _integrator.get_asset();

                form_client_data_grid_view_account_information.Rows[0].Cells[2].Value = asset.initial_balance.ToString("#,##0");
                form_client_data_grid_view_account_information.Rows[0].Cells[3].Value = (asset.total_position_size + asset.total_order_size).ToString("#,##0"); ;
                form_client_data_grid_view_account_information.Rows[0].Cells[4].Value = asset.available_balance.ToString("#,##0");
                form_client_data_grid_view_account_information.Rows[0].Cells[5].Value = asset.pnl.ToString("#,##0");
                form_client_data_grid_view_account_information.Rows[0].Cells[6].Value = asset.equity.ToString("#,##0");
            }
        }

        private void _on_form_main_table_layout_panel_buttons_button_login_clicked(object sender, EventArgs e)
        {
            if (form_client_data_grid_view_account_information.RowCount == 1 && form_client_data_grid_view_account_information.ColumnCount == 7)
            {
                string exchange_name = form_client_data_grid_view_account_information.Rows[0].Cells[0].Value.ToString();
                Exchange exchange = Converter.get_exchange(form_client_data_grid_view_account_information.Rows[0].Cells[0].Value.ToString());

                if (!exchange.Equals(default(Exchange)))
                {
                    if (int.TryParse(form_client_data_grid_view_account_information.Rows[0].Cells[1].Value.ToString(), out int port))
                    {
                        _integrator = new Integrator(exchange, port);

                        if (_integrator.on_connected())
                        {
                            if (_integrator.on_all_symbol_constraints_loaded())
                            {
                                _load_symbol_constraints();

                                Asset asset = _integrator.get_asset();
                                form_client_data_grid_view_account_information.Rows[0].Cells[2].Value = asset.initial_balance;

                                form_client_data_grid_view_account_information.ReadOnly = true;
                                form_client_table_layout_panel_login_button_login.Enabled = false;

                                _integrator.start_streams();
                                _data_updating_background_worker.RunWorkerAsync();
                            }
                        }
                        else
                        {
                            MessageBox.Show("연결 실패", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("입력값 오류: 포트", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return;
                    }
                }
            }
        }

        private void _load_symbol_constraints()
        {
            DataGridView target_data_grid_view = form_client_data_grid_view_dashboard;

            List<SymbolConstraint> symbol_constraints = _integrator.get_symbol_constraints();

            foreach (SymbolConstraint symbol_constraint in symbol_constraints)
            {
                target_data_grid_view.Rows.Add(new DataGridViewRow());
                target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[0].Value = target_data_grid_view.Rows.Count;
                target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[1].Value = symbol_constraint.code;
                target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[2].Value = symbol_constraint.symbol;
                target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[3].Value = symbol_constraint.price_step;
                target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[4].Value = symbol_constraint.quantity_step;

                DataGridViewButtonCell data_grid_view_button_cell_open_orderbook = new DataGridViewButtonCell();
                data_grid_view_button_cell_open_orderbook.Value = "보기";
                target_data_grid_view.Rows[target_data_grid_view.Rows.Count - 1].Cells[5] = data_grid_view_button_cell_open_orderbook;

                //_orderbook_forms[symbol_constraint.code] = new form_orderbook();
            }
        }

        private void _on_form_main_data_grid_view_dashboard_cell_content_clicked(object sender, DataGridViewCellEventArgs event_args)
        {
            if (_integrator != null)
            {
                if (sender is DataGridView target_data_grid_view)
                {
                    if (event_args.RowIndex != -1)
                    {
                        if (event_args.ColumnIndex == target_data_grid_view.ColumnCount - 1)
                        {
                            string code = target_data_grid_view.Rows[event_args.RowIndex].Cells[1].Value.ToString();
                            string symbol = target_data_grid_view.Rows[event_args.RowIndex].Cells[2].Value.ToString();

                            SymbolConstraint symbol_constraint = _integrator.get_symbol_constraints().FirstOrDefault(x => x.code == code && x.symbol == symbol);

                            if (!_orderbook_forms.ContainsKey(symbol)) _orderbook_forms[symbol] = new FormOrderbook(symbol_constraint, _integrator);

                            _orderbook_forms[symbol].StartPosition = FormStartPosition.CenterParent;
                            _orderbook_forms[symbol].Show();
                        }
                    }
                }
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

        private void _on_form_client_table_layout_panel_reset_button_reset_clicked(object sender, EventArgs e)
        {
            if (_integrator != null) _integrator.reset(true);
        }
    }
}
