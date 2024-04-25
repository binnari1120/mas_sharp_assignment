namespace Client
{
    partial class FormClient
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            form_client_table_layout_panel_login_button_login = new Button();
            form_client_table_layout_panel_login = new TableLayoutPanel();
            form_client_data_grid_view_account_information = new DataGridView();
            form_client_data_grid_view_account_information_column_1 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_account_information_column_2 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_account_information_column_3 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_account_information_column_4 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_account_information_column_5 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_account_information_column_6 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_account_information_column_7 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_dashboard = new DataGridView();
            form_client_data_grid_view_dashboard_column_1 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_dashboard_column_2 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_dashboard_column_3 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_dashboard_column_4 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_dashboard_column_5 = new DataGridViewTextBoxColumn();
            form_client_data_grid_view_dashboard_column_6 = new DataGridViewTextBoxColumn();
            form_client_table_layout_panel_reset = new TableLayoutPanel();
            form_client_table_layout_panel_reset_button_reset = new Button();
            form_client_table_layout_panel_login.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)form_client_data_grid_view_account_information).BeginInit();
            ((System.ComponentModel.ISupportInitialize)form_client_data_grid_view_dashboard).BeginInit();
            form_client_table_layout_panel_reset.SuspendLayout();
            SuspendLayout();
            // 
            // form_client_table_layout_panel_login_button_login
            // 
            form_client_table_layout_panel_login_button_login.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            form_client_table_layout_panel_login_button_login.Location = new Point(3, 4);
            form_client_table_layout_panel_login_button_login.Margin = new Padding(3, 4, 3, 4);
            form_client_table_layout_panel_login_button_login.Name = "form_client_table_layout_panel_login_button_login";
            form_client_table_layout_panel_login_button_login.Size = new Size(106, 37);
            form_client_table_layout_panel_login_button_login.TabIndex = 0;
            form_client_table_layout_panel_login_button_login.Text = "접속";
            form_client_table_layout_panel_login_button_login.UseVisualStyleBackColor = true;
            form_client_table_layout_panel_login_button_login.Click += _on_form_main_table_layout_panel_buttons_button_login_clicked;
            // 
            // form_client_table_layout_panel_login
            // 
            form_client_table_layout_panel_login.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            form_client_table_layout_panel_login.ColumnCount = 1;
            form_client_table_layout_panel_login.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            form_client_table_layout_panel_login.Controls.Add(form_client_table_layout_panel_login_button_login, 0, 0);
            form_client_table_layout_panel_login.Location = new Point(692, 16);
            form_client_table_layout_panel_login.Margin = new Padding(3, 4, 3, 4);
            form_client_table_layout_panel_login.Name = "form_client_table_layout_panel_login";
            form_client_table_layout_panel_login.RowCount = 1;
            form_client_table_layout_panel_login.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            form_client_table_layout_panel_login.Size = new Size(112, 45);
            form_client_table_layout_panel_login.TabIndex = 1;
            // 
            // form_client_data_grid_view_account_information
            // 
            form_client_data_grid_view_account_information.AllowUserToAddRows = false;
            form_client_data_grid_view_account_information.AllowUserToDeleteRows = false;
            form_client_data_grid_view_account_information.AllowUserToResizeColumns = false;
            form_client_data_grid_view_account_information.AllowUserToResizeRows = false;
            form_client_data_grid_view_account_information.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            form_client_data_grid_view_account_information.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            form_client_data_grid_view_account_information.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            form_client_data_grid_view_account_information.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            form_client_data_grid_view_account_information.Columns.AddRange(new DataGridViewColumn[] { form_client_data_grid_view_account_information_column_1, form_client_data_grid_view_account_information_column_2, form_client_data_grid_view_account_information_column_3, form_client_data_grid_view_account_information_column_4, form_client_data_grid_view_account_information_column_5, form_client_data_grid_view_account_information_column_6, form_client_data_grid_view_account_information_column_7 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            form_client_data_grid_view_account_information.DefaultCellStyle = dataGridViewCellStyle2;
            form_client_data_grid_view_account_information.EditMode = DataGridViewEditMode.EditOnEnter;
            form_client_data_grid_view_account_information.Location = new Point(13, 16);
            form_client_data_grid_view_account_information.Margin = new Padding(3, 4, 3, 4);
            form_client_data_grid_view_account_information.Name = "form_client_data_grid_view_account_information";
            form_client_data_grid_view_account_information.RowHeadersVisible = false;
            form_client_data_grid_view_account_information.RowTemplate.Height = 23;
            form_client_data_grid_view_account_information.Size = new Size(673, 45);
            form_client_data_grid_view_account_information.TabIndex = 2;
            form_client_data_grid_view_account_information.DataError += _on_data_grid_view_data_error_generated;
            form_client_data_grid_view_account_information.SelectionChanged += _on_data_grid_view_selection_changed;
            // 
            // form_client_data_grid_view_account_information_column_1
            // 
            form_client_data_grid_view_account_information_column_1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_account_information_column_1.HeaderText = "증권사";
            form_client_data_grid_view_account_information_column_1.Name = "form_client_data_grid_view_account_information_column_1";
            // 
            // form_client_data_grid_view_account_information_column_2
            // 
            form_client_data_grid_view_account_information_column_2.HeaderText = "포트";
            form_client_data_grid_view_account_information_column_2.Name = "form_client_data_grid_view_account_information_column_2";
            // 
            // form_client_data_grid_view_account_information_column_3
            // 
            form_client_data_grid_view_account_information_column_3.HeaderText = "원금";
            form_client_data_grid_view_account_information_column_3.Name = "form_client_data_grid_view_account_information_column_3";
            // 
            // form_client_data_grid_view_account_information_column_4
            // 
            form_client_data_grid_view_account_information_column_4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_account_information_column_4.HeaderText = "매입금액";
            form_client_data_grid_view_account_information_column_4.Name = "form_client_data_grid_view_account_information_column_4";
            // 
            // form_client_data_grid_view_account_information_column_5
            // 
            form_client_data_grid_view_account_information_column_5.HeaderText = "예수금";
            form_client_data_grid_view_account_information_column_5.Name = "form_client_data_grid_view_account_information_column_5";
            // 
            // form_client_data_grid_view_account_information_column_6
            // 
            form_client_data_grid_view_account_information_column_6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_account_information_column_6.HeaderText = "평가수익";
            form_client_data_grid_view_account_information_column_6.Name = "form_client_data_grid_view_account_information_column_6";
            // 
            // form_client_data_grid_view_account_information_column_7
            // 
            form_client_data_grid_view_account_information_column_7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_account_information_column_7.HeaderText = "평가잔고";
            form_client_data_grid_view_account_information_column_7.Name = "form_client_data_grid_view_account_information_column_7";
            // 
            // form_client_data_grid_view_dashboard
            // 
            form_client_data_grid_view_dashboard.AllowUserToAddRows = false;
            form_client_data_grid_view_dashboard.AllowUserToDeleteRows = false;
            form_client_data_grid_view_dashboard.AllowUserToResizeColumns = false;
            form_client_data_grid_view_dashboard.AllowUserToResizeRows = false;
            form_client_data_grid_view_dashboard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            form_client_data_grid_view_dashboard.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            form_client_data_grid_view_dashboard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            form_client_data_grid_view_dashboard.Columns.AddRange(new DataGridViewColumn[] { form_client_data_grid_view_dashboard_column_1, form_client_data_grid_view_dashboard_column_2, form_client_data_grid_view_dashboard_column_3, form_client_data_grid_view_dashboard_column_4, form_client_data_grid_view_dashboard_column_5, form_client_data_grid_view_dashboard_column_6 });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("굴림", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            form_client_data_grid_view_dashboard.DefaultCellStyle = dataGridViewCellStyle4;
            form_client_data_grid_view_dashboard.EditMode = DataGridViewEditMode.EditOnEnter;
            form_client_data_grid_view_dashboard.Location = new Point(13, 69);
            form_client_data_grid_view_dashboard.Margin = new Padding(3, 4, 3, 4);
            form_client_data_grid_view_dashboard.Name = "form_client_data_grid_view_dashboard";
            form_client_data_grid_view_dashboard.RowHeadersVisible = false;
            form_client_data_grid_view_dashboard.RowTemplate.Height = 23;
            form_client_data_grid_view_dashboard.Size = new Size(791, 334);
            form_client_data_grid_view_dashboard.TabIndex = 3;
            form_client_data_grid_view_dashboard.CellContentClick += _on_form_main_data_grid_view_dashboard_cell_content_clicked;
            form_client_data_grid_view_dashboard.DataError += _on_data_grid_view_data_error_generated;
            form_client_data_grid_view_dashboard.SelectionChanged += _on_data_grid_view_selection_changed;
            // 
            // form_client_data_grid_view_dashboard_column_1
            // 
            form_client_data_grid_view_dashboard_column_1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            form_client_data_grid_view_dashboard_column_1.HeaderText = "#";
            form_client_data_grid_view_dashboard_column_1.Name = "form_client_data_grid_view_dashboard_column_1";
            form_client_data_grid_view_dashboard_column_1.Width = 80;
            // 
            // form_client_data_grid_view_dashboard_column_2
            // 
            form_client_data_grid_view_dashboard_column_2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_dashboard_column_2.HeaderText = "종목코드";
            form_client_data_grid_view_dashboard_column_2.Name = "form_client_data_grid_view_dashboard_column_2";
            // 
            // form_client_data_grid_view_dashboard_column_3
            // 
            form_client_data_grid_view_dashboard_column_3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_dashboard_column_3.FillWeight = 150F;
            form_client_data_grid_view_dashboard_column_3.HeaderText = "종목";
            form_client_data_grid_view_dashboard_column_3.Name = "form_client_data_grid_view_dashboard_column_3";
            // 
            // form_client_data_grid_view_dashboard_column_4
            // 
            form_client_data_grid_view_dashboard_column_4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_dashboard_column_4.HeaderText = "호가단위(원)";
            form_client_data_grid_view_dashboard_column_4.Name = "form_client_data_grid_view_dashboard_column_4";
            // 
            // form_client_data_grid_view_dashboard_column_5
            // 
            form_client_data_grid_view_dashboard_column_5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_dashboard_column_5.HeaderText = "수량단위(주)";
            form_client_data_grid_view_dashboard_column_5.Name = "form_client_data_grid_view_dashboard_column_5";
            // 
            // form_client_data_grid_view_dashboard_column_6
            // 
            form_client_data_grid_view_dashboard_column_6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            form_client_data_grid_view_dashboard_column_6.HeaderText = "🔍";
            form_client_data_grid_view_dashboard_column_6.Name = "form_client_data_grid_view_dashboard_column_6";
            // 
            // form_client_table_layout_panel_reset
            // 
            form_client_table_layout_panel_reset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            form_client_table_layout_panel_reset.ColumnCount = 1;
            form_client_table_layout_panel_reset.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            form_client_table_layout_panel_reset.Controls.Add(form_client_table_layout_panel_reset_button_reset, 0, 0);
            form_client_table_layout_panel_reset.Location = new Point(13, 411);
            form_client_table_layout_panel_reset.Margin = new Padding(3, 4, 3, 4);
            form_client_table_layout_panel_reset.Name = "form_client_table_layout_panel_reset";
            form_client_table_layout_panel_reset.RowCount = 1;
            form_client_table_layout_panel_reset.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            form_client_table_layout_panel_reset.Size = new Size(791, 45);
            form_client_table_layout_panel_reset.TabIndex = 4;
            // 
            // form_client_table_layout_panel_reset_button_reset
            // 
            form_client_table_layout_panel_reset_button_reset.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            form_client_table_layout_panel_reset_button_reset.Location = new Point(3, 4);
            form_client_table_layout_panel_reset_button_reset.Margin = new Padding(3, 4, 3, 4);
            form_client_table_layout_panel_reset_button_reset.Name = "form_client_table_layout_panel_reset_button_reset";
            form_client_table_layout_panel_reset_button_reset.Size = new Size(785, 37);
            form_client_table_layout_panel_reset_button_reset.TabIndex = 0;
            form_client_table_layout_panel_reset_button_reset.Text = "초기화";
            form_client_table_layout_panel_reset_button_reset.UseVisualStyleBackColor = true;
            form_client_table_layout_panel_reset_button_reset.Click += _on_form_client_table_layout_panel_reset_button_reset_clicked;
            // 
            // form_client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(816, 469);
            Controls.Add(form_client_table_layout_panel_reset);
            Controls.Add(form_client_data_grid_view_dashboard);
            Controls.Add(form_client_data_grid_view_account_information);
            Controls.Add(form_client_table_layout_panel_login);
            Margin = new Padding(3, 4, 3, 4);
            Name = "form_client";
            Text = "클라이언트";
            form_client_table_layout_panel_login.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)form_client_data_grid_view_account_information).EndInit();
            ((System.ComponentModel.ISupportInitialize)form_client_data_grid_view_dashboard).EndInit();
            form_client_table_layout_panel_reset.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button form_client_table_layout_panel_login_button_login;
        private System.Windows.Forms.TableLayoutPanel form_client_table_layout_panel_login;
        private System.Windows.Forms.DataGridView form_client_data_grid_view_account_information;
        private System.Windows.Forms.DataGridView form_client_data_grid_view_dashboard;
        private DataGridViewTextBoxColumn form_client_data_grid_view_dashboard_column_1;
        private DataGridViewTextBoxColumn form_client_data_grid_view_dashboard_column_2;
        private DataGridViewTextBoxColumn form_client_data_grid_view_dashboard_column_3;
        private DataGridViewTextBoxColumn form_client_data_grid_view_dashboard_column_4;
        private DataGridViewTextBoxColumn form_client_data_grid_view_dashboard_column_5;
        private DataGridViewTextBoxColumn form_client_data_grid_view_dashboard_column_6;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_1;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_2;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_3;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_4;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_5;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_6;
        private DataGridViewTextBoxColumn form_client_data_grid_view_account_information_column_7;
        private TableLayoutPanel form_client_table_layout_panel_reset;
        private Button form_client_table_layout_panel_reset_button_reset;
    }
}

