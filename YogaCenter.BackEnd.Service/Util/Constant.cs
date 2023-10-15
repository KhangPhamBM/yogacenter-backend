using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YogaCenter.BackEnd.Service.Util
{
    public class Constant
    {
        private static Constant instance;
        public static Constant getInstance()
        {
            if (instance == null)
            {
                instance = new Constant();
            }
            return instance;
        }
        public class Role
        {
            public static int ADMIN_ROLEID = 1;
            public static int STAFF_ROLEID = 2;
            public static int TRAINER_ROLEID = 3;
            public static int TRAINEE_ROLEID = 4;
        }

        /*public class Level
        {
            public static string BEGINNER_LEVEL = "Beginner";
            public static string INTERMEDIATE_LEVEL = "Intermediate";
            public static string ADVANCED_LEVEL = "Advanced";
        }*/

        /*public class FeedbackStatus
        {
            public static int UNCHECKED = 0;
            public static int APPROVED = 1;
            public static int REJECTED = 2;
        }*/

        /*public class BlogStatus
        {
            public static int UNCHECKED = 0;
            public static int APPROVED = 1;
            public static int REJECTED = 2;
        }*/

        public class Subscription
        {
            public static int PENDING_PAY_VNPAY = 0;
            public static int SUCCESS_PAY_VNPAY = 1;
            public static int FAIL_PAY_VNPAY = 2; 
            public static int PENDING_REFUND_VNPAY = 3;
            public static int FAIL_REFUND_VNPAY = 4;
            public static int SUCCESS_REFUND_VNPAY = 5;

            public static int PENDING_PAY_MOMO = 6;
            public static int SUCCESS_PAY_MOMO = 7;
            public static int FAIL_PAY_MOMO = 8;
            public static int PENDING_REFUND_MOMO = 9;
            public static int FAIL_REFUND_MOMO = 10;
            public static int SUCCESS_REFUND_MOMO = 11;

            public static int RESERVED = 12;

            public static int PENDING_PAY_BANK_TRANSFER = 13;
            public static int SUCCESS_PAY_BANK_TRANSFER = 14;
            public static int FAIL_PAY_BANK_TRANSFER = 15;
            public static int PENDING_REFUND_BANK_TRANSFER = 16;
            public static int FAIL_REFUND_BANK_TRANSFER = 17;
            public static int SUCCESS_REFUND_BANK_TRANSFER = 18;

        }

        public class SettingValue
        {
            public static int PAYINGTIME = 1;
            public static int REFUNDTIME = 2;
            public static int MINOFTRAINEE = 3;
            public static int MAXOFTRAINEE = 4;
        }
    }

}
