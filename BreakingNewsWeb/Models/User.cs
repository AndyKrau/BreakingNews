﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BreakingNewsWeb.Models
{
    public class User
    {
        // делаем поле id автоинкрементным
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        //public DateTime CreatedDatetime { get; set; }

    }

    public enum Role
    {
        admin = 0,
        user = 1
    }
}