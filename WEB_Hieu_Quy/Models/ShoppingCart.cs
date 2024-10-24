using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_Hieu_Quy.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }
        public void AddtoCart(ShoppingCartItem item, int quantity)
        {
            var checkSP = Items.FirstOrDefault(x => x.IdSanPham == item.IdSanPham);
            if (checkSP != null)
            {
                checkSP.SoLuong += quantity;
                checkSP.TongGia = checkSP.GiaSanPham * checkSP.SoLuong;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkSP = Items.SingleOrDefault(x => x.IdSanPham == id);
            if (checkSP != null)
            {
                Items.Remove(checkSP);
            }
        }
        public void UpdateQuantity(int id, int quantity)
        {
            var checkSP = Items.SingleOrDefault(x => x.IdSanPham == id);
            if (checkSP != null)
            {
                checkSP.SoLuong = quantity;
                checkSP.TongGia = checkSP.GiaSanPham * checkSP.SoLuong;
            }
        }
        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TongGia);
        }
         public decimal GetTotalQuantity()
        {
            return Items.Sum(x => x.SoLuong);
        }
        public void ClearCart()
        {
            Items.Clear();
        }

    }
    public class ShoppingCartItem
    {
        public int IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string AnhSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaSanPham { get; set; }
        public decimal TongGia { get; set; }

    }
}