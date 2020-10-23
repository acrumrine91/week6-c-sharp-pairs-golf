using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICityDAO
    {
        City GetVenueCity(int cityNum);
    }
}
