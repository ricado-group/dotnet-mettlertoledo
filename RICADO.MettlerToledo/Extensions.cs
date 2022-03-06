using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RICADO.MettlerToledo
{
    internal static class Extensions
    {
        internal static bool HasSequence(this List<byte> list, byte[] pattern)
        {
            if(list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            
            return list.ToArray().HasSequence(pattern);
        }

        internal static bool HasSequence(this byte[] array, byte[] pattern)
        {
            if(array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if(pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if(array.Length == 0 || pattern.Length == 0)
            {
                return false;
            }

            for(int i = 0; i < array.Length - pattern.Length + 1; i++)
            {
                if(array[i] != pattern[0])
                {
                    continue;
                }

                for(int j = pattern.Length - 1; j >= 1; j--)
                {
                    if(array[i + j] != pattern[j])
                    {
                        break;
                    }

                    if(j == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal static int IndexOf(this List<byte> list, byte[] pattern)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return list.ToArray().IndexOf(pattern);
        }

        internal static int IndexOf(this byte[] array, byte[] pattern)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (array.Length == 0 || pattern.Length == 0)
            {
                return -1;
            }

            for (int i = 0; i < array.Length - pattern.Length + 1; i++)
            {
                if (array[i] != pattern[0])
                {
                    continue;
                }

                for (int j = pattern.Length - 1; j >= 1; j--)
                {
                    if (array[i + j] != pattern[j])
                    {
                        break;
                    }

                    if (j == 1)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
