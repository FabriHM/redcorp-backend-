﻿using RedcorpCenter.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedcorpCenter.Infraestructure
{
    public interface IEmployeeInfraestructure
    {
        Employee GetById(int id);
        public bool Save(Employee employee);
        public bool update(int id, string name, string last_name, string email, string area, string cargo);
        public bool delete(int id);

        Employee GetByLogin(string email, string password);
        Task<List<Employee>> GetAllAsync();
    }
}
