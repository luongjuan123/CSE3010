using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleCAdvance.Hinh
{
    public class HinhTron : IHinh
    {
        private double banKinh;
        public double BanKinh { get; 
            set
            {
                if (value < 0)
                {
                    throw new Exception("Bán kính không được âm");
                }
                else
                {
                    banKinh = value;
                }

            }
        }
        public HinhTron(double banKinh)
        {
            this.banKinh = banKinh;
        }
        public double getDienTich()
        {
            return Math.PI * banKinh * banKinh;
        }
        public double getChuVi()
        {
            return 2 * Math.PI * banKinh;
        }
    }
} 