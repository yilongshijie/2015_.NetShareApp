using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class GoodCartLocalStorage
    {
        public IEnumerable<GoodCartLocalStorageItem> cache_shopcar;
    }
    public class GoodCartLocalStorageItem
    {
        public int goodchildid;
        public int num;
    }

}
