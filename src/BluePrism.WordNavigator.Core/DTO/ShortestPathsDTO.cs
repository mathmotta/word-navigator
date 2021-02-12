using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluePrism.WordNavigator.Core.DTO
{
    /// <summary>
    /// A DTO to hold navigation results in a nicer way.
    /// </summary>
    public class ShortestPathsDTO
    {
        private readonly ICollection<ICollection<string>> _navigationResults;

        public static ShortestPathsDTO CreateFrom(ICollection<ICollection<string>> navigationResults)
        {
            return new ShortestPathsDTO(navigationResults);
        }

        protected ShortestPathsDTO(ICollection<ICollection<string>> navigationResults)
        {
            _navigationResults = navigationResults;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var list in _navigationResults)
            {
                sb.Append("[ ");
                foreach (string item in list)
                {
                    sb.Append(item);
                    if (!list.Last().Equals(item))
                    {
                        sb.Append(" -> ");
                    }
                }
                sb.Append(" ]");
                if (!_navigationResults.Last().Equals(list))
                {
                    sb.Append(",");

                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
