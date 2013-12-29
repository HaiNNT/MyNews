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
    public partial class frmNewCategory : Form
    {
        public event NotifyLoadData NotifyEvent;
        public frmNewCategory()
        {
            InitializeComponent();
            LoadAllData();
        }

        public void LoadAllData()
        {
            List<Category> list = new List<Category>();
            list = (new CategoryDAL()).GetAllCategory();
            dgdData.DataSource = list;
        }

        private void cmdInsert_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() != "")
            {
                bool rs = (new CategoryDAL()).InsertCategory(new Category(0, txtName.Text));
                if (rs)
                {
                    MessageBox.Show("Thêm mới chuyên mục thành công.");
                    LoadAllData();
                    NotifyEvent(txtName.Text);
                }
                else
                {
                    MessageBox.Show("Thêm mới chuyên mục thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                }
            }
            else
            {
                MessageBox.Show("Thêm mới chuyên mục thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
            }
        }



        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (dgdData.SelectedRows.Count > 0)
            {
                if (txtName.Text.Trim() != "")
                {
                    bool rs = (new CategoryDAL()).UpdateCategory(new Category(Convert.ToInt32(dgdData.SelectedRows[0].Cells[0].Value), txtName.Text));
                    if (rs)
                    {
                        MessageBox.Show("Cập nhật chuyên mục thành công.");
                        LoadAllData();
                        NotifyEvent(txtName.Text);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật chuyên mục thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật chuyên mục thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
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
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (dgdData.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show(this, "Bạn có muốn xóa Chuyên Mục này không?\nLưu ý: Tất cả các Rss link và tin tức nằm trong chuyên mục này đều sẽ bị xóa.", "Chú ý", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    bool rs = (new CategoryDAL()).DeleteCategory(Convert.ToInt32(dgdData.SelectedRows[0].Cells[0].Value));
                    if (rs)
                    {
                        LoadAllData();
                        NotifyEvent("");
                    }
                    else
                    {
                        MessageBox.Show("Xóa chuyên mục thất bại. Vui lòng kiểm tra kết nối tới server.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 đối tượng để thực hiện thao tác!");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }




    }

}

