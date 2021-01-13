using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Homework.Storages.Clouds
{
    interface ICloud
    {
        Task Connect();
        Stream Upload();
        Stream Download();
    }
}
