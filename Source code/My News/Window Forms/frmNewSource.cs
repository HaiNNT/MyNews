using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using My_News.DAL;

namespace My_News.Window_Forms
{
    public delegate void NotifyLoadData(string str);
    public partial class frmNewSource : Form
    {
        public event NotifyLoadData NotifyEvent;
        public frmNewSource()
        {
            InitializeComponent();
            LoadAllData();
        }

        public void LoadAllData()
        {
            List<Source> list = new List<Source>();
            list = (new SourceDAL()).GetAllSource();
            dgdData.DataSource = list;
        }

        private void cmdInsert_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() != "" && txtHomepage.Text.Trim() != "")
            {
                bool rs = (new SourceDAL()).InsertSource(new Source(0, txtName.Text, txtHomepage.Text, txtLogo.Text));
                if (rs)
                {
                    MessageBox.Show("Thêm mới nguồn thành công.");
                    LoadAllData();
                    NotifyEvent(txtName.Text);
                }
                else
                {
                    MessageBox.Show("Thêm mới nguồn thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                }
            }
            else
            {
                MessageBox.Show("Thêm mới nguồn thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (dgdData.SelectedRows.Count > 0)
            {
                if (txtName.Text.Trim() != "" && txtHomepage.Text.Trim() != "")
                {
                    bool rs = (new SourceDAL()).UpdateSource(new Source(Convert.ToInt32(dgdData.SelectedRows[0].Cells[0].Value), txtName.Text, txtHomepage.Text, txtLogo.Text));
                    if (rs)
                    {
                        MessageBox.Show("Cập nhật nguồn thành công.");
                        LoadAllData();
                        NotifyEvent(txtName.Text);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật nguồn thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật nguồn thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 đối tượng để thực hiện thao tác!");
            }
        }

        private void dgdData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = ((int)dgdData.SelectedRows[0].Cells[0].Value).ToString();
            txtName.Text = (string)dgdData.SelectedRows[0].Cells[1].Value;
            txtHomepage.Text = (string)dgdData.SelectedRows[0].Cells[2].Value;
            txtLogo.Text = (string)dgdData.SelectedRows[0].Cells[3].Value;
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (dgdData.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(this, "Bạn có muốn xóa Nguồn này không?\nLưu ý: Tất cả các Rss link và tin tức nằm trong nguồn này đều sẽ bị xóa.", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    bool rs = (new SourceDAL()).DeleteSource(Convert.ToInt32(dgdData.SelectedRows[0].Cells[0].Value));
                    if (rs)
                    {
                        LoadAllData();
                        NotifyEvent("");
                    }
                    else
                    {
                        MessageBox.Show("Xóa nguồn thất bại. Vui lòng kiểm tra kết nối tới server.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 đối tượng để thực hiện thao tác!");
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Close();
        }



    }
}
