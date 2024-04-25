namespace Server
{
    partial class FormServer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            form_server_table_layout_panel_buttons_button_run = new Button();
            form_server_rich_text_box_status = new RichTextBox();
            form_server_data_grid_view_port_setting = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            form_server_table_layout_panel_buttons = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)form_server_data_grid_view_port_setting).BeginInit();
            form_server_table_layout_panel_buttons.SuspendLayout();
            SuspendLayout();
            // 
            // form_server_table_layout_panel_buttons_button_run
            // 
            form_server_table_layout_panel_buttons_button_run.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            form_server_table_layout_panel_buttons_button_run.Location = new Point(3, 3);
            form_server_table_layout_panel_buttons_button_run.Name = "form_server_table_layout_panel_buttons_button_run";
            form_server_table_layout_panel_buttons_button_run.Size = new Size(69, 44);
            form_server_table_layout_panel_buttons_button_run.TabIndex = 0;
            form_server_table_layout_panel_buttons_button_run.Text = "가동";
            form_server_table_layout_panel_buttons_button_run.UseVisualStyleBackColor = true;
            form_server_table_layout_panel_buttons_button_run.Click += _on_form_server_table_layout_panel_buttons_button_run_clicked;
            // 
            // form_server_rich_text_box_status
            // 
            form_server_rich_text_box_status.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            form_server_rich_text_box_status.Location = new Point(12, 68);
            form_server_rich_text_box_status.Name = "form_server_rich_text_box_status";
            form_server_rich_text_box_status.Size = new Size(554, 46);
            form_server_rich_text_box_status.TabIndex = 1;
            form_server_rich_text_box_status.Text = "";
            // 
            // form_server_data_grid_view_port_setting
            // 
            form_server_data_grid_view_port_setting.AllowUserToAddRows = false;
            form_server_data_grid_view_port_setting.AllowUserToDeleteRows = false;
            form_server_data_grid_view_port_setting.AllowUserToResizeColumns = false;
            form_server_data_grid_view_port_setting.AllowUserToResizeRows = false;
            form_server_data_grid_view_port_setting.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            form_server_data_grid_view_port_setting.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("맑은 고딕", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            form_server_data_grid_view_port_setting.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            form_server_data_grid_view_port_setting.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            form_server_data_grid_view_port_setting.Columns.AddRange(new DataGridViewColumn[] { Column1 });
            form_server_data_grid_view_port_setting.Location = new Point(12, 12);
            form_server_data_grid_view_port_setting.Name = "form_server_data_grid_view_port_setting";
            form_server_data_grid_view_port_setting.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            form_server_data_grid_view_port_setting.RowsDefaultCellStyle = dataGridViewCellStyle2;
            form_server_data_grid_view_port_setting.Size = new Size(473, 50);
            form_server_data_grid_view_port_setting.TabIndex = 2;
            form_server_data_grid_view_port_setting.DataError += _on_data_grid_view_data_error_generated;
            form_server_data_grid_view_port_setting.SelectionChanged += _on_data_grid_view_selection_changed;
            // 
            // Column1
            // 
            Column1.HeaderText = "포트";
            Column1.Name = "Column1";
            // 
            // form_server_table_layout_panel_buttons
            // 
            form_server_table_layout_panel_buttons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            form_server_table_layout_panel_buttons.ColumnCount = 1;
            form_server_table_layout_panel_buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            form_server_table_layout_panel_buttons.Controls.Add(form_server_table_layout_panel_buttons_button_run, 0, 0);
            form_server_table_layout_panel_buttons.Location = new Point(491, 12);
            form_server_table_layout_panel_buttons.Name = "form_server_table_layout_panel_buttons";
            form_server_table_layout_panel_buttons.RowCount = 1;
            form_server_table_layout_panel_buttons.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            form_server_table_layout_panel_buttons.Size = new Size(75, 50);
            form_server_table_layout_panel_buttons.TabIndex = 3;
            // 
            // FormServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 126);
            Controls.Add(form_server_table_layout_panel_buttons);
            Controls.Add(form_server_data_grid_view_port_setting);
            Controls.Add(form_server_rich_text_box_status);
            Name = "FormServer";
            Text = "서버";
            ((System.ComponentModel.ISupportInitialize)form_server_data_grid_view_port_setting).EndInit();
            form_server_table_layout_panel_buttons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button form_server_table_layout_panel_buttons_button_run;
        private RichTextBox form_server_rich_text_box_status;
        private DataGridView form_server_data_grid_view_port_setting;
        private TableLayoutPanel form_server_table_layout_panel_buttons;
        private DataGridViewTextBoxColumn Column1;
    }
}
