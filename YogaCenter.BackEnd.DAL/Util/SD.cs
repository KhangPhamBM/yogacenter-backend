using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Util
{
    public class SD
    {
        private static SD instance;
        public static SD getInstance()
        {
            if (instance == null)
            {
                instance = new SD();
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
            public static int SUCCESSFUL = 1;
            public static int FAILED = 2;
            public static int PENDING = 3;
        }

        public class PaymentType
        {
            public static int VNPAY = 1;
            public static int MOMO = 2;
        }

        public class TimeFrame
        {
            public static int TIMEFRAME_7H = 1;
            public static int TIMEFRAME_9H = 2;
            public static int TIMEFRAME_13H = 3;
            public static int TIMEFRAME_17H = 4;
            public static int TIMEFRAME_19H = 5;
        }
        public class SettingValue
        {
            public static int PAYINGTIME = 1;
            public static int REFUNDTIME = 2;

        }

        public class Room
        {
            public static int A01 = 1;
            public static int A02 = 2;
            public static int A03 = 3;
            public static int A04 = 4;
            public static int A05 = 5;
            public static int A06 = 6;
            public static int A07 = 7;
            public static int A08 = 8;
            public static int A09 = 9;
            public static int A10 = 10;

            public static int B01 = 11;
            public static int B02 = 12;
            public static int B03 = 13;
            public static int B04 = 14;
            public static int B05 = 15;
            public static int B06 = 16;
            public static int B07 = 17;
            public static int B08 = 18;
            public static int B09 = 19;
            public static int B10 = 20;

        }


        public class TicketType
        {
            public static int REFUND_TICKET = 1;
            public static int OTHER_TICKET = 2;

        }

        public class TicketStatus
        {
            public static int PENDING = 1;
            public static int APPROVED = 2;
            public static int REJECTED = 3;


        }

        public class AttendanceStatus
        {
            public static int NOT_YET = 1;
            public static int ATTENDED = 2;
            public static int ABSENT = 3;


        }
        public class ResponseMessage
        {
            public static string CREATE_SUCCESSFUL = "CREATE_SUCCESSFULLY";
            public static string UPDATE_SUCCESSFUL = "UPDATE_SUCCESSFULLY";
            public static string DELETE_SUCCESSFUL = "DELETE_SUCCESSFULLY";
            public static string CREATE_FAILED = "CREATE_FAILED";
            public static string UPDATE_FAILED = "UPDATE_FAILED";
            public static string DELETE_FAILED = "DELETE_FAILED";
            public static string LOGIN_FAILED = "LOGIN_FAILED";
        }
    }

}
