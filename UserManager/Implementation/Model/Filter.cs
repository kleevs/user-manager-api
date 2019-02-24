﻿using UserManager.Model;

namespace UserManager.Implementation.Model
{
    public class Filter : IFilter
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
    }
}
