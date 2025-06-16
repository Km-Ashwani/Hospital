using Hospital.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Auth
{
    public interface IJwtGenerate
    {
        string GenerateToken(AppUsers user, IEnumerable<string> role);
    }
}
