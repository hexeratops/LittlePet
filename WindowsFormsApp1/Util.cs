using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittlePet
{
    /// <summary>
    /// Some misc utility classes for helpful behaviour.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Allows for safely accessing windows components, regardless if you're
        /// running on a multithreaded environment or not.
        /// </summary>
        /// <param name="c">The control to invoke with.</param>
        /// <param name="action">The action to execute using the control.</param>
        public static void SafeInvoke(this Control c, Action action)
        {
            if(c.InvokeRequired)
            {
                c.Invoke(action);
            }
            else
            {
                action();
            }
        }



        /// <summary>
        /// Allows for safely accessing windows components, regardless if you're
        /// running on a multithreaded environment or not. This overloaded method
        /// allows yo uto return a property from the control.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c">The control to invoke with.</param>
        /// <param name="func">The action to execute using the control.</param>
        /// <returns></returns>
        public static T SafeInvoke<T>(this Control c, Func<T> func)
        {
            if (c.InvokeRequired)
            {
                return (T)c.Invoke(func);
            }
            else
            {
                return func();
            }
        }
    }
}
