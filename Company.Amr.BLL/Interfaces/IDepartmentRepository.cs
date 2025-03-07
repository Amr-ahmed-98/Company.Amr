﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Amr.DAL.Models;

namespace Company.Amr.BLL.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department>GetAll();
        Department? Get(int id);


        int Add(Department model);
        int Update(Department model);
        int Delete(Department model);

    }
}
