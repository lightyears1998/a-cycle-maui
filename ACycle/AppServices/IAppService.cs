using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.AppServices
{
    public interface IAppService
    {
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }
    }
}
