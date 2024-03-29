﻿namespace HouseRentingSystem.Data.Models;

using System.ComponentModel.DataAnnotations;

using static HouseRentingSystem.Common.EntityValidationConstants.CategoryConstants;
public class Category
{
    public Category()
    {
        this.Houses = new HashSet<House>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<House> Houses { get; set; }
}
