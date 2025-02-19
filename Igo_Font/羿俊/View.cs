﻿using Igo_Font;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IGO_font
{
    public partial class View : Form
    {
        public View()
        {
            InitializeComponent();

        }
        IGOEntities dbcontext = new IGOEntities();
        IEnumerable<TicketAndProduct> pp = null;

        private void View_Load(object sender, EventArgs e)
        {
            var q = dbcontext.Products.Select(n => n.City.City1).Distinct();
            foreach (string item in q)
            {
                cmb_City.Items.Add(item);
            }
            cmb_City.SelectedIndex = 0;

            //顯示所有行程
            var show = from p in dbcontext.Products.AsEnumerable()
                       where p.SubCategoryID == 2
                       select new
                       {
                           產品編號 = p.ProductID,
                           產品名稱 = p.ProductName,
                           產品數量 = p.Quantity,
                           產品地點 = p.City.City1,
                           開始時間 = p.StartTime,
                           結束時間 = p.EndTime


                       };
            dataGridView1.DataSource = show.ToList();

        }
        //===========================================================================================
        private void btn_Search_Click(object sender, EventArgs e)
        {
            pp = from p in dbcontext.TicketAndProducts.AsEnumerable()
                 select p;
            if (Check_Date.Checked == false && Check_City.Checked == false)
            {
                MessageBox.Show("請選擇查詢方式");

            }
            //新增查詢
            else
            {
                //依日期查詢
                if (Check_Date.Checked == true)
                {

                    var q = from p in dbcontext.Products.AsEnumerable()


                            where p.StartTime.Value.Date <= dateTimePicker1.Value.Date
                                  && p.EndTime.Value.Date >= dateTimePicker1.Value.Date
                                  && p.SubCategoryID == 2

                            select new
                            {
                                產品編號 = p.ProductID,

                                產品名稱 = p.ProductName,
                                產品數量 = p.Quantity,
                                產品地點 = p.City.City1,
                                開始時間 = p.StartTime,
                                結束時間 = p.EndTime,
                                票種 = p.TicketAndProducts,





                            };

                    if (q.FirstOrDefault() == null)
                    {

                        dataGridView1.DataSource = null;
                        MessageBox.Show("沒有產品");
                    }
                    else
                    {
                        dataGridView1.DataSource = q.ToList();

                    }
                }
                //依縣市查詢
                if (Check_City.Checked == true)
                {
                    var q = from p in dbcontext.Products.AsEnumerable()
                            where p.City.City1 == cmb_City.SelectedItem.ToString() && p.SubCategoryID == 2
                            select new
                            {
                                產品編號 = p.ProductID,
                                產品名稱 = p.ProductName,
                                產品數量 = p.Quantity,
                                產品地點 = p.City.City1,
                                開始時間 = p.StartTime,
                                結束時間 = p.EndTime


                            };
                    if (q.FirstOrDefault() == null)
                    {
                        dataGridView1.DataSource = null;
                        MessageBox.Show("沒有產品");

                    }
                    else
                    {
                        dataGridView1.DataSource = q.ToList();

                    }
                }
                //依日期及縣市查詢
                if (Check_City.Checked == true && Check_Date.Checked == true)
                {
                    var q = from p in dbcontext.Products.AsEnumerable()
                            where p.City.City1 == cmb_City.SelectedItem.ToString()
                            && p.StartTime.Value.Date <= dateTimePicker1.Value.Date
                                  && p.EndTime.Value.Date >= dateTimePicker1.Value.Date
                                  && p.SubCategoryID == 2
                            select new
                            {
                                產品編號 = p.ProductID,
                                產品名稱 = p.ProductName,
                                產品數量 = p.Quantity,
                                產品地點 = p.City.City1,
                                開始時間 = p.StartTime,
                                結束時間 = p.EndTime


                            };

                    if (q.FirstOrDefault() == null)
                    {
                        dataGridView1.DataSource = null;
                        MessageBox.Show("沒有產品");
                    }
                    else
                    {
                        dataGridView1.DataSource = q.ToList();

                    }
                }
            }
        }
        //============================================================================================
        ArrayList ShopCart = new ArrayList();

        Temp data = new Temp();

        List<Mycheck> mychecks = new List<Mycheck>();





        private void btn_Add_Click(object sender, EventArgs e)
        {
            Mycheck mycheck = new Mycheck();
            bool txt_Full = true;
            bool txt_Half = true;
            int txt_Fprice;
            int txt_Hprice;
            txt_Full = int.TryParse(txt_FullPrice.Text, out txt_Fprice);
            txt_Half = int.TryParse(txt_HalfPrice.Text, out txt_Hprice);
            int ticket = 0;

            if (txt_Full == true && txt_Half == true)
            {   //c用來抓產品數量
                if (dataGridView1.DataSource != null)
                {
                    int c = (int)dataGridView1.CurrentRow.Cells[2].Value;
                    if (txt_Fprice + txt_Hprice <= c)
                    {
                        var p3 = from p in dbcontext.TicketAndProducts.AsEnumerable()
                                 where p.ProductID == (int)dataGridView1.CurrentRow.Cells[0].Value
                                 select p;

                        var q2 = from p in dbcontext.Products.AsEnumerable()
                                 where p.ProductID == (int)dataGridView1.CurrentRow.Cells[0].Value
                                 select new
                                 {
                                     p.ProductID,
                                     p.ProductName,
                                     p.City.City1,
                                     //p.Address,
                                     //p.Supplier.CompanyName,
                                     選擇時間 = dateTimePicker1.Value.Date,
                                     //p.SubCategory.SubCategoryName,
                                     全票張數 = txt_FullPrice.Text,
                                     全票價錢 = p3.ToList()[0].Price,
                                     半票張數 = txt_HalfPrice.Text,
                                     半票價錢 = p3.ToList()[1].Price,
                                 };


                        mycheck.ProductID = q2.ToList()[0].ProductID;
                        mycheck.Ticket = int.Parse(q2.ToList()[0].全票張數) + int.Parse(q2.ToList()[0].半票張數);
                        mycheck.ProductName = q2.ToList()[0].ProductName;
                        mycheck.FullPrice1 = (int)p3.ToList()[0].Price;
                        mycheck.HalfPrice1 = (int)p3.ToList()[1].Price;

                        mychecks.Add(mycheck);

                        for (int i = 0; i < mychecks.Count; i++)
                        {
                            if (mychecks[i].ProductName == dataGridView1.CurrentRow.Cells[1].Value.ToString())
                            {
                                ticket += mychecks[i].Ticket;


                            }


                        }
                        if (ticket <= c)
                        {

                            foreach (var item in q2)
                            {
                                ShopCart.Add(item);

                            }


                            dataGridView2.DataSource = null;
                            dataGridView2.DataSource = ShopCart;

                        }
                        else
                        {
                            mychecks.RemoveAt(mychecks.Count - 1);
                            MessageBox.Show("沒有那麼多貨物");
                        }

                    }
                    else
                    {
                        MessageBox.Show("沒有那麼多訂單");
                    }
                }
                else
                {

                    MessageBox.Show("請選擇產品");
                }
            }

            else
            {
                MessageBox.Show("請輸入正確數量");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (ShopCart.Count != 0)
            {
                mychecks.RemoveAt(dataGridView2.CurrentRow.Index);
                ShopCart.RemoveAt(dataGridView2.CurrentRow.Index);
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = ShopCart;
            }
            else
            {
                MessageBox.Show("裡面無資料");
            }



        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ShopCart.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ShopCart;
        }



        private void btn_AddCart_Click(object sender, EventArgs e)
        {
            Mycheck mycheck = new Mycheck();
            bool txt_Full = true;
            bool txt_Half = true;
            int txt_Fprice;
            int txt_Hprice;
            txt_Full = int.TryParse(txt_FullPrice.Text, out txt_Fprice);
            txt_Half = int.TryParse(txt_HalfPrice.Text, out txt_Hprice);




            int c = (int)dataGridView1.CurrentRow.Cells[2].Value;



            var q2 = from p in dbcontext.Products.AsEnumerable()
                     where p.ProductID == (int)dataGridView2.CurrentRow.Cells[0].Value
                     select new
                     {
                         p.ProductID,
                         p.ProductName,
                         p.City.City1,
                         //p.Address,
                         //p.Supplier.CompanyName,
                         選擇時間 = dateTimePicker1.Value.Date,
                         //p.SubCategory.SubCategoryName,
                         全票張數 = txt_FullPrice.Text,

                         半票張數 = txt_HalfPrice.Text
                     };
            //單價初始化 ================================================================
            var q3 = from p in dbcontext.Products.AsEnumerable()
                     where p.ProductID == (int)dataGridView2.CurrentRow.Cells[0].Value
                     select p;

            var p3 = from p in dbcontext.TicketAndProducts.AsEnumerable()
                     where q3.ToList()[0].ProductID == p.ProductID
                     select p;




            //訂單編號產生
            Random r = new Random();
            string rrr = (r.Next(1, 100) * r.Next(1, 100)).ToString();
            //全票區
            data.ProductID = q3.ToList()[0].ProductID;
            data.TicketID = 6;
            data.Quantity = int.Parse(q2.ToList()[0].全票張數);
            data.TempOrder = rrr;
            data.TotalPrice = p3.ToList()[0].Price * int.Parse(txt_FullPrice.Text);
            data.CustomerID = customer.customerID;
            dbcontext.Temps.Add(data);
            dbcontext.SaveChanges();

            //================================================================================
            //半票區
            data.ProductID = q3.ToList()[0].ProductID;
            data.TicketID = 7;
            data.Quantity = int.Parse(q2.ToList()[0].半票張數);
            data.TotalPrice = p3.ToList()[1].Price * int.Parse(txt_HalfPrice.Text);
            data.TempOrder = rrr;
            data.CustomerID = customer.customerID;
            dbcontext.Temps.Add(data);
            dbcontext.SaveChanges();


            ShopCart.RemoveAt(dataGridView2.CurrentRow.Index);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ShopCart;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_ShowAll_Click(object sender, EventArgs e)
        {
            var show = from p in dbcontext.Products.AsEnumerable()
                       where p.SubCategoryID == 2
                       select new
                       {
                           產品編號 = p.ProductID,
                           產品名稱 = p.ProductName,
                           產品數量 = p.Quantity,
                           產品地點 = p.City.City1,
                           開始時間 = p.StartTime,
                           結束時間 = p.EndTime


                       };
            dataGridView1.DataSource = show.ToList();
        }

        private void txt_FullPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txt_HalfPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

     
    }
    public class Mycheck
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Ticket { get; set; }
        public int FullPrice1 { get; set; }
        public int HalfPrice1 { get; set; }
    }
}
