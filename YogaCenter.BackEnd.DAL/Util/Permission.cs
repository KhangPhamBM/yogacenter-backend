using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.DAL.Util
{
    public class Permission
    {
        public const string ADMIN = "ADMIN";
        public const string STAFF = "STAFF";
        public const string TRAINEE = "TRAINEE";
        public const string TRAINER = "TRAINER";

        public const string ALL = $"{ADMIN}, {STAFF},{TRAINEE}, {TRAINER}";
        public const string MANAGEMENT = $"{ADMIN},{STAFF}";
        public const string CLASS = $"{ADMIN}, {STAFF},{TRAINER}";




    }
}
