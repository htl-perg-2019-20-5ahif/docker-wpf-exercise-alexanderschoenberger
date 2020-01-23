using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Car
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string Make { get; set; }
    [Required]
    public string Type { get; set; }
}

public class BookedCars
{
    [Key]
    public int ID { get; set; }
    [Required]
    public int CarID { get; set; }
    public Car Car { get; set; }
    [Required]
    public DateTime? Date { get; set; }
}
/*
public class User
{
    [Key]
    public decimal ID { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}
*/
