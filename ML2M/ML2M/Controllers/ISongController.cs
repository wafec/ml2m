using ML2M.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Controllers
{
    public interface ISongController
    {
        List<Song> GetSongs(ResourceConfiguration resourceConfiguration);
    }
}
