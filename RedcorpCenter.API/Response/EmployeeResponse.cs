﻿namespace RedcorpCenter.API.Response
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public string area { get; set; }
        public string cargo { get; set; }
    }
}
