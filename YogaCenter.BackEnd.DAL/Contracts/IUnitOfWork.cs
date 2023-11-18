using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Contracts
{
    public interface IUnitOfWork 
    {
       void SaveChange();

    }
}
