using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSProject.ServiceModules
{
    class Cryptography
    {
        public static string Encrypt(string s)
        {
            string enc = "";
            Stack<int> stack = new Stack<int>();
            foreach (var item in s.ToCharArray())
            {
                int i = Convert.ToSByte(item);
                stack.Push(i);
            }
            foreach (var item in stack)
            {
                enc += item + "-";
            }
            return enc;
        }
        public static string Decrypt(string s)
        {
            string dec = "";
            Stack<int> stack = new Stack<int>();
            string x = "";
            foreach (var item in s.ToCharArray())
            {
                if (item == '-') 
                {
                    int i = Convert.ToInt32(x);
                    stack.Push(i);
                    x = "";
                }
                else
                {
                    x += item;
                }
            }
            foreach (var item in stack)
            {
                dec += Convert.ToChar(item);
            }
            return dec;
        }
    }
}
