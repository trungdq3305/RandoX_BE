﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RandoX.Data.Entities;

public partial class Shipment
{
    public string Id { get; set; }

    public DateOnly? TestResultDispatchedDate { get; set; }

    public DateOnly? TestResultDeliveryDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public ulong? IsDeleted { get; set; }

    public virtual ICollection<ShipmentHistory> ShipmentHistories { get; set; } = new List<ShipmentHistory>();
}