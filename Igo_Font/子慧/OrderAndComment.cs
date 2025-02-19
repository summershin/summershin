﻿using Igo_Font;
using IGO_font;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;


namespace IGO
{
    public partial class OrderAndComment : Form
    {
        public OrderAndComment()
        {
            InitializeComponent();

            LoadProducttoCombobox();

        }

        private void LoadProducttoCombobox()
        {
            var q = (from p in dbContext.OrderDetails.AsEnumerable()
                     where  p.Order.CustomerID == customer.customerID
                     orderby p.ProductID ascending
                    select $"{ p.ProductID,4}" +": "+ p.Product.ProductName).Distinct();

            if (q.Any())
            {
                foreach (var item in q.Distinct())
                {
                    this.Cb_Product.Items.Add(item);
                }
            }

        }

        IGOEntities dbContext = new IGOEntities();
        private void Rb_All_CheckedChanged(object sender, EventArgs e)
        {
            var q = from p in dbContext.OrderDetails
                    where  p.Order.CustomerID == customer.customerID
                    select new
                    {
                        p.OrderID,
                        p.Order.OrderDate,
                        p.Order.Payment.PayType,
                        p.Order.TotalPrice,
                        p.ProductID,
                        p.Product.ProductName,
                        p.TicketType.TicketName,
                        p.Quantity,
                        p.Order.Status.StatusName,

                    };

            DGV_OrderDetail.DataSource = q.ToList();

            //bs.DataSource = q.ToList();
            //DGV_OrderDetail.DataSource = bs;

            //Tb_Product.Text = Convert.ToString(DGV_OrderDetail.CurrentRow.Cells[4].Value);
           // Tb_Product.DataBindings.Add("Text", bs, "ProductName");



        }

     

        private void Rb_Cancel_CheckedChanged(object sender, EventArgs e)
        {
            var q = from p in dbContext.OrderDetails
                    where p.Order.Status.StatusName=="取消訂單" && p.Order.CustomerID== customer.customerID
                    select new
                    {
                        p.OrderID,
                        p.Order.OrderDate,
                        p.Order.Payment.PayType,
                        p.Order.TotalPrice,
                        p.ProductID,
                        p.Product.ProductName,
                        p.TicketType.TicketName,
                        p.Quantity,
                        p.Order.Status.StatusName,

                    };

            DGV_OrderDetail.DataSource = q.ToList();
            
            
            //Tb_Product.Text = Convert.ToString(DGV_OrderDetail.CurrentRow.Cells[4].Value);
        }

        private void Rb_CreditOrder_CheckedChanged(object sender, EventArgs e)
        {
            var q = from p in dbContext.OrderDetails
                    where p.Order.Payment.PayType == "銀行轉帳" && p.Order.CustomerID == customer.customerID
                    select new
                    {
                        p.OrderID,
                        p.Order.OrderDate,
                        p.Order.Payment.PayType,
                        p.Order.TotalPrice,
                        p.ProductID,
                        p.Product.ProductName,
                        p.TicketType.TicketName,
                        p.Quantity,
                        p.Order.Status.StatusName,

                    };

            DGV_OrderDetail.DataSource = q.ToList();
        }

        private void Rb_6Month_CheckedChanged(object sender, EventArgs e)
        {

            var day = DateTime.Now.Date;
           

            var q = from p in dbContext.OrderDetails
                    where DbFunctions.DiffDays( p.Order.OrderDate,day)<=90 && p.Order.CustomerID == customer.customerID
                    select new
                    {
                        p.OrderID,
                        p.Order.OrderDate,
                        p.Order.Payment.PayType,
                        p.Order.TotalPrice,
                        p.ProductID,
                        p.Product.ProductName,
                        p.TicketType.TicketName,
                        p.Quantity,
                        p.Order.Status.StatusName,

                    };

            DGV_OrderDetail.DataSource = q.ToList();
        }

        private void Rb_Month_CheckedChanged(object sender, EventArgs e)
        {
            var day = DateTime.Now.Date;


            var q = from p in dbContext.OrderDetails
                    where DbFunctions.DiffDays(p.Order.OrderDate, day) <= 30 && p.Order.CustomerID == customer.customerID
                    select new
                    {
                        p.OrderID,
                        p.Order.OrderDate,
                        p.Order.Payment.PayType,
                        p.Order.TotalPrice,
                        p.ProductID,
                        p.Product.ProductName,
                        p.TicketType.TicketName,
                        p.Quantity,
                        p.Order.Status.StatusName,

                    };

            DGV_OrderDetail.DataSource = q.ToList();
        }

        //--------------------------下面-----------------------------------
        private void Btn_Submit_Click(object sender, EventArgs e)
        {
            if (Tb_Comment.TextLength > 250)
            {
                MessageBox.Show("超過字數!");
                return;
            }


            else if (Cb_Product.Text =="")
            {
                MessageBox.Show("缺少必要產品ID!");
                return;
            }

            else if (Cb_Star.Text == "")
            {
                MessageBox.Show("缺少必要五星評分");
                return;
            }

            else  if (Tb_Comment.Text == "")
            {
                MessageBox.Show("缺少必要評論!");
                return;
            }
           
           
            
            
            string g = Cb_Product.Text.Substring(0,4);
            FeedbackManagement feedback = new FeedbackManagement
            {
                CustomerID = customer.customerID,
                FeedbackContent = Tb_Comment.Text,
                Ranking = Convert.ToInt32(Cb_Star.Text),
                ProductsID = Convert.ToInt32(g),
                
                FeedbackDate = DateTime.Now,
                
                
            };
            this.dbContext.FeedbackManagements.Add(feedback);

            this.dbContext.SaveChanges();
            MessageBox.Show("評論存取成功!");


        }




        BindingSource bs = new BindingSource();

        private void OrderAndComment_Load(object sender, EventArgs e)
        {
            
           
        }

        private void Gb_AfterQuery_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Lb_5Star_Click(object sender, EventArgs e)
        {

        }

        private void Cb_Star_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Lb_CommentProduct_Click(object sender, EventArgs e)
        {

        }

        private void Tb_Comment_TextChanged(object sender, EventArgs e)
        {

        }

        private void Cb_Product_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Gb_Button_Enter(object sender, EventArgs e)
        {

        }
    }
}
