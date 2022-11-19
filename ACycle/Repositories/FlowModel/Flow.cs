using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.Repositories.FlowModel
{
    public abstract class Flow
    {
        public string Name;

        public FlowDirection Direction;
    }

    public enum FlowDirection
    {
        IN,
        OUT
    }
}
