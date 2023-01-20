using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.Cli.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebAppTinhVanCat_aspnetcore.Services
{

    public class DiaGioi
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class Tinh : DiaGioi
    {
        public List<Huyen> Districts { get; set; }
    }

    public class Huyen : DiaGioi
    {
        public string Level { get; set; }
        public List<Xa> Wards { get; set; }
    }

    public class Xa : DiaGioi
    {
        public string Level { get; set; }
    }



    public class DiaGioiHanhChinhVN
    {


        public List<Tinh> TinhVN { get; set; }
        IWebHostEnvironment _env;

        public DiaGioiHanhChinhVN(IWebHostEnvironment env)
        {
            _env = env;

            string DataPath = Path.Combine(_env.ContentRootPath, "wwwroot/json/DiaGioiHanhChinhVN.json");// đường dẫn 

            using (StreamReader sr = File.OpenText(DataPath))
            {
                var obj = sr.ReadToEnd();
                TinhVN = JsonConvert.DeserializeObject<List<Tinh>>(obj);
            }

        }


        public string GetAddress(int tinh, int huyen, int xa)
        {

            var Tinh = TinhVN.Where(t => t.Id == tinh).FirstOrDefault();
            if (Tinh == null) return string.Empty;
            var Huyen = Tinh.Districts.Where(h => h.Id == huyen).FirstOrDefault();
            if (Huyen == null) return string.Empty;
            var Xa = Huyen.Wards.Where(x => x.Id == xa).FirstOrDefault();
            if (Xa == null) return string.Empty;

            StringBuilder Address = new StringBuilder();

            Address.Append(Xa.Name + ", ");
            Address.Append(Huyen.Name + ", ");
            Address.Append(Tinh.Name);

            return Address.ToString();
        }



    }

}