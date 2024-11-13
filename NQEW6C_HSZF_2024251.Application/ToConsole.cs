using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251.Application
{
    public class ToConsole
    {
        public void Display(object o)
        {
            if (o == null) { Console.WriteLine("Nincs megjeleníthető adat"); return; }

            foreach (PropertyInfo propInfo in o.GetType().GetProperties())
            {
                Console.WriteLine($"{propInfo.Name}: {propInfo.GetValue(o)?.ToString()}");
            }
        }
    }
}
