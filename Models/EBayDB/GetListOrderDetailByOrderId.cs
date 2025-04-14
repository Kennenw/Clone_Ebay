using System;
using System.Collections.Generic;

namespace Web_Clone_Ebay.Models.EBayDB;

public partial class GetListOrderDetailByOrderId
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? OrderDetail { get; set; }
}
