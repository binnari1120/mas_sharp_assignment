using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Server
{
    internal partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();

            _initialize_ui();
        }

        private void _initialize_ui()
        {
            form_server_data_grid_view_port_setting.Rows.Add();

            if (form_server_data_grid_view_port_setting.RowCount >= 1 && form_server_data_grid_view_port_setting.ColumnCount >= 1)
            {
                form_server_data_grid_view_port_setting.Rows[0].Cells[0].Value = "5000";
            }
        }

        private bool _try_initialize_application()
        {
            bool result = false;

            if (int.TryParse(form_server_data_grid_view_port_setting.Rows[0].Cells[0].Value.ToString(), out int port))
            {
                var builder = WebApplication.CreateBuilder();
                builder.Services.AddCors();
                builder.Services.AddSignalR();

                var app = builder.Build();

                app.UseHttpsRedirection();
                app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

                app.MapHub<DataHub>("/dataHub");

                string url = $"http://localhost:{port}";

                string message = $" - {DateTime.Now} | Server is ready to be connected";
                Debug.WriteLine(message);
                update_status(message);

                message = $" - {DateTime.Now} | Listening on port: {port}";
                Debug.WriteLine(message);
                update_status(message);

                app.RunAsync(url);

                result = true;
            }
            else
            {
                MessageBox.Show("입력값 오류: 포트", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return result;
        }

        private void update_status(string message)
        {
            if (form_server_rich_text_box_status.InvokeRequired)
            {
                form_server_rich_text_box_status.Invoke(new Action<string>(update_status), message);

                return;
            }

            form_server_rich_text_box_status.AppendText($"{form_server_rich_text_box_status.Text}\n{message}");
        }

        private void _on_form_server_table_layout_panel_buttons_button_run_clicked(object sender, EventArgs e)
        {
            if (_try_initialize_application())
            {
                form_server_data_grid_view_port_setting.ReadOnly = true;
                form_server_table_layout_panel_buttons_button_run.Enabled = false;
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
    }
}
