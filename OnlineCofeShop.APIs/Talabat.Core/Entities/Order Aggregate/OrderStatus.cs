using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{

    ///How store OrderStatus at DataBase => if Pending => store [0],
    ///PaymentSucceeded => [1], PaymentFailed => [2] 
    ///We use Data Annotation => (EnumMember) To Change way of store value at DB
    ///for Example insteade of stored with [0] will stored (Pending)
    ///Which You will Store OrderStatus as String
    ///Enum:is a data type that allows you to define a set of named values,
    ///and variables of that type can only take one of those specific values.,
    ///typically representing states, options, or categories.

    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending, //Creation to order without Payment
        [EnumMember(Value = "Payment Received")]
        PaymentReceived,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,
    }
}
