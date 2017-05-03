using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UltimateChecker
{
    public class Lib
    {
        public static Dictionary<int, char> Signs = new Dictionary<int, char>() {
            { 1, 'A' },
            { 2, 'B' },
            { 3, 'C' },
            { 4, 'D' },
            { 5, 'E' },
            { 6, 'F' },
            { 7, 'G' },
            { 8, 'H' }
        };

        public static T CreateCopy<T>(T aobject)
        {
            if (aobject != null)
            {
                MethodInfo memberwiseClone = aobject.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                T Copy = (T)memberwiseClone.Invoke(aobject, null);
                foreach (FieldInfo f in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    object original = f.GetValue(aobject);
                    f.SetValue(Copy, CreateCopy(original));
                }
                return Copy;
            }
            else return default(T);
        }
       
        public static UserControl DraggingChecker { get; set; }
    }
}
