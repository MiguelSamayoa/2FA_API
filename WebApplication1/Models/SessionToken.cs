using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{

    public partial class SessionToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Creacion { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }

}