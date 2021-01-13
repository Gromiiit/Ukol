using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api.Users.Routes;
using Dropbox.Api;

namespace Notino.Homework.Storages.Clouds
{
    public class Dropbox : ICloud
    {
        public Dropbox()
        {
        }

        public async Task Connect()
        {
            using (var dbx = new DropboxClient("YOUR ACCESS TOKEN"))
            {
                //var full = await dbx.Users.GetAccountAsync();
                //Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
                throw new NotImplementedException();
            }
        }

        public Stream Download()
        {
            throw new NotImplementedException();
        }

        public Stream Upload()
        {
            throw new NotImplementedException();
        }
    }
}
