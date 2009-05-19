using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMerge
{
    class Hits : IEnumerable<Hit>
    {
        public List<Hit> HitsTable { get; set; }

        #region IEnumerable<Hit> Members

        public IEnumerator<Hit> GetEnumerator()
        {
            return HitsTable.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return HitsTable.GetEnumerator();

        }

        public void Add(Hit Item)
        {
            HitsTable.Add(Item);
        }

        public Hits()
        {
            HitsTable = new List<Hit>();
        }

        #endregion
    }
}
