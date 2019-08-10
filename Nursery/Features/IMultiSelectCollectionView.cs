using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Nursery.Features
{
    public interface IMultiSelectCollectionView
    {
        void AddControl(Selector selector);
        void RemoveControl(Selector selector);
    }
}
